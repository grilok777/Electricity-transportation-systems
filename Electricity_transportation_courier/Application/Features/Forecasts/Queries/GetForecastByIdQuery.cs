using Domain.Interfaces;
using MediatR;

namespace Application.Features.Forecasts
{
    public record GetForecastByIdQuery(int Id) : IRequest<ForecastDayDto>;

    public class GetForecastByIdQueryHandler : IRequestHandler<GetForecastByIdQuery, ForecastDayDto>
    {
        private readonly IGridUnitOfWork _uow;

        public GetForecastByIdQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<ForecastDayDto> Handle(GetForecastByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _uow.PowerPlantDays.FindAsync(
                d => d.Id == request.Id,
                d => d.HourDatas 
            );

            var day = result.FirstOrDefault();
            if (day == null) throw new Exception("Прогноз не знайдено.");

            return new ForecastDayDto
            {
                Id = day.Id,
                PlantId = day.PlantId,
                ForecastDate = day.ForecastDate,
                Status = day.Status.ToString(),
                RetryCount = day.RetryCount,
                CreatedAt = day.CreatedAt,
                ProcessedAt = day.ProcessedAt,
                TotalDailyKw = day.HourDatas.Sum(h => h.ForecastedKw),
                HourlyForecasts = day.HourDatas.Select(h => new HourDataDto
                {
                    Hour = h.ForecastTimestamp.Hour,
                    ForecastedKw = h.ForecastedKw
                }).OrderBy(h => h.Hour).ToList()
            };
        }
    }
}