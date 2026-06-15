using Domain.Interfaces;
using MediatR;

namespace Application.Features.Operators
{
    public record DeleteOperatorCommand(int Id) : IRequest;

    public class DeleteOperatorCommandHandler : IRequestHandler<DeleteOperatorCommand>
    {
        private readonly IGridUnitOfWork _uow;
        public DeleteOperatorCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(DeleteOperatorCommand request, CancellationToken cancellationToken)
        {
            var op = await _uow.Operators.GetByIdAsync(request.Id);
            if (op == null) throw new Exception("Оператора не знайдено.");

            _uow.Operators.Delete(op);
            await _uow.SaveChangesAsync();
        }
    }
}