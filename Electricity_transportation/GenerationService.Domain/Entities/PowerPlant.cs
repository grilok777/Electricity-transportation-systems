using System;
using System.Collections.Generic;


namespace Domain.Entities
{
    public enum PlantType
    {
        Solar,
        Wind
    }

    public enum PlantStatus
    {
        Active,
        Maintenance,
        Offline
    }

    public class PowerPlant
    {

        public int Id { get; set; }

        public int DealId { get; set; }
        public OwnerDeal Deal { get; set; } = null!;

        public PlantType Type { get; set; }
        public float MaxCapacityKw { get; set; }
        public string Location { get; set; } = string.Empty;
        public PlantStatus Status { get; set; }

        public DateTime DateCommissioning { get; set; }
        public DateTime? DateDecommissioning { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public ICollection<PowerPlantDay> ForecastDays { get; set; } = new List<PowerPlantDay>();
    }
}
