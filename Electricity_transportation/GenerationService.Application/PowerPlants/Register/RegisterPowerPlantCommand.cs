using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.PowerPlants
{
    public record RegisterPowerPlantCommand(int UserId, PowerPlantDto Dto) : IRequest;

    public class RegisterPowerPlantCommandHandler : IRequestHandler<RegisterPowerPlantCommand>
    {
        private readonly IGenerationUnitOfWork _uow;

        public RegisterPowerPlantCommandHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(RegisterPowerPlantCommand request, CancellationToken cancellationToken)
        {
            Enum.TryParse(request.Dto.Type, true, out PlantType type);

            var plant = new PowerPlant
            {
                Location = request.Dto.Location,
                MaxCapacityKw = request.Dto.MaxCapacityKw,
                Type = type,
                Status = PlantStatus.Active,
                DateCommissioning = DateTime.UtcNow
            };

            await _uow.PowerPlants.AddAsync(plant);
            await _uow.SaveChangesAsync();
        }
    }
}