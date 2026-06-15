using Domain.Interfaces;
using MediatR;

namespace Application.Users
{
    public record ResetPasswordCommand(string Username, string ResetCode, string NewPassword) : IRequest;

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IGenerationUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        private const string HardcodedResetCode = "Астанавітесь!";

        public ResetPasswordCommandHandler(IGenerationUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.ResetCode != HardcodedResetCode)
            {
                throw new UnauthorizedAccessException("Неправильний код відновлення пароля.");
            }

            var user = await _uow.Users.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new Exception("Користувача з таким ім'ям не знайдено.");
            }

            // 3. Валідація нового пароля (така ж як при реєстрації)
            if (!request.NewPassword.Any(char.IsDigit) || !request.NewPassword.Any(char.IsUpper))
            {
                throw new ArgumentException("Пароль повинен містити хоча б одну цифру та одну велику літеру.");
            }

            // 4. Хешування та оновлення пароля
            user.Password = _passwordHasher.HashPassword(request.NewPassword);

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync();
        }
    }
}