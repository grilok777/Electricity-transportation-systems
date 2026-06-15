
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Operators
{
    public record GetAllOperatorsQuery() : IRequest<List<OperatorDto>>;

    public class GetAllOperatorsQueryHandler : IRequestHandler<GetAllOperatorsQuery, List<OperatorDto>>
    {
        private readonly IGridUnitOfWork _uow;
        public GetAllOperatorsQueryHandler(IGridUnitOfWork uow) => _uow = uow;

        public async Task<List<OperatorDto>> Handle(GetAllOperatorsQuery request, CancellationToken cancellationToken)
        {
            var operators = await _uow.Operators.GetAllAsync();
            return operators.Select(o => new OperatorDto { Id = o.Id, Username = o.Username, AccessLevel = o.AccessLevel }).ToList();
        }
    }
}