using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenerationService.Models.Generation
{
    public class GenerationOwnerPowerPlant
    {
        [Key]
        public int Id { get; set; }
        public int DealId { get; set; }
        [ForeignKey("DealId")]
        public GenerationOwnerDeal Deal { get; set; }
        [MaxLength(255)] public string Username { get; set; }

        [MaxLength(32)] public string Password { get; set; }
        public DateOnly DateRegistration { get; set; }

        public ICollection<GenerationPowerPlant> Plants { get; set; }
    }
}
