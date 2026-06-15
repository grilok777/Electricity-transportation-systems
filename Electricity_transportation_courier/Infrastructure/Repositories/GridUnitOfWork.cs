using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Data
{
    public class GridUnitOfWork : IGridUnitOfWork
    {
        private readonly GridDbContext _context;

        private IOperatorRepository? _operators;
        private IBaseRepository<Substation>? _substations;
        private IBaseRepository<SubstationLine>? _substationLines;
        private IBaseRepository<GridDatetime>? _datetimes;
        /*private IBaseRepository<NeedForHourLine>? _needsForHourLines;*/
        private IBaseRepository<AvailablePower>? _availablePowers;
        private IBaseRepository<PowerPlant>? _powerPlants;
        private IBaseRepository<PowerPlantDay>? _powerPlantDays;
        private IBaseRepository<HourData>? _hourDatas;
        private IBaseRepository<OwnerDeal>? _ownerDeals;

        public GridUnitOfWork(GridDbContext context) => _context = context;

        public IOperatorRepository Operators => _operators ??= new OperatorRepository(_context);
        public IBaseRepository<Substation> Substations => _substations ??= new BaseRepository<Substation>(_context);
        public IBaseRepository<SubstationLine> SubstationLines => _substationLines ??= new BaseRepository<SubstationLine>(_context);
        public IBaseRepository<GridDatetime> Datetimes => _datetimes ??= new BaseRepository<GridDatetime>(_context);
        /*public IBaseRepository<NeedForHourLine> NeedsForHourLines => _needsForHourLines ??= new BaseRepository<NeedForHourLine>(_context);*/
        public IBaseRepository<AvailablePower> AvailablePowers => _availablePowers ??= new BaseRepository<AvailablePower>(_context);
        public IBaseRepository<PowerPlant> PowerPlants => _powerPlants ??= new BaseRepository<PowerPlant>(_context);
        public IBaseRepository<PowerPlantDay> PowerPlantDays => _powerPlantDays ??= new BaseRepository<PowerPlantDay>(_context);
        public IBaseRepository<HourData> HourDatas => _hourDatas ??= new BaseRepository<HourData>(_context);
        public IBaseRepository<OwnerDeal> OwnerDeals => _ownerDeals ??= new BaseRepository<OwnerDeal>(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}