using Domain.Enum;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Forecasts
{
    public record UpdateForecastStatusCommand(int ForecastId, DayStatus NewStatus) : IRequest;

    public class UpdateForecastStatusCommandHandler : IRequestHandler<UpdateForecastStatusCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public UpdateForecastStatusCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(UpdateForecastStatusCommand request, CancellationToken cancellationToken)
        {
            var forecast = await _uow.PowerPlantDays.GetByIdAsync(request.ForecastId);
            if (forecast == null) throw new Exception("Прогноз не знайдено.");

            forecast.Status = request.NewStatus;

            forecast.ProcessedAt = DateTime.UtcNow;

            _uow.PowerPlantDays.Update(forecast);
            await _uow.SaveChangesAsync();
        }
    }
}