using Domain.Interfaces;
using MediatR;

namespace Application.Features.OwnerDeals
{
    public record DeleteOwnerDealCommand(int Id) : IRequest;

    public class DeleteOwnerDealCommandHandler : IRequestHandler<DeleteOwnerDealCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public DeleteOwnerDealCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(DeleteOwnerDealCommand request, CancellationToken cancellationToken)
        {
            var deal = await _uow.OwnerDeals.GetByIdAsync(request.Id);
            if (deal == null) throw new Exception("Угоду не знайдено.");

            _uow.OwnerDeals.Delete(deal);
            await _uow.SaveChangesAsync();
        }
    }
}