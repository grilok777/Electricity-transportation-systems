namespace Application.Deals
{
    public class CreateDealDto
    {
        public string OwnerName { get; set; } = string.Empty;
        public string NumberPhone { get; set; } = string.Empty;
        public string PlaceLocation { get; set; } = string.Empty;
        public DateTime ConclusionDeal { get; set; }
        public DateTime CompletionDeal { get; set; }
    }
}
