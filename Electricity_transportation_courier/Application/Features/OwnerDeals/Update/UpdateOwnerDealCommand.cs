using Domain.Interfaces;
using MediatR;

namespace Application.Features.OwnerDeals
{
    public record UpdateOwnerDealCommand(
        int Id,
        string OwnerName,
        string NumberPhone,
        string PlaceLocation,
        DateOnly ConclusionDeal,
        DateOnly? CompetionDeal
    ) : IRequest;

    public class UpdateOwnerDealCommandHandler : IRequestHandler<UpdateOwnerDealCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public UpdateOwnerDealCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(UpdateOwnerDealCommand request, CancellationToken cancellationToken)
        {
            var deal = await _uow.OwnerDeals.GetByIdAsync(request.Id);
            if (deal == null) throw new Exception("Угоду не знайдено.");

            deal.OwnerName = request.OwnerName;
            deal.NumberPhone = request.NumberPhone;
            deal.PlaceLocation = request.PlaceLocation;
            deal.ConclusionDeal = request.ConclusionDeal;
            deal.CompetionDeal = request.CompetionDeal;

            _uow.OwnerDeals.Update(deal);
            await _uow.SaveChangesAsync();
        }
    }
}