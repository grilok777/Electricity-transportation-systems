using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenerationService.Models.Generation
{
    public class GenerationPowerPlant
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public GenerationOwnerPowerPlant Owner { get; set; }
        [MaxLength(32)] public string PlantType { get; set; }
        public double MaxCapacityKw { get; set; }
        [MaxLength(255)] public string Location { get; set; }
        [MaxLength(32)] public string Status { get; set; }
        public DateOnly DateCommissioning { get; set; }
        public DateOnly DateDecommissioning { get; set; }

        public ICollection<GenerationPowerPlantDay> PlantDays { get; set; }
    }
}
