
using Domain.Enum;

namespace Domain.Entities
{
    public class Substation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public float MaxThroughputMw { get; set; }
        public Status Status { get; set; }
        public ICollection<SubstationLine> SubstationLines { get; set; } = new List<SubstationLine>();
    }
}
