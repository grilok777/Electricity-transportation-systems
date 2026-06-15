using Domain.Interfaces;
using MediatR;

namespace Application.Features.Substations
{
    public record DeleteSubstationCommand(int Id) : IRequest;

    public class DeleteSubstationCommandHandler : IRequestHandler<DeleteSubstationCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public DeleteSubstationCommandHandler(IGridUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeleteSubstationCommand request, CancellationToken cancellationToken)
        {
            var substation = await _uow.Substations.GetByIdAsync(request.Id);
            if (substation == null) throw new Exception("Підстанцію не знайдено.");

            _uow.Substations.Delete(substation);
            await _uow.SaveChangesAsync();
        }
    }
}