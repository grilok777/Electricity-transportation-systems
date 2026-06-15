using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.SubstationLines
{
    public record CreateSubstationLineCommand(int SubstationId, string LineName, float BaseLoadKw) : IRequest<int>;

    public class CreateSubstationLineCommandHandler : IRequestHandler<CreateSubstationLineCommand, int>
    {
        private readonly IGridUnitOfWork _uow;

        public CreateSubstationLineCommandHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<int> Handle(CreateSubstationLineCommand request, CancellationToken cancellationToken)
        {
            var substation = await _uow.Substations.GetByIdAsync(request.SubstationId);
            if (substation == null)
                throw new Exception($"Підстанцію з ID {request.SubstationId} не знайдено.");

            var line = new SubstationLine
            {
                SubstationId = request.SubstationId,
                LineName = request.LineName,
                BaseLoadKw = request.BaseLoadKw
            };

            await _uow.SubstationLines.AddAsync(line);
            await _uow.SaveChangesAsync();

            return line.Id;
        }
    }
}