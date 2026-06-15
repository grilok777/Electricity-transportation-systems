using Domain.Interfaces;
using MediatR;

namespace Application.Features.AvailablePowers
{
    public record GetAvailablePowerByIdQuery(int Id) : IRequest<AvailablePowerDto>;

    public class GetAvailablePowerByIdQueryHandler : IRequestHandler<GetAvailablePowerByIdQuery, AvailablePowerDto>
    {
        private readonly IGridUnitOfWork _uow;
        public GetAvailablePowerByIdQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<AvailablePowerDto> Handle(GetAvailablePowerByIdQuery request, CancellationToken cancellationToken)
        {
            var a = await _uow.AvailablePowers.GetByIdAsync(request.Id);
            if (a == null) throw new Exception("Запис не знайдено.");

            return new AvailablePowerDto { Id = a.Id, SubstationLineId = a.SubstationLineId, DatetimeId = a.DatetimeId, AvailablePowerPlants = a.AvailablePowerPlants };
        }
    }
}