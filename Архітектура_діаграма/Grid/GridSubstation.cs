using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GridService.Models.Grid
{
    public class GridSubstation
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)] public string Name { get; set; }
        public double MaxThroughputKw { get; set; }
        [MaxLength(255)] public string Location { get; set; }
        [MaxLength(48)] public string Status { get; set; }

        public ICollection<GridSubstationLine> SubstationLines { get; set; }
    }
}
