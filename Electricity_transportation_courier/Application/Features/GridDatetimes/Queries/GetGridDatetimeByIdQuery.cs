using Domain.Interfaces;
using MediatR;

namespace Application.Features.GridDatetimes
{
    public record GetGridDatetimeByIdQuery(int Id) : IRequest<GridDatetimeDto>;

    public class GetGridDatetimeByIdQueryHandler : IRequestHandler<GetGridDatetimeByIdQuery, GridDatetimeDto>
    {
        private readonly IGridUnitOfWork _uow;
        public GetGridDatetimeByIdQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<GridDatetimeDto> Handle(GetGridDatetimeByIdQuery request, CancellationToken cancellationToken)
        {
            var d = await _uow.Datetimes.GetByIdAsync(request.Id);
            if (d == null) throw new Exception("Запис не знайдено.");

            return new GridDatetimeDto { Id = d.Id, AvailablePower = d.AvailablePower, RequiredPower = d.RequiredPower, ForecastDate = d.ForecastDate, ForecastTimestamp = d.ForecastTimestamp };
        }
    }
}