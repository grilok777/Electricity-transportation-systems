
using Domain.Enum;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Substations
{
    public record SearchSubstationsQuery(float? MinCapacity, float? MaxCapacity, Status? Status) : IRequest<List<SubstationDto>>;

    public class SearchSubstationsQueryHandler : IRequestHandler<SearchSubstationsQuery, List<SubstationDto>>
    {
        private readonly IGridUnitOfWork _uow;
        public SearchSubstationsQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<SubstationDto>> Handle(SearchSubstationsQuery request, CancellationToken cancellationToken)
        {
            var results = await _uow.Substations.FindAsync(s =>
                (!request.MinCapacity.HasValue || s.MaxThroughputMw >= request.MinCapacity.Value) &&
                (!request.MaxCapacity.HasValue || s.MaxThroughputMw <= request.MaxCapacity.Value) &&
                (!request.Status.HasValue || s.Status == request.Status.Value)
            );

            return results.Select(s => new SubstationDto { Id = s.Id, Name = s.Name, Location = s.Location, MaxThroughputMw = s.MaxThroughputMw, Status = s.Status.ToString() }).ToList();
        }
    }
}