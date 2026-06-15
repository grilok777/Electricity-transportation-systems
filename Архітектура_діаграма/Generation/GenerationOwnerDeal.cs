using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenerationService.Models.Generation
{
    public class GenerationOwnerDeal
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)] public string OwnerName { get; set; }
        [MaxLength(30)] public string NumberPhone { get; set; }
        [MaxLength(255)] public string PlaceLocation { get; set; }
        public DateOnly ConclusionDeal { get; set; }
        public DateOnly CompetionDeal { get; set; }

        public ICollection<GenerationOwnerPowerPlant> Owners { get; set; }

    }
}
