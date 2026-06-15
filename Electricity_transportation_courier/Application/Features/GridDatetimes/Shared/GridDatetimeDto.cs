namespace Application.Features.GridDatetimes
{
    public class GridDatetimeDto
    {
        public int Id { get; set; }
        public float AvailablePower { get; set; }
        public float RequiredPower { get; set; }
        public DateOnly ForecastDate { get; set; }
        public TimeOnly ForecastTimestamp { get; set; }
    }
}