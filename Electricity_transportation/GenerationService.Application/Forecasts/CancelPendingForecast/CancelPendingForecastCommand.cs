using Domain.Entities;
using Domain.Interfaces;
using MediatR;


namespace Application.Forecasts
{
    public record CancelPendingForecastCommand(int ForecastId, int UserId) : IRequest;

    public class CancelPendingForecastCommandHandler : IRequestHandler<CancelPendingForecastCommand>
    {
        private readonly IGenerationUnitOfWork _uow;

        public CancelPendingForecastCommandHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(CancelPendingForecastCommand request, CancellationToken cancellationToken)
        {
            var forecast = await _uow.PowerPlantDays.GetByIdAsync(request.ForecastId);
            if (forecast == null) throw new Exception("Прогноз не знайдено.");

            /*var plant = await _uow.PowerPlants.GetByIdAsync(forecast.PlantId);
            if (plant == null || plant.UserId != request.UserId)
                throw new UnauthorizedAccessException("Ви не маєте доступу до цієї станції.");*/

            if (forecast.Status != DayStatus.Pending)
                throw new InvalidOperationException("Тільки прогнози в статусі 'Pending' можна скасувати.");

            forecast.Status = DayStatus.Canceled;

            _uow.PowerPlantDays.Update(forecast);
            await _uow.SaveChangesAsync();
        }
    }
}