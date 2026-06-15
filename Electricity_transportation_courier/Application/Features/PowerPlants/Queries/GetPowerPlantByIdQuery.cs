using Domain.Interfaces;
using MediatR;

namespace Application.Features.PowerPlants  
{
    public record GetPowerPlantByIdQuery(int Id) : IRequest<PowerPlantDto>;

    public class GetPowerPlantByIdQueryHandler : IRequestHandler<GetPowerPlantByIdQuery, PowerPlantDto>
    {
        private readonly IGridUnitOfWork _uow;

        public GetPowerPlantByIdQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<PowerPlantDto> Handle(GetPowerPlantByIdQuery request, CancellationToken cancellationToken)
        {
            var p = await _uow.PowerPlants.GetByIdAsync(request.Id);
            if (p == null) throw new Exception("Електростанцію не знайдено.");

            return new PowerPlantDto
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
            };
        }
    }
}