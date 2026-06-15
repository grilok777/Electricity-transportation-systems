
using Domain.Entities;
using Domain.Interfaces;

using Infrastructure.Repositories;

namespace Infrastructure.Data
{
    public class GenerationUnitOfWork : IGenerationUnitOfWork
    {
        private readonly GenerationDbContext _context;

        private IUserRepository? _users;
        private IBaseRepository<PowerPlant>? _powerPlants;
        private IBaseRepository<PowerPlantDay>? _powerPlantDays;
        private IBaseRepository<OwnerDeal>? _ownerDeals;

        public GenerationUnitOfWork(GenerationDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users =>
            _users ??= new UserRepository(_context);

        public IBaseRepository<PowerPlant> PowerPlants =>
            _powerPlants ??= new BaseRepository<PowerPlant>(_context);

        public IBaseRepository<PowerPlantDay> PowerPlantDays =>
            _powerPlantDays ??= new BaseRepository<PowerPlantDay>(_context);

        public IBaseRepository<OwnerDeal> OwnerDeals =>
            _ownerDeals ??= new BaseRepository<OwnerDeal>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
