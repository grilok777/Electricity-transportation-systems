using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OperatorRepository : BaseRepository<Operator>, IOperatorRepository
    {
        public OperatorRepository(GridDbContext context) : base(context) { }

        public async Task<Operator?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(o => o.Username == name);
        }
    }
}