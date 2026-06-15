using Domain.Interfaces;
using MediatR;

namespace Application.PowerPlants
{
    public record GetUserPlantsQuery(int OwnerId) : IRequest<List<PowerPlantDto>>;

    public class GetOwnerPlantsQueryHandler : IRequestHandler<GetUserPlantsQuery, List<PowerPlantDto>>
    {
        private readonly IGenerationUnitOfWork _uow;
        public GetOwnerPlantsQueryHandler(IGenerationUnitOfWork uow) => _uow = uow;

        public async Task<List<PowerPlantDto>> Handle(GetUserPlantsQuery request, CancellationToken cancellationToken)
        {
            var plants = await _uow.PowerPlants.FindAsync(
                p => p.Deal.UserId == request.OwnerId,
                p => p.Deal 
            );

            return plants.Select(p => new PowerPlantDto
            {
                Id = p.Id,
                DealId = p.DealId,
                Type = p.Type.ToString(),
                MaxCapacityKw = p.MaxCapacityKw,
                Location = p.Location,
                Status = p.Status.ToString()
            }).ToList();
        }
    }
}