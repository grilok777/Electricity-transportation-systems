
namespace Application.Forecasts
{
    public class ForecastSummaryDto
    {
        public int Id { get; set; }
        public DateOnly ForecastDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public float TotalDailyKw { get; set; } // Сума всіх годин
        public List<HourDataDto> HourlyForecasts { get; set; } = new();
    }
}
