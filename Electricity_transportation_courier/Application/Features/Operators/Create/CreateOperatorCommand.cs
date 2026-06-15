using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Operators
{
    public record CreateOperatorCommand(string Username, string Password, string AccessLevel) : IRequest<int>;

    public class CreateOperatorCommandHandler : IRequestHandler<CreateOperatorCommand, int>
    {
        private readonly IGridUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public CreateOperatorCommandHandler(IGridUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
        {
            var existingOperator = await _uow.Operators.GetByNameAsync(request.Username);
            if (existingOperator != null)
                throw new Exception("Оператор з таким іменем вже існує.");

            var newOperator = new Operator
            {
                Username = request.Username,
                Password = _passwordHasher.HashPassword(request.Password),
                AccessLevel = request.AccessLevel
            };

            await _uow.Operators.AddAsync(newOperator);
            await _uow.SaveChangesAsync();

            return newOperator.Id;
        }
    }
}