using Domain.Enum;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.PowerPlants  
{
    public record SearchPowerPlantsQuery(
        int? DealId,
        int? SubstationLineId,
        float? MinCapacity,
        Status? Status
    ) : IRequest<List<PowerPlantDto>>;

    public class SearchPowerPlantsQueryHandler : IRequestHandler<SearchPowerPlantsQuery, List<PowerPlantDto>>
    {
        private readonly IGridUnitOfWork _uow;

        public SearchPowerPlantsQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<PowerPlantDto>> Handle(SearchPowerPlantsQuery request, CancellationToken cancellationToken)
        {
            var results = await _uow.PowerPlants.FindAsync(p =>
                (!request.DealId.HasValue || p.DealId == request.DealId.Value) &&
                (!request.SubstationLineId.HasValue || p.SubstationLineId == request.SubstationLineId.Value) &&
                (!request.MinCapacity.HasValue || p.MaxCapacityKw >= request.MinCapacity.Value) &&
                (!request.Status.HasValue || p.Status == request.Status.Value)
            );

            return results.Select(p => new PowerPlantDto
            {
                Id = p.Id,
                DealId = p.DealId,
                SubstationLineId = p.SubstationLineId,
                Type = p.Type.ToString(),
                MaxCapacityKw = p.MaxCapacityKw,
                Location = p.Location,
                Status = p.Status.ToString(),
                DateCommissioning = p.DateCommissioning,
                DateDecommissioning = p.DateDecommissioning
            }).ToList();
        }
    }
}