using Domain.Interfaces;
using MediatR;

namespace Application.Features.SubstationLines
{ 
    public record GetSubstationLineByIdQuery(int Id) : IRequest<SubstationLineDto>;

    public class GetSubstationLineByIdQueryHandler : IRequestHandler<GetSubstationLineByIdQuery, SubstationLineDto>
    {
        private readonly IGridUnitOfWork _uow;

        public GetSubstationLineByIdQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<SubstationLineDto> Handle(GetSubstationLineByIdQuery request, CancellationToken cancellationToken)
        {
            var line = await _uow.SubstationLines.GetByIdAsync(request.Id);
            if (line == null) throw new Exception("Лінію не знайдено.");

            return new SubstationLineDto
            {
                Id = line.Id,
                SubstationId = line.SubstationId,
                LineName = line.LineName,
                BaseLoadKw = line.BaseLoadKw
            };
        }
    }
}