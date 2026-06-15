using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GridService.Models.Grid
{
    public class GridSubstationLine
    {
        [Key]
        public int Id { get; set; }

        public int SubstationId { get; set; }
        [ForeignKey("SubstationId")]
        public GridSubstation Substation { get; set; }
        [MaxLength(255)] public string Name { get; set; }
        public double BaseLoadKw { get; set; }

        public ICollection<GridPowerPlant> PowerPlants { get; set; }
        public ICollection<GridNeedHourLine> NeedHourLines { get; set; }
        public ICollection<GridAvailablePower> AvailablePowers { get; set; }
    }
}
