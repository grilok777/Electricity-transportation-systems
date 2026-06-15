using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.OwnerDeals
{
    public record CreateOwnerDealCommand(
        string OwnerName,
        string NumberPhone,
        string PlaceLocation,
        DateOnly ConclusionDeal,
        DateOnly? CompetionDeal
    ) : IRequest<int>;

    public class CreateOwnerDealCommandHandler : IRequestHandler<CreateOwnerDealCommand, int>
    {
        private readonly IGridUnitOfWork _uow;

        public CreateOwnerDealCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<int> Handle(CreateOwnerDealCommand request, CancellationToken cancellationToken)
        {
            var deal = new OwnerDeal
            {
                OwnerName = request.OwnerName,
                NumberPhone = request.NumberPhone,
                PlaceLocation = request.PlaceLocation,
                ConclusionDeal = request.ConclusionDeal,
                CompetionDeal = request.CompetionDeal
            };

            await _uow.OwnerDeals.AddAsync(deal);
            await _uow.SaveChangesAsync();

            return deal.Id;
        }
    }
}