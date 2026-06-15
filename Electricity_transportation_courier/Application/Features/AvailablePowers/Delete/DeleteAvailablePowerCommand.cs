using Domain.Interfaces;
using MediatR;

namespace Application.Features.AvailablePowers
{
    public record DeleteAvailablePowerCommand(int Id) : IRequest;

    public class DeleteAvailablePowerCommandHandler : IRequestHandler<DeleteAvailablePowerCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public DeleteAvailablePowerCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(DeleteAvailablePowerCommand request, CancellationToken cancellationToken)
        {
            var record = await _uow.AvailablePowers.GetByIdAsync(request.Id);
            if (record == null) throw new Exception("Запис не знайдено.");

            _uow.AvailablePowers.Delete(record);
            await _uow.SaveChangesAsync();
        }
    }
}