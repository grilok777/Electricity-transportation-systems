using MediatR;
using Domain.Interfaces;

namespace Application.Users
{
    public record GetUserProfileQuery(int OwnerId) : IRequest<UserProfileDto>;
    public class GetOwnerProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IGenerationUnitOfWork _uow;

        public GetOwnerProfileQueryHandler(IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var owner = await _uow.Users.GetByIdAsync(request.OwnerId);
            if (owner == null) throw new Exception("Користувача не знайдено.");

            return new UserProfileDto
            {
                Id = owner.Id,
                Username = owner.Username,
                DateRegistration = owner.DateRegistration
            };
        }
    }
}