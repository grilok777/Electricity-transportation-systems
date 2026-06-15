using Domain.Enum;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Substations
{
    public record UpdateSubstationCommand(int Id, string Name, string Location, float MaxThroughputMw, Status Status) : IRequest;

    public class UpdateSubstationCommandHandler : IRequestHandler<UpdateSubstationCommand>
    {
        private readonly IGridUnitOfWork _uow;

        public UpdateSubstationCommandHandler(IGridUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(UpdateSubstationCommand request, CancellationToken cancellationToken)
        {
            var substation = await _uow.Substations.GetByIdAsync(request.Id);
            if (substation == null) throw new Exception($"Підстанцію з ID {request.Id} не знайдено.");

            substation.Name = request.Name;
            substation.Location = request.Location;
            substation.MaxThroughputMw = request.MaxThroughputMw;
            substation.Status = request.Status;

            _uow.Substations.Update(substation);
            await _uow.SaveChangesAsync();
        }
    }
}