
namespace Domain.Entities
{
    public class AvailablePower
    {
        public int Id { get; set; }
        public int SubstationLineId { get; set; }
        public SubstationLine SubstationLine { get; set; } = null!;
        public int DatetimeId { get; set; }
        public GridDatetime Datetime { get; set; } = null!;
        public List<int> AvailablePowerPlants { get; set; } = new List<int>();
    }
}
