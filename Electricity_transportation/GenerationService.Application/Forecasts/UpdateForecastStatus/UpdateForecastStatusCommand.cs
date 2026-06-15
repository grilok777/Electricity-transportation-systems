using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Forecasts
{
    public record UpdateForecastStatusCommand(int ForecastId, DayStatus Status) : IRequest;

    public class UpdateForecastStatusCommandHandler : IRequestHandler<UpdateForecastStatusCommand>
    {
        private readonly IGenerationUnitOfWork _uow;

        public UpdateForecastStatusCommandHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(UpdateForecastStatusCommand request, CancellationToken cancellationToken)
        {
            var forecast = await _uow.PowerPlantDays.GetByIdAsync(request.ForecastId);
            if (forecast == null) throw new Exception("Прогноз не знайдено.");

            forecast.Status = request.Status;
            forecast.ProcessedAt = DateTime.UtcNow;

            _uow.PowerPlantDays.Update(forecast);
            await _uow.SaveChangesAsync();
        }
    }
}