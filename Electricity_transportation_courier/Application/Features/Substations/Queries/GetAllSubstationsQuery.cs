
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Substations
{
    public record GetAllSubstationsQuery() : IRequest<List<SubstationDto>>;

    public class GetAllSubstationsQueryHandler : IRequestHandler<GetAllSubstationsQuery, List<SubstationDto>>
    {
        private readonly IGridUnitOfWork _uow;
        public GetAllSubstationsQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<SubstationDto>> Handle(GetAllSubstationsQuery request, CancellationToken cancellationToken)
        {
            var substations = await _uow.Substations.GetAllAsync();
            return substations.Select(s => new SubstationDto { Id = s.Id, Name = s.Name, Location = s.Location, MaxThroughputMw = s.MaxThroughputMw, Status = s.Status.ToString() }).ToList();
        }
    }
}