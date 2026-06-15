using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOperatorRepository : IBaseRepository<Operator>
    {
        Task<Operator?> GetByNameAsync(string name);
    }
}