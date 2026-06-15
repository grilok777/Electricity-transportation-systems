using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GridService.Models.Grid
{
    public class GridAvailablePower
    {
        [Key]
        public int Id { get; set; }
        public int SubstationLineId { get; set; }
        [ForeignKey("SubstationLineId")]
        public GridSubstationLine SubstationLine { get; set; }
        public int DatetimeId { get; set; }
        [ForeignKey("DatetimeId")]
        public GridDatetime Datetime { get; set; }
        public List<int> AvailablePowerPlants { get; set; }
    }
}
