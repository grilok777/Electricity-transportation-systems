using Domain.Entities;
using Domain.Enum; 
using Domain.Interfaces;
using MediatR;

namespace Application.Features.PowerPlants
{
    public record CreatePowerPlantCommand(
        int DealId,
        int SubstationLineId,
        PlantType Type,
        float MaxCapacityKw,
        string Location,
        DateOnly DateCommissioning
    ) : IRequest<int>;

    public class CreatePowerPlantCommandHandler : IRequestHandler<CreatePowerPlantCommand, int>
    {
        private readonly IGridUnitOfWork _uow;

        public CreatePowerPlantCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<int> Handle(CreatePowerPlantCommand request, CancellationToken cancellationToken)
        {
            var deal = await _uow.OwnerDeals.GetByIdAsync(request.DealId);
            if (deal == null) throw new Exception("Угоду не знайдено.");

            var line = await _uow.SubstationLines.GetByIdAsync(request.SubstationLineId);
            if (line == null) throw new Exception("Лінію підстанції не знайдено.");

            var plant = new PowerPlant
            {
                DealId = request.DealId,
                SubstationLineId = request.SubstationLineId,
                Type = request.Type,
                MaxCapacityKw = request.MaxCapacityKw,
                Location = request.Location,
                Status = Status.Active, 
                DateCommissioning = request.DateCommissioning
            };

            await _uow.PowerPlants.AddAsync(plant);
            await _uow.SaveChangesAsync();

            return plant.Id;
        }
    }
}