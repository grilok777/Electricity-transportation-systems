using Domain.Interfaces;
using MediatR;

namespace Application.Features.AvailablePowers
{
    public record UpdateAvailablePowerCommand(int Id, List<int> AvailablePowerPlants) : IRequest;

    public class UpdateAvailablePowerCommandHandler : IRequestHandler<UpdateAvailablePowerCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public UpdateAvailablePowerCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(UpdateAvailablePowerCommand request, CancellationToken cancellationToken)
        {
            var record = await _uow.AvailablePowers.GetByIdAsync(request.Id);
            if (record == null) throw new Exception("Запис не знайдено.");

            record.AvailablePowerPlants = request.AvailablePowerPlants;

            _uow.AvailablePowers.Update(record);
            await _uow.SaveChangesAsync();
        }
    }
}