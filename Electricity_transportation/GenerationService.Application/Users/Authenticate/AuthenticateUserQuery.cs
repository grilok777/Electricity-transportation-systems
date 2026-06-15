using Domain.Interfaces;
using MediatR;


namespace Application.Users
{
    public record AuthenticateUserQuery(string Username, string Password) : IRequest<(string Token, int OwnerId)>;

    public class AuthenticateOwnerQueryHandler : IRequestHandler<AuthenticateUserQuery, (string Token, int OwnerId)>
    {
        private readonly IGenerationUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthenticateOwnerQueryHandler(IGenerationUnitOfWork uow, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<(string Token, int OwnerId)> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var owner = await _uow.Users.GetByUsernameAsync(request.Username);
            if (owner == null)
                throw new UnauthorizedAccessException("Неправильний логін або пароль.");

            bool isValid = _passwordHasher.VerifyPassword(request.Password, owner.Password);

            if (!isValid)
                throw new UnauthorizedAccessException("Неправильний логін або пароль.");

            string token = _jwtProvider.GenerateToken(owner);

            return (token, owner.Id);
        }
    }
}