using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGridUnitOfWork : IDisposable
    {
        IOperatorRepository Operators { get; }
        IBaseRepository<Substation> Substations { get; }
        IBaseRepository<SubstationLine> SubstationLines { get; }
        IBaseRepository<GridDatetime> Datetimes { get; }

        /*IBaseRepository<NeedForHourLine> NeedsForHourLines { get; }*/
        IBaseRepository<AvailablePower> AvailablePowers { get; }

        IBaseRepository<PowerPlant> PowerPlants { get; }
        IBaseRepository<PowerPlantDay> PowerPlantDays { get; }
        IBaseRepository<HourData> HourDatas { get; }
        IBaseRepository<OwnerDeal> OwnerDeals { get; }

        Task<int> SaveChangesAsync();
    }
}