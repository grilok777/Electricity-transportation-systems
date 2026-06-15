using Domain.Interfaces;
using MediatR;

namespace Application.Features.GridDatetimes
{
    public record UpdateGridDatetimeCommand(
        int Id,
        float AvailablePower,
        float RequiredPower,
        DateOnly ForecastDate,
        TimeOnly ForecastTimestamp
    ) : IRequest;

    public class UpdateGridDatetimeCommandHandler : IRequestHandler<UpdateGridDatetimeCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public UpdateGridDatetimeCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(UpdateGridDatetimeCommand request, CancellationToken cancellationToken)
        {
            var datetime = await _uow.Datetimes.GetByIdAsync(request.Id);
            if (datetime == null) throw new Exception("Запис не знайдено.");

            datetime.AvailablePower = request.AvailablePower;
            datetime.RequiredPower = request.RequiredPower;
            datetime.ForecastDate = request.ForecastDate;
            datetime.ForecastTimestamp = request.ForecastTimestamp;

            _uow.Datetimes.Update(datetime);
            await _uow.SaveChangesAsync();
        }
    }
}