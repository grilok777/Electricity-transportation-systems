namespace Application.Features.Substations
{
    public class SubstationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public float MaxThroughputMw { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}