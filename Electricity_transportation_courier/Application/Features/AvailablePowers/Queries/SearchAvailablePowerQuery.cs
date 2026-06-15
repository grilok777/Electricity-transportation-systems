using Domain.Interfaces;
using MediatR;

namespace Application.Features.AvailablePowers
{
    public record SearchAvailablePowerQuery(
        int? SubstationLineId,
        int? DatetimeId
    ) : IRequest<List<AvailablePowerDto>>;

    public class SearchAvailablePowerQueryHandler : IRequestHandler<SearchAvailablePowerQuery, List<AvailablePowerDto>>
    {
        private readonly IGridUnitOfWork _uow;
        public SearchAvailablePowerQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<AvailablePowerDto>> Handle(SearchAvailablePowerQuery request, CancellationToken cancellationToken)
        {
            var results = await _uow.AvailablePowers.FindAsync(a =>
                (!request.SubstationLineId.HasValue || a.SubstationLineId == request.SubstationLineId.Value) &&
                (!request.DatetimeId.HasValue || a.DatetimeId == request.DatetimeId.Value)
            );

            return results.Select(a => new AvailablePowerDto
            {
                Id = a.Id,
                SubstationLineId = a.SubstationLineId,
                DatetimeId = a.DatetimeId,
                AvailablePowerPlants = a.AvailablePowerPlants
            }).ToList();
        }
    }
}