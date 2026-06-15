namespace Application.Features.PowerPlants
{
    public class PowerPlantDto
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int SubstationLineId { get; set; }
        public string Type { get; set; } = string.Empty;
        public float MaxCapacityKw { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateOnly DateCommissioning { get; set; }
        public DateOnly? DateDecommissioning { get; set; } 
    }
}