

using Domain.Enum;

namespace Domain.Entities
{


    public class PowerPlantDay
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public PowerPlant PowerPlant { get; set; } = null!;

        public DateOnly ForecastDate { get; set; }
        public DayStatus Status { get; set; } = DayStatus.Pending;
        public int RetryCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }

        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

        private readonly List<HourData> _hourDatas = new();
        public IReadOnlyCollection<HourData> HourDatas => _hourDatas.AsReadOnly();

        // Валідація/додавання:
        public void AddHourForecast(int hour, float forecastedKw, float plantMaxCapacityKw)
        {
            if (_hourDatas.Count >= 24)
                throw new InvalidOperationException("День вже містить максимальну кількість годинних записів (24).");

            if (forecastedKw < 0)
                throw new ArgumentException("Прогнозована потужність не може бути меншою за нуль.");

            if (forecastedKw > plantMaxCapacityKw)
                throw new ArgumentException($"Прогноз ({forecastedKw} кВт) перевищує максимальну потужність станції ({plantMaxCapacityKw} кВт).");

            _hourDatas.Add(new HourData
            {
                PowerPlantDay = this,
                ForecastTimestamp = new TimeOnly(hour, 0), 
                ForecastedKw = forecastedKw
            });
        }
    }
}
