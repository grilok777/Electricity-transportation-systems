namespace Application.Features.OwnerDeals
{
    public class OwnerDealDto
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string NumberPhone { get; set; } = string.Empty;
        public string PlaceLocation { get; set; } = string.Empty;
        public DateOnly ConclusionDeal { get; set; }
        public DateOnly? CompetionDeal { get; set; }
    }
}