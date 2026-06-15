using Domain.Enum;


namespace Domain.Entities
{

    public class PowerPlant
    {

        public int Id { get; set; }

        public int DealId { get; set; }
        public OwnerDeal Deal { get; set; } = null!;

        public int SubstationLineId { get; set; }
        public SubstationLine SubstationLine { get; set; } = null!;

        public PlantType Type { get; set; }
        public float MaxCapacityKw { get; set; }
        public string Location { get; set; } = string.Empty;
        public Status Status { get; set; }

        public DateOnly DateCommissioning { get; set; }
        public DateOnly? DateDecommissioning { get; set; }
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PowerPlantDay> ForecastDays { get; set; } = new List<PowerPlantDay>();
    }
}
