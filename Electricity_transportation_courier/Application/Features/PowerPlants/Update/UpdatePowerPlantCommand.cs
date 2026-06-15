using Domain.Enum;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.PowerPlants  
{
    public record UpdatePowerPlantCommand(
        int Id,
        int SubstationLineId,
        float MaxCapacityKw,
        Status Status,
        DateOnly? DateDecommissioning 
    ) : IRequest;

    public class UpdatePowerPlantCommandHandler : IRequestHandler<UpdatePowerPlantCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public UpdatePowerPlantCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(UpdatePowerPlantCommand request, CancellationToken cancellationToken)
        {
            var plant = await _uow.PowerPlants.GetByIdAsync(request.Id);
            if (plant == null) throw new Exception("Електростанцію не знайдено.");

            // Якщо лінію змінили, перевіряємо, чи існує нова лінія
            if (plant.SubstationLineId != request.SubstationLineId)
            {
                var line = await _uow.SubstationLines.GetByIdAsync(request.SubstationLineId);
                if (line == null) throw new Exception("Нову лінію підстанції не знайдено.");
                plant.SubstationLineId = request.SubstationLineId;
            }

            plant.MaxCapacityKw = request.MaxCapacityKw;
            plant.Status = request.Status;
            plant.DateDecommissioning = request.DateDecommissioning;

            _uow.PowerPlants.Update(plant);
            await _uow.SaveChangesAsync();
        }
    }
}