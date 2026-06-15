using System.ComponentModel.DataAnnotations;

namespace GridService.Models.Grid
{
    public class GridDatetime
    {
        [Key]
        public int Id { get; set; }
        public double AvailablePower { get; set; }
        public double RequiredPower { get; set; }
        public DateOnly ForecastDate { get; set; }
        public TimeOnly ForecastTimestamp { get; set; }

        public ICollection<GridNeedHourLine> NeedHourLines { get; set; }
        public ICollection<GridAvailablePower> AvailablePowers { get; set; }
    }
}
