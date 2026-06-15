
using Domain.Interfaces;
using MediatR;

namespace Application.Features.GridDatetimes
{
    public record GetAllGridDatetimesQuery() : IRequest<List<GridDatetimeDto>>;

    public class GetAllGridDatetimesQueryHandler : IRequestHandler<GetAllGridDatetimesQuery, List<GridDatetimeDto>>
    {
        private readonly IGridUnitOfWork _uow;
        public GetAllGridDatetimesQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<GridDatetimeDto>> Handle(GetAllGridDatetimesQuery request, CancellationToken cancellationToken)
        {
            var data = await _uow.Datetimes.GetAllAsync();
            return data.Select(d => new GridDatetimeDto { Id = d.Id, AvailablePower = d.AvailablePower, RequiredPower = d.RequiredPower, ForecastDate = d.ForecastDate, ForecastTimestamp = d.ForecastTimestamp }).ToList();
        }
    }
}