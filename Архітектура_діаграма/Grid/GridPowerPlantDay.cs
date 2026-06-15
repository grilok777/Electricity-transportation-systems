using GridService.Models.Grid;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GridService.Models.Grid
{
    public class GridPowerPlantDay
    {
        [Key]
        public int Id { get; set; }
        public int PlantId { get; set; }
        [ForeignKey("PlantId")]
        public GridPowerPlant Plant { get; set; }
        public DateOnly ForecastDate { get; set; }
        public TimeOnly ForecastTimestamp { get; set; }
        [MaxLength(32)] public string Status { get; set; }
        public int RetryCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ProcessedAt { get; set; }

        public ICollection<GridHourData> HourData { get; set; }
    }
}
