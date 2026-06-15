
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Forecasts
{
    public record SubmitDailyForecastCommand(int PlantId, DateOnly Date, List<HourDataDto> Forecasts) : IRequest;

    public class SubmitDailyForecastCommandHandler : IRequestHandler<SubmitDailyForecastCommand>
    {
        private readonly IGenerationUnitOfWork _uow;

        public SubmitDailyForecastCommandHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(SubmitDailyForecastCommand request, CancellationToken cancellationToken)
        {
            var plant = await _uow.PowerPlants.GetByIdAsync(request.PlantId);
            if (plant == null) throw new Exception("Станцію не знайдено.");

            if (plant.Status != PlantStatus.Active)
                throw new InvalidOperationException($"Неможливо подати прогноз: поточний статус станції - {plant.Status}.");

            var commissioningDate = DateOnly.FromDateTime(plant.DateCommissioning);
            if (request.Date < commissioningDate)
                throw new InvalidOperationException($"Неможливо подати прогноз: {request.Date} передує даті запуску станції ({commissioningDate}).");

            if (plant.DateDecommissioning.HasValue)
            {
                var decommissioningDate = DateOnly.FromDateTime(plant.DateDecommissioning.Value);
                if (request.Date > decommissioningDate)
                    throw new InvalidOperationException($"Неможливо подати прогноз: станцію було виведено з експлуатації {decommissioningDate}.");
            }

            var existingForecasts = await _uow.PowerPlantDays.FindAsync(d => d.PlantId == request.PlantId && d.ForecastDate == request.Date);

            if (existingForecasts.Any(f => f.Status == DayStatus.Pending || f.Status == DayStatus.Approved))
            {
                throw new InvalidOperationException("На цю дату вже існує активний прогноз (очікує підтвердження або затверджений). Скасуйте його, щоб подати новий.");
            }

            int nextRetryCount = 0;
            if (existingForecasts.Any())
            {
                nextRetryCount = existingForecasts.Max(f => f.RetryCount) + 1;
            }

            if (request.Forecasts.Count != 24)
                throw new ArgumentException("Має бути рівно 24 записи для кожної години доби.");

            var newDay = new PowerPlantDay
            {
                PlantId = request.PlantId,
                ForecastDate = request.Date,
                Status = DayStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var hr in request.Forecasts)
            {
                newDay.AddHourForecast(hr.Hour, hr.ForecastedKw, plant.MaxCapacityKw);
            }

            await _uow.PowerPlantDays.AddAsync(newDay);
            await _uow.SaveChangesAsync();
        }
    }
}