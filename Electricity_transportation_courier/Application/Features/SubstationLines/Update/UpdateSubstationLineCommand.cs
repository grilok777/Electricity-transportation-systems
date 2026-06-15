using Domain.Interfaces;
using MediatR;

namespace Application.Features.SubstationLines
{
    public record UpdateSubstationLineCommand(int Id, string LineName, float BaseLoadKw) : IRequest;

    public class UpdateSubstationLineCommandHandler : IRequestHandler<UpdateSubstationLineCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public UpdateSubstationLineCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task Handle(UpdateSubstationLineCommand request, CancellationToken cancellationToken)
        {
            var line = await _uow.SubstationLines.GetByIdAsync(request.Id);
            if (line == null) throw new Exception("Лінію не знайдено.");

            line.LineName = request.LineName;
            line.BaseLoadKw = request.BaseLoadKw;

            _uow.SubstationLines.Update(line);
            await _uow.SaveChangesAsync();
        }
    }
}