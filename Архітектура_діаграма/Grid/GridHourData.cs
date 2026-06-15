using GridService.Models.Grid;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GridService.Models.Grid
{
    public class GridHourData
    {
        [Key]
        public int Id { get; set; }
        public int PowerPlantDayID { get; set; }
        [ForeignKey("PowerPlantDayID")]
        public GridPowerPlantDay PowerPlantDay { get; set; }
        public double ForecastKw { get; set; }
    }
}
