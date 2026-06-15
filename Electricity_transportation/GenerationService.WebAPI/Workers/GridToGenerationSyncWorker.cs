using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Workers
{
    public class GridToGenerationSyncWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GridToGenerationSyncWorker> _logger;
        private readonly string _remoteApiUrl = "https://localhost:7100/api/sync"; 

        public GridToGenerationSyncWorker(IServiceProvider serviceProvider, ILogger<GridToGenerationSyncWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var httpClient = new HttpClient();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<GenerationDbContext>();

                    var lastSync = await db.PowerPlantDays.MaxAsync(d => (DateTime?)d.LastModifiedAt) ?? DateTime.MinValue;
                    var response = await httpClient.GetFromJsonAsync<List<PowerPlantDay>>($"{_remoteApiUrl}/forecasts-full?since={lastSync:O}", stoppingToken);

                    if (response != null && response.Any())
                    {
                        bool anyChangesSaved = false;

                        foreach (var remoteDay in response)
                        {
                            // 1. Шукаємо за ПРИРОДНИМ КЛЮЧЕМ (не за ID!)
                            var localDay = await db.PowerPlantDays
                                .Include(d => d.HourDatas)
                                .FirstOrDefaultAsync(d => d.PlantId == remoteDay.PlantId
                                                       && d.ForecastDate == remoteDay.ForecastDate
                                                       && d.RetryCount == remoteDay.RetryCount, stoppingToken);

                            if (localDay == null)
                            {
                                // СЦЕНАРІЙ А: Запису немає. Вставляємо як новий.
                                remoteDay.Id = 0; // Скидаємо чужий PK
                                foreach (var h in remoteDay.HourDatas) h.Id = 0; // Скидаємо чужі PK годин

                                db.PowerPlantDays.Add(remoteDay);
                                anyChangesSaved = true;
                            }
                            else
                            {
                                // СЦЕНАРІЙ Б: Запис є. Робимо DEEP COMPARE (Глибоке порівняння)
                                bool isModified = false;

                                // Перевіряємо поля самого дня
                                if (localDay.Status != remoteDay.Status ||
                                    localDay.ProcessedAt != remoteDay.ProcessedAt)
                                {
                                    localDay.Status = remoteDay.Status;
                                    localDay.ProcessedAt = remoteDay.ProcessedAt;
                                    isModified = true;
                                }

                                // Перевіряємо кожну годину
                                foreach (var remoteHour in remoteDay.HourDatas)
                                {
                                    var localHour = localDay.HourDatas
                                        .FirstOrDefault(h => h.ForecastTimestamp == remoteHour.ForecastTimestamp);

                                    if (localHour == null)
                                    {
                                        remoteHour.Id = 0;
                                        remoteHour.PowerPlantDayId = localDay.Id;
                                        db.HourDatas.Add(remoteHour);

                                        isModified = true;
                                    }
                                    else if (localHour.ForecastedKw != remoteHour.ForecastedKw)
                                    {
                                        // Значення кіловат змінилося!
                                        localHour.ForecastedKw = remoteHour.ForecastedKw;
                                        isModified = true;
                                    }
                                }

                                // ЗБЕРІГАЄМО ТІЛЬКИ ЯКЩО БУЛИ ЗМІНИ! Це розриває нескінченну петлю.
                                if (isModified)
                                {
                                    localDay.LastModifiedAt = remoteDay.LastModifiedAt; // Синхронізуємо час
                                    db.PowerPlantDays.Update(localDay);
                                    anyChangesSaved = true;
                                }
                            }
                        }

                        if (anyChangesSaved)
                        {
                            await db.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation($"Успішно синхронізовано оновлення для {response.Count} днів.");
                        }
                    }
                }
                catch (Exception ex) { _logger.LogError($"Помилка двосторонньої синхронізації: {ex.Message}"); }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); 
            }
        }
    }
}