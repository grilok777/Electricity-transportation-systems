
namespace Application.Forecasts
{

    public class SubmitForecastDto
    {
        public DateOnly Date { get; set; }
        public List<HourDataDto> HourlyForecasts { get; set; } = new();
    }
}
