using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AvailablePowers
{
    public record CreateAvailablePowerCommand(
        int SubstationLineId,
        int DatetimeId,
        List<int> AvailablePowerPlants
    ) : IRequest<int>;

    public class CreateAvailablePowerCommandHandler : IRequestHandler<CreateAvailablePowerCommand, int>
    {
        private readonly IGridUnitOfWork _uow;

        public CreateAvailablePowerCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<int> Handle(CreateAvailablePowerCommand request, CancellationToken cancellationToken)
        {
            var line = await _uow.SubstationLines.GetByIdAsync(request.SubstationLineId);
            if (line == null) throw new Exception($"Лінію з ID {request.SubstationLineId} не знайдено.");

            var datetime = await _uow.Datetimes.GetByIdAsync(request.DatetimeId);
            if (datetime == null) throw new Exception($"Запис часу з ID {request.DatetimeId} не знайдено.");

            var existing = await _uow.AvailablePowers.FindAsync(a =>
                a.SubstationLineId == request.SubstationLineId &&
                a.DatetimeId == request.DatetimeId);

            if (existing.Any()) throw new Exception("Дані про доступну потужність для цієї лінії на цей час вже існують.");

            var availablePower = new AvailablePower
            {
                SubstationLineId = request.SubstationLineId,
                DatetimeId = request.DatetimeId,
                AvailablePowerPlants = request.AvailablePowerPlants
            };

            await _uow.AvailablePowers.AddAsync(availablePower);
            await _uow.SaveChangesAsync();

            return availablePower.Id;
        }
    }
}