
using Domain.Interfaces;
using MediatR;

namespace Application.Features.SubstationLines
{
    public record SearchSubstationLinesQuery(
        int? SubstationId,
        float? MinBaseLoad,
        float? MaxBaseLoad,
        string? SearchTerm
    ) : IRequest<List<SubstationLineDto>>;

    public class SearchSubstationLinesQueryHandler : IRequestHandler<SearchSubstationLinesQuery, List<SubstationLineDto>>
    {
        private readonly IGridUnitOfWork _uow;

        public SearchSubstationLinesQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<SubstationLineDto>> Handle(SearchSubstationLinesQuery request, CancellationToken cancellationToken)
        {
            var lines = await _uow.SubstationLines.FindAsync(l =>
                (!request.SubstationId.HasValue || l.SubstationId == request.SubstationId.Value) &&

                (!request.MinBaseLoad.HasValue || l.BaseLoadKw >= request.MinBaseLoad.Value) &&

                (!request.MaxBaseLoad.HasValue || l.BaseLoadKw <= request.MaxBaseLoad.Value) &&

                (string.IsNullOrEmpty(request.SearchTerm) || l.LineName.Contains(request.SearchTerm))
            );

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