using Domain.Interfaces;
using MediatR;

namespace Application.Users
{
    public record ChangePasswordCommand(int OwnerId, string OldPassword, string NewPassword) : IRequest;

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IGenerationUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(IGenerationUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.OldPassword == request.NewPassword)
                throw new ArgumentException("Новий пароль не може співпадати зі старим.");

            var owner = await _uow.Users.GetByIdAsync(request.OwnerId);
            if (owner == null) throw new Exception("Користувача не знайдено.");

            bool isOldValid = _passwordHasher.VerifyPassword(request.OldPassword, owner.Password);
            if (!isOldValid) throw new UnauthorizedAccessException("Старий пароль неправильний.");

            owner.Password = _passwordHasher.HashPassword(request.NewPassword);

            _uow.Users.Update(owner);
            await _uow.SaveChangesAsync();
        }
    }
}