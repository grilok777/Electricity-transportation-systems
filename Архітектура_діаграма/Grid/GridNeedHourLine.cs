using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GridService.Models.Grid
{
    public class GridNeedHourLine
    {
        [Key]
        public int Id { get; set; }
        public int SubstationLineId { get; set; }
        [ForeignKey("SubstationLineId")]
        public GridSubstationLine SubstationLine { get; set; }
        public int DatetimeId { get; set; }
        [ForeignKey("DatetimeId")]
        public GridDatetime Datetime { get; set; }

    }
}
