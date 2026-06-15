namespace Application.Features.SubstationLines
{
    public class SubstationLineDto
    {
        public int Id { get; set; }
        public int SubstationId { get; set; }
        public string LineName { get; set; } = string.Empty;
        public float BaseLoadKw { get; set; }
    }
}