using Domain.Interfaces;
using MediatR;

namespace Application.Features.GridDatetimes
{
    public record SearchGridDatetimesQuery(
        DateOnly? StartDate,
        DateOnly? EndDate,
        bool? DeficitOnly 
    ) : IRequest<List<GridDatetimeDto>>;

    public class SearchGridDatetimesQueryHandler : IRequestHandler<SearchGridDatetimesQuery, List<GridDatetimeDto>>
    {
        private readonly IGridUnitOfWork _uow;
        public SearchGridDatetimesQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<GridDatetimeDto>> Handle(SearchGridDatetimesQuery request, CancellationToken cancellationToken)
        {
            var results = await _uow.Datetimes.FindAsync(d =>
                (!request.StartDate.HasValue || d.ForecastDate >= request.StartDate.Value) &&

                (!request.EndDate.HasValue || d.ForecastDate <= request.EndDate.Value) &&

                // Фільтр дефіциту потужності (коли потрібно більше, ніж є)
                (!request.DeficitOnly.HasValue || !request.DeficitOnly.Value || d.RequiredPower > d.AvailablePower)
            );

            var sorted = results.OrderBy(d => d.ForecastDate).ThenBy(d => d.ForecastTimestamp);

            return sorted.Select(d => new GridDatetimeDto
            {
                Id = d.Id,
                AvailablePower = d.AvailablePower,
                RequiredPower = d.RequiredPower,
                ForecastDate = d.ForecastDate,
                ForecastTimestamp = d.ForecastTimestamp
            }).ToList();
        }
    }
}