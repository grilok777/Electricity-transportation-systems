using Domain.Interfaces;
using MediatR;

namespace Application.Features.Substations
{
    public record GetSubstationByIdQuery(int Id) : IRequest<SubstationDto>;

    public class GetSubstationByIdQueryHandler : IRequestHandler<GetSubstationByIdQuery, SubstationDto>
    {
        private readonly IGridUnitOfWork _uow;
        public GetSubstationByIdQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<SubstationDto> Handle(GetSubstationByIdQuery request, CancellationToken cancellationToken)
        {
            var s = await _uow.Substations.GetByIdAsync(request.Id);
            if (s == null) throw new Exception("Підстанцію не знайдено.");

            return new SubstationDto { Id = s.Id, Name = s.Name, Location = s.Location, MaxThroughputMw = s.MaxThroughputMw, Status = s.Status.ToString() };
        }
    }
}