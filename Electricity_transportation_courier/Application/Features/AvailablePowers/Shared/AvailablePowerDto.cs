namespace Application.Features.AvailablePowers
{
    public class AvailablePowerDto
    {
        public int Id { get; set; }
        public int SubstationLineId { get; set; }
        public int DatetimeId { get; set; }
        public List<int> AvailablePowerPlants { get; set; } = new List<int>();
    }
}