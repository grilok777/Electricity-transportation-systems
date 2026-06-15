using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GridDbContext : DbContext
    {
        public GridDbContext(DbContextOptions<GridDbContext> options) : base(options) { }

        // 1. Таблиці інфраструктури та персоналу
        public DbSet<Operator> Operators => Set<Operator>();
        public DbSet<Substation> Substations => Set<Substation>();
        public DbSet<SubstationLine> SubstationLines => Set<SubstationLine>();

        // 2. Таблиці часу та доступності
        public DbSet<GridDatetime> Datetimes => Set<GridDatetime>();
        public DbSet<AvailablePower> AvailablePowers => Set<AvailablePower>();

        // 3. Таблиці клієнтів (Угоди та Станції)
        public DbSet<OwnerDeal> OwnerDeals => Set<OwnerDeal>();
        public DbSet<PowerPlant> PowerPlants => Set<PowerPlant>();

        // 4. Таблиці прогнозів
        public DbSet<PowerPlantDay> PowerPlantDays => Set<PowerPlantDay>();
        public DbSet<HourData> HourDatas => Set<HourData>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Substation>(entity =>
            {
                entity.Property(s => s.Status).HasConversion<string>();
            });

            modelBuilder.Entity<SubstationLine>(entity =>
            {
                entity.HasOne(l => l.Substation)
                    .WithMany(s => s.SubstationLines) 
                    .HasForeignKey(l => l.SubstationId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<AvailablePower>(entity =>
            {
                entity.HasOne(a => a.SubstationLine)
                    .WithMany(l => l.AvailablePowers)
                    .HasForeignKey(a => a.SubstationLineId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Datetime)
                    .WithMany() 
                    .HasForeignKey(a => a.DatetimeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OwnerDeal>(entity =>
            {
                // Індекс для швидкої синхронізації з Generation
                entity.HasIndex(d => d.LastModifiedAt);
            });

            modelBuilder.Entity<PowerPlant>(entity =>
            {
                entity.Property(p => p.Type).HasConversion<string>();
                entity.Property(p => p.Status).HasConversion<string>();

                // Індекс для швидкої синхронізації
                entity.HasIndex(p => p.LastModifiedAt);

                entity.HasOne(p => p.Deal)
                    .WithMany(d => d.PowerPlants)
                    .HasForeignKey(p => p.DealId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PowerPlantDay>(entity =>
            {
                entity.Property(d => d.Status).HasConversion<string>();

                entity.HasOne(d => d.PowerPlant)
                    .WithMany(p => p.ForecastDays)
                    .HasForeignKey(d => d.PlantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HourData>(entity =>
            {
                entity.HasOne(h => h.PowerPlantDay)
                    .WithMany(d => d.HourDatas)
                    .HasForeignKey(h => h.PowerPlantDayId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                var prop = entry.Entity.GetType().GetProperty("LastModifiedAt");
                if (prop != null) prop.SetValue(entry.Entity, now);

                if (entry.Entity is HourData hourData && entry.State == EntityState.Modified)
                {
                    var parentDay = PowerPlantDays.Local.FirstOrDefault(d => d.Id == hourData.PowerPlantDayId);
                    if (parentDay != null) parentDay.LastModifiedAt = now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}