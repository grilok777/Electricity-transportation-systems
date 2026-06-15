
using Domain.Interfaces;
using MediatR;

namespace Application.Forecasts
{
    public record GetForecastHistoryQuery(int PlantId, DateOnly StartDate, DateOnly EndDate) : IRequest<List<ForecastSummaryDto>>;

    public class GetForecastHistoryQueryHandler : IRequestHandler<GetForecastHistoryQuery, List<ForecastSummaryDto>>
    {
        private readonly IGenerationUnitOfWork _uow;

        public GetForecastHistoryQueryHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<ForecastSummaryDto>> Handle(GetForecastHistoryQuery request, CancellationToken cancellationToken)
        {
            var history = await _uow.PowerPlantDays.FindAsync(
                d => d.PlantId == request.PlantId &&
                d.ForecastDate >= request.StartDate &&
                d.ForecastDate <= request.EndDate,
                d => d.HourDatas
            );

            return history.Select(d => new ForecastSummaryDto
            {
                Id = d.Id,
                ForecastDate = d.ForecastDate,
                Status = d.Status.ToString(),
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