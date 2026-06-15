using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenerationDbContext : DbContext
    {
        public GenerationDbContext(DbContextOptions<GenerationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<OwnerDeal> OwnerDeals => Set<OwnerDeal>();
        public DbSet<PowerPlant> PowerPlants => Set<PowerPlant>();
        public DbSet<PowerPlantDay> PowerPlantDays => Set<PowerPlantDay>();
        public DbSet<HourData> HourDatas => Set<HourData>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            modelBuilder.Entity<OwnerDeal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Deals)
                    .HasForeignKey(d => d.UserId) // Зв'язок Угода -> Користувач
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PowerPlant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).HasConversion<string>();
                entity.Property(e => e.Status).HasConversion<string>();

                entity.HasOne(p => p.Deal)
                    .WithMany(d => d.PowerPlants)
                    .HasForeignKey(p => p.DealId) // Зв'язок Станція -> Угода
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PowerPlantDay>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.HasIndex(d => d.LastModifiedAt);
                entity.HasOne(d => d.PowerPlant)
                    .WithMany(p => p.ForecastDays)
                    .HasForeignKey(d => d.PlantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HourData>(entity =>
            {
                entity.HasKey(e => e.Id);

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
