using Domain.Entities;
using Domain.Interfaces;
using MediatR;


namespace Application.PowerPlants
{
    public record UpdatePlantStatusCommand(int PlantId, PlantStatus NewStatus) : IRequest;

    public class UpdatePlantStatusCommandHandler : IRequestHandler<UpdatePlantStatusCommand>
    {
        private readonly IGenerationUnitOfWork _uow;

        public UpdatePlantStatusCommandHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(UpdatePlantStatusCommand request, CancellationToken cancellationToken)
        {
            var plant = await _uow.PowerPlants.GetByIdAsync(request.PlantId);
            if (plant == null) throw new Exception("Станцію не знайдено.");

            plant.Status = request.NewStatus;
            _uow.PowerPlants.Update(plant);
            await _uow.SaveChangesAsync();
        }
    }
}