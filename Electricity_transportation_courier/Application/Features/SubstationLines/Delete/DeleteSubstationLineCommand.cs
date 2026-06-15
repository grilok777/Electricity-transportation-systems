using Domain.Interfaces;
using MediatR;

namespace Application.Features.SubstationLines
{
    public record DeleteSubstationLineCommand(int Id) : IRequest;

    public class DeleteSubstationLineCommandHandler : IRequestHandler<DeleteSubstationLineCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public DeleteSubstationLineCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(DeleteSubstationLineCommand request, CancellationToken cancellationToken)
        {
            var line = await _uow.SubstationLines.GetByIdAsync(request.Id);
            if (line == null) throw new Exception("Лінію не знайдено.");

            _uow.SubstationLines.Delete(line);
            await _uow.SaveChangesAsync();
        }
    }
}