
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Operators
{
    public record AuthenticateOperatorQuery(string Username, string Password) : IRequest<LoginResponseDto>;

    public class AuthenticateOperatorQueryHandler : IRequestHandler<AuthenticateOperatorQuery, LoginResponseDto>
    {
        private readonly IGridUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthenticateOperatorQueryHandler(IGridUnitOfWork uow, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<LoginResponseDto> Handle(AuthenticateOperatorQuery request, CancellationToken cancellationToken)
        {

            var op = await _uow.Operators.GetByNameAsync(request.Username);

            if (op == null || !_passwordHasher.VerifyPassword(request.Password, op.Password))
            {
                throw new UnauthorizedAccessException("Невірне ім'я користувача або пароль.");
            }

            var token = _jwtProvider.GenerateToken(op);//, op.AccessLevel

            var profile = new OperatorDto { Id = op.Id, Username = op.Username, AccessLevel = op.AccessLevel };

            return new LoginResponseDto(token, profile);
        }
    }
}