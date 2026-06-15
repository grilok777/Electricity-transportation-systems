using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenerationService.Models.Generation
{
    public class GenerationHourData
    {
        [Key]
        public int Id { get; set; }
        public int PowerPlantDayID { get; set; }
        [ForeignKey("PowerPlantDayID")]
        public GenerationPowerPlantDay PlantDay { get; set; }
        public double ForecastKw { get; set; }
    }
}
