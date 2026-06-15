

using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGenerationUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IBaseRepository<PowerPlant> PowerPlants { get; }
        IBaseRepository<PowerPlantDay> PowerPlantDays { get; }
        IBaseRepository<OwnerDeal> OwnerDeals { get; }
        Task<int> SaveChangesAsync();
    }
}
