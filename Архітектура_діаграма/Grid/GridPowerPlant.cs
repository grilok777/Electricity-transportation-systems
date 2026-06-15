using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GridService.Models.Grid
{
    public class GridPowerPlant
    {
        [Key]
        public int Id { get; set; }
        public int SubstationLineId { get; set; }
        [ForeignKey("SubstationLineId")]
        public GridSubstationLine SubstationLine { get; set; }
        [MaxLength(32)] public string PlantType { get; set; }
        public double MaxCapacityKw { get; set; }
        [MaxLength(255)] public string Location { get; set; }
        [MaxLength(32)] public string Status { get; set; }
        public DateOnly DateCommissioning { get; set; }
        public DateOnly DateDecommissioning { get; set; }

        public ICollection<GridPowerPlantDay> PlantDays { get; set; }
    }
}
