using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Users
{
    public record RegisterUserCommand(string Username, string Password) : IRequest;

    public class RegisterOwnerCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IGenerationUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterOwnerCommandHandler(IGenerationUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.Password.Any(char.IsDigit) || !request.Password.Any(char.IsUpper))
                throw new ArgumentException("Пароль повинен містити хоча б одну цифру та одну велику літеру.");

            var existingUser = await _uow.Users.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new Exception("Користувач з таким ім'ям вже існує.");

            string hash = _passwordHasher.HashPassword(request.Password);

            var newOwner = new User
            {
                Username = request.Username,
                Password = hash,
                DateRegistration = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _uow.Users.AddAsync(newOwner);
            await _uow.SaveChangesAsync();
        }
    }
}