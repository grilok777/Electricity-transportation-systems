
namespace Domain.Entities
{
    public class temp_NeedForHourLine
    {
        public int Id { get; set; }
        public int SubstationLineId { get; set; }
        public SubstationLine SubstationLine { get; set; } = null!;
        public int DatetimeId { get; set; }
        public GridDatetime Datetime { get; set; } = null!;
    }
}
