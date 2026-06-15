using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.GridDatetimes
{
    public record CreateGridDatetimeCommand(
        float AvailablePower,
        float RequiredPower,
        DateOnly ForecastDate,
        TimeOnly ForecastTimestamp
    ) : IRequest<int>;

    public class CreateGridDatetimeCommandHandler : IRequestHandler<CreateGridDatetimeCommand, int>
    {
        private readonly IGridUnitOfWork _uow;

        public CreateGridDatetimeCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<int> Handle(CreateGridDatetimeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _uow.Datetimes.FindAsync(d =>
                d.ForecastDate == request.ForecastDate &&
                d.ForecastTimestamp == request.ForecastTimestamp);

            if (existing.Any())
                throw new Exception("Дані на цю дату та час вже існують.");

            var datetime = new GridDatetime
            {
                AvailablePower = request.AvailablePower,
                RequiredPower = request.RequiredPower,
                ForecastDate = request.ForecastDate,
                ForecastTimestamp = request.ForecastTimestamp
            };

            await _uow.Datetimes.AddAsync(datetime);
            await _uow.SaveChangesAsync();

            return datetime.Id;
        }
    }
}