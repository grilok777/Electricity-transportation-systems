using Domain.Interfaces;
using MediatR;

namespace Application.Features.OwnerDeals
{
    // SearchTerm - для пошуку по імені власника
    // OnlyActive - якщо true, поверне тільки ті угоди, де CompletionDeal == null (ще діють) 
    //              або CompletionDeal > сьогоднішньої дати
    public record SearchOwnerDealsQuery(string? SearchTerm, bool? OnlyActive) : IRequest<List<OwnerDealDto>>;

    public class SearchOwnerDealsQueryHandler : IRequestHandler<SearchOwnerDealsQuery, List<OwnerDealDto>>
    {
        private readonly IGridUnitOfWork _uow;

        public SearchOwnerDealsQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<OwnerDealDto>> Handle(SearchOwnerDealsQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var results = await _uow.OwnerDeals.FindAsync(d =>
                (string.IsNullOrEmpty(request.SearchTerm) || d.OwnerName.Contains(request.SearchTerm)) &&

                // Перевірка на активність угоди
                (!request.OnlyActive.HasValue || !request.OnlyActive.Value ||
                 d.CompetionDeal == null || d.CompetionDeal > today)
            );

            return results.Select(d => new OwnerDealDto
            {
                Id = d.Id,
                OwnerName = d.OwnerName,
                NumberPhone = d.NumberPhone,
                PlaceLocation = d.PlaceLocation,
                ConclusionDeal = d.ConclusionDeal,
                CompetionDeal = d.CompetionDeal
            }).ToList();
        }
    }
}