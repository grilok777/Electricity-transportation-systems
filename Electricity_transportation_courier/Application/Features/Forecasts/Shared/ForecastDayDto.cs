namespace Application.Features.Forecasts
{
    public class ForecastDayDto
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public DateOnly ForecastDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }

        // Зручне поле для оператора (сума за день)
        public float TotalDailyKw { get; set; }

        // Вкладений масив годин
        public List<HourDataDto> HourlyForecasts { get; set; } = new List<HourDataDto>();
    }
}