using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenerationService.Models.Generation
{
    public class GenerationPowerPlantDay
    {
        [Key]
        public int Id { get; set; }
        public int PlantId { get; set; }
        [ForeignKey("PlantId")]
        public GenerationPowerPlant Plant { get; set; }
        public DateOnly ForecastDate { get; set; }
        public TimeOnly ForecastTimestamp { get; set; }
        [MaxLength(32)] public string Status { get; set; }
        public int RetryCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ProcessedAt { get; set; }

        public ICollection<GenerationHourData> HourData { get; set; }

    }
}
