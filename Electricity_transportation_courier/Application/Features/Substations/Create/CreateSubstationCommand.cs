using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Substations
{
    public record CreateSubstationCommand(string Name, string Location, float MaxThroughputMw) : IRequest<int>;

    public class CreateSubstationCommandHandler : IRequestHandler<CreateSubstationCommand, int>
    {
        private readonly IGridUnitOfWork _uow;

        public CreateSubstationCommandHandler(IGridUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<int> Handle(CreateSubstationCommand request, CancellationToken cancellationToken)
        {
            var substation = new Substation
            {
                Name = request.Name,
                Location = request.Location,
                MaxThroughputMw = request.MaxThroughputMw,
                Status = Status.Offline 
            };

            await _uow.Substations.AddAsync(substation);
            await _uow.SaveChangesAsync();

            return substation.Id;
        }
    }
}