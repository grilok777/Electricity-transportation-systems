
using Domain.Enum;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Forecasts
{
    public record SearchForecastsQuery(
        int? PlantId,
        DateOnly? StartDate,
        DateOnly? EndDate,
        DayStatus? Status
    ) : IRequest<List<ForecastDayDto>>;

    public class SearchForecastsQueryHandler : IRequestHandler<SearchForecastsQuery, List<ForecastDayDto>>
    {
        private readonly IGridUnitOfWork _uow;

        public SearchForecastsQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<ForecastDayDto>> Handle(SearchForecastsQuery request, CancellationToken cancellationToken)
        {
            var results = await _uow.PowerPlantDays.FindAsync(d =>
                (!request.PlantId.HasValue || d.PlantId == request.PlantId.Value) &&
                (!request.StartDate.HasValue || d.ForecastDate >= request.StartDate.Value) &&
                (!request.EndDate.HasValue || d.ForecastDate <= request.EndDate.Value) &&
                (!request.Status.HasValue || d.Status == request.Status.Value),
                d => d.HourDatas 
            );

            var sorted = results.OrderByDescending(d => d.ForecastDate);

            return sorted.Select(d => new ForecastDayDto
            {
                Id = d.Id,
                PlantId = d.PlantId,
                ForecastDate = d.ForecastDate,
                Status = d.Status.ToString(),
                RetryCount = d.RetryCount,
                CreatedAt = d.CreatedAt,
                ProcessedAt = d.ProcessedAt,
                TotalDailyKw = d.HourDatas.Sum(h => h.ForecastedKw),
                HourlyForecasts = d.HourDatas.Select(h => new HourDataDto
                {
                    Hour = h.ForecastTimestamp.Hour,
                    ForecastedKw = h.ForecastedKw
                }).OrderBy(h => h.Hour).ToList()
            }).ToList();
        }
    }
}