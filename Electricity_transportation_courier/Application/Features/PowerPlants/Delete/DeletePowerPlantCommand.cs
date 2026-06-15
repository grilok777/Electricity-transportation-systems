using Domain.Interfaces;
using MediatR;

namespace Application.Features.PowerPlants
{
    public record DeletePowerPlantCommand(int Id) : IRequest;

    public class DeletePowerPlantCommandHandler : IRequestHandler<DeletePowerPlantCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public DeletePowerPlantCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(DeletePowerPlantCommand request, CancellationToken cancellationToken)
        {
            var plant = await _uow.PowerPlants.GetByIdAsync(request.Id);
            if (plant == null) throw new Exception("Електростанцію не знайдено.");

            _uow.PowerPlants.Delete(plant);
            await _uow.SaveChangesAsync();
        }
    }
}