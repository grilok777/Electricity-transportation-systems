
namespace Domain.Entities
{
    public class HourData
    {
        public int Id { get; set; }
        public int PowerPlantDayId { get; set; }
        public PowerPlantDay PowerPlantDay { get; set; } = null!;
        public TimeOnly ForecastTimestamp { get; set; }
        public float ForecastedKw { get; set; }
    }
}
/*[
  { "hour": 0, "forecastedKw": 0.0 },
  { "hour": 1, "forecastedKw": 0.0 },
  { "hour": 2, "forecastedKw": 0.0 },
  { "hour": 3, "forecastedKw": 0.0 },
  { "hour": 4, "forecastedKw": 0.0 },
  { "hour": 5, "forecastedKw": 0.03 },
  { "hour": 6, "forecastedKw": 0.1 },
  { "hour": 7, "forecastedKw": 1.85 },
  { "hour": 8, "forecastedKw": 2.8 },
  { "hour": 9, "forecastedKw": 3.55 },
  { "hour": 10, "forecastedKw": 4.2 },
  { "hour": 11, "forecastedKw": 4.85 },
  { "hour": 12, "forecastedKw": 5.0 },
  { "hour": 13, "forecastedKw": 4.9 },
  { "hour": 14, "forecastedKw": 4.55 },
  { "hour": 15, "forecastedKw": 3.8 },
  { "hour": 16, "forecastedKw": 3.0 },
  { "hour": 17, "forecastedKw": 2.05 },
  { "hour": 18, "forecastedKw": 1.2 },
  { "hour": 19, "forecastedKw": 0.5 },
  { "hour": 20, "forecastedKw": 0.05 },
  { "hour": 21, "forecastedKw": 0.0 },
  { "hour": 22, "forecastedKw": 0.0 },
  { "hour": 23, "forecastedKw": 0.0 }
]*/