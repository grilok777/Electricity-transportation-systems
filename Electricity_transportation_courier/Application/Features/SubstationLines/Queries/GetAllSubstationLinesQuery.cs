
using Domain.Interfaces;
using MediatR;

namespace Application.Features.SubstationLines
{
    public record GetAllSubstationLinesQuery() : IRequest<List<SubstationLineDto>>;

    public class GetAllSubstationLinesQueryHandler : IRequestHandler<GetAllSubstationLinesQuery, List<SubstationLineDto>>
    {
        private readonly IGridUnitOfWork _uow;

        public GetAllSubstationLinesQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<SubstationLineDto>> Handle(GetAllSubstationLinesQuery request, CancellationToken cancellationToken)
        {
            var lines = await _uow.SubstationLines.GetAllAsync();

            return lines.Select(l => new SubstationLineDto
            {
                Id = l.Id,
                SubstationId = l.SubstationId,
                LineName = l.LineName,
                BaseLoadKw = l.BaseLoadKw
            }).ToList();
        }
    }
}