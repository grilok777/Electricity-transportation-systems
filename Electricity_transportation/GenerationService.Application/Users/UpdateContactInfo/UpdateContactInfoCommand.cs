using Domain.Interfaces;
using MediatR;


namespace Application.Users
{
    public record UpdateContactInfoCommand(int OwnerId, int DealId, ContactInfoDto Dto) : IRequest;

    public class UpdateContactInfoCommandHandler : IRequestHandler<UpdateContactInfoCommand>
    {
        private readonly IGenerationUnitOfWork _uow;

        public UpdateContactInfoCommandHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(UpdateContactInfoCommand request, CancellationToken cancellationToken)
        {
            var owners = await _uow.Users.FindAsync(o => o.Id == request.OwnerId, o => o.Deals);
            var owner = owners.FirstOrDefault();

            if (owner == null) throw new Exception("Користувача не знайдено.");

            var targetDeal = owner.Deals.FirstOrDefault(d => d.Id == request.DealId);

            if (targetDeal == null)
                throw new Exception($"Угоду з Id {request.DealId} для цього користувача не знайдено.");

            targetDeal.NumberPhone = request.Dto.NumberPhone;
            targetDeal.PlaceLocation = request.Dto.PlaceLocation;

            _uow.Users.Update(owner);
            await _uow.SaveChangesAsync();
        }
    }
}