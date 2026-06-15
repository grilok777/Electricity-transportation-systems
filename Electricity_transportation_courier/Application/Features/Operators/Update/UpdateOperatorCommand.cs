using Domain.Interfaces;
using MediatR;

namespace Application.Features.Operators
{
    public record UpdateOperatorCommand(int Id, string Username, string AccessLevel) : IRequest;

    public class UpdateOperatorCommandHandler : IRequestHandler<UpdateOperatorCommand>
    {
        private readonly IGridUnitOfWork _uow;
        public UpdateOperatorCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(UpdateOperatorCommand request, CancellationToken cancellationToken)
        {
            var op = await _uow.Operators.GetByIdAsync(request.Id);
            if (op == null) throw new Exception("Оператора не знайдено.");

            op.Username = request.Username;
            op.AccessLevel = request.AccessLevel;

            _uow.Operators.Update(op);
            await _uow.SaveChangesAsync();
        }
    }
}