using Domain.Interfaces;
using MediatR;

namespace Application.Features.GridDatetimes
{
    public record DeleteGridDatetimeCommand(int Id) : IRequest;

    public class DeleteGridDatetimeCommandHandler : IRequestHandler<DeleteGridDatetimeCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public DeleteGridDatetimeCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(DeleteGridDatetimeCommand request, CancellationToken cancellationToken)
        {
            var datetime = await _uow.Datetimes.GetByIdAsync(request.Id);
            if (datetime == null) throw new Exception("Запис не знайдено.");

            _uow.Datetimes.Delete(datetime);
            await _uow.SaveChangesAsync();
        }
    }
}