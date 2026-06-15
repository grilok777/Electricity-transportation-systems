using Domain.Interfaces;
using MediatR;

namespace Application.Features.OwnerDeals
{
    public record GetOwnerDealByIdQuery(int Id) : IRequest<OwnerDealDto>;

    public class GetOwnerDealByIdQueryHandler : IRequestHandler<GetOwnerDealByIdQuery, OwnerDealDto>
    {
        private readonly IGridUnitOfWork _uow;

        public GetOwnerDealByIdQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<OwnerDealDto> Handle(GetOwnerDealByIdQuery request, CancellationToken cancellationToken)
        {
            var d = await _uow.OwnerDeals.GetByIdAsync(request.Id);
            if (d == null) throw new Exception("Угоду не знайдено.");

            return new OwnerDealDto
            {
                Id = d.Id,
                OwnerName = d.OwnerName,
                NumberPhone = d.NumberPhone,
                PlaceLocation = d.PlaceLocation,
                ConclusionDeal = d.ConclusionDeal,
                CompetionDeal = d.CompetionDeal
            };
        }
    }
}