using Domain.Entities;
using Domain.Interfaces;
using MediatR;


namespace Application.Deals
{
    public record SignDealCommand(int OwnerId, CreateDealDto Dto) : IRequest;

    public class SignDealCommandHandler : IRequestHandler<SignDealCommand>
    {
        private readonly IGenerationUnitOfWork _uow;

        public SignDealCommandHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(SignDealCommand request, CancellationToken cancellationToken)
        {
            var deal = new OwnerDeal
            {
                UserId = request.OwnerId,
                OwnerName = request.Dto.OwnerName,
                NumberPhone = request.Dto.NumberPhone,
                PlaceLocation = request.Dto.PlaceLocation,
                ConclusionDeal = DateOnly.FromDateTime(request.Dto.ConclusionDeal),
                CompetionDeal = DateOnly.FromDateTime(request.Dto.CompletionDeal)
            };

            await _uow.OwnerDeals.AddAsync(deal);
            await _uow.SaveChangesAsync();
        }
    }
}