using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Workers
{
    public class GridDealPlantSyncWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GridDealPlantSyncWorker> _logger;
        // URL вашого Grid API
        private readonly string _gridApiUrl = "https://localhost:7100/api/sync";

        public GridDealPlantSyncWorker(IServiceProvider serviceProvider, ILogger<GridDealPlantSyncWorker> logger)
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
                    _logger.LogInformation("Початок синхронізації з Grid...");

                    // Cтворюємо Scope
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<GenerationDbContext>();

                    // 1. Знаходимо дату останньої синхронізації для Угод
                    var lastDealSync = await db.OwnerDeals.MaxAsync(d => (DateTime?)d.LastModifiedAt) ?? DateTime.MinValue;

                    // 2. Тягнемо змінені Угоди з Grid
                    var dealsResponse = await httpClient.GetFromJsonAsync<List<OwnerDeal>>($"{_gridApiUrl}/deals?since={lastDealSync:O}", stoppingToken);

                    if (dealsResponse != null && dealsResponse.Any())
                    {
                        foreach (var gridDeal in dealsResponse)
                        {
                            var existingDeal = await db.OwnerDeals.FindAsync(gridDeal.Id);
                            if (existingDeal == null)
                            {
                                // Вставляємо НОВУ угоду (UserId залишається null)
                                db.OwnerDeals.Add(gridDeal);
                            }
                            else
                            {
                                // Оновлюємо існуючу (але ЗБЕРІГАЄМО UserId, якщо адмін його вже проставив!)
                                existingDeal.OwnerName = gridDeal.OwnerName;
                                existingDeal.NumberPhone = gridDeal.NumberPhone;
                                existingDeal.PlaceLocation = gridDeal.PlaceLocation;
                                existingDeal.ConclusionDeal = gridDeal.ConclusionDeal;
                                existingDeal.CompetionDeal = gridDeal.CompetionDeal;
                                existingDeal.LastModifiedAt = gridDeal.LastModifiedAt;
                                db.OwnerDeals.Update(existingDeal);
                            }
                        }
                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation($"Синхронізовано {dealsResponse.Count} угод.");
                    }

                    // 3. Знаходимо дату останньої синхронізації для Станцій
                    var lastPlantSync = await db.PowerPlants.MaxAsync(p => (DateTime?)p.LastModifiedAt) ?? DateTime.MinValue;

                    // 4. Тягнемо змінені Станції з Grid
                    var plantsResponse = await httpClient.GetFromJsonAsync<List<PowerPlant>>($"{_gridApiUrl}/plants?since={lastPlantSync:O}", stoppingToken);

                    if (plantsResponse != null && plantsResponse.Any())
                    {
                        foreach (var gridPlant in plantsResponse)
                        {
                            var existingPlant = await db.PowerPlants.FindAsync(gridPlant.Id);
                            if (existingPlant == null)
                            {
                                db.PowerPlants.Add(gridPlant);
                            }
                            else
                            {
                                existingPlant.DealId = gridPlant.DealId;
                                existingPlant.Type = gridPlant.Type;
                                existingPlant.MaxCapacityKw = gridPlant.MaxCapacityKw;
                                existingPlant.Location = gridPlant.Location;
                                existingPlant.Status = gridPlant.Status;
                                existingPlant.DateCommissioning = gridPlant.DateCommissioning;
                                existingPlant.DateDecommissioning = gridPlant.DateDecommissioning;
                                existingPlant.LastModifiedAt = gridPlant.LastModifiedAt;
                                db.PowerPlants.Update(existingPlant);
                            }
                        }
                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation($"Синхронізовано {plantsResponse.Count} станцій.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Помилка синхронізації: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}