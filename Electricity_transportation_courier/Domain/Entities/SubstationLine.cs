
namespace Domain.Entities
{
    public class SubstationLine
    {
        public int Id { get; set; }
        public int SubstationId { get; set; }
        public Substation Substation { get; set; } = null!;
        public string LineName { get; set; } = string.Empty;
        public float BaseLoadKw { get; set; }

        /*public ICollection<NeedForHourLine> Needs { get; set; } = new List<NeedForHourLine>();*/
        public ICollection<AvailablePower> AvailablePowers { get; set; } = new List<AvailablePower>();
    }
}
