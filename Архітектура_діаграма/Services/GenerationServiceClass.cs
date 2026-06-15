using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;


namespace Application.Services
{
    public class GenerationServiceClass 
    { 
    private readonly IGenerationUnitOfWork _uow;

        public GenerationServiceClass(
            IGenerationUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task RegisterPowerPlantAsync(int ownerId, PowerPlantDto dto)
        {
            // Перетворюємо Enum з рядка 
            Enum.TryParse(dto.Type, true, out PlantType type);

            var plant = new PowerPlant
            {
                OwnerId = ownerId,
                Location = dto.Location,
                MaxCapacityKw = dto.MaxCapacityKw,
                Type = type,
                Status = PlantStatus.Active,
                DateCommissioning = DateTime.UtcNow
            };

            await _uow.PowerPlants.AddAsync(plant);
            await _uow.SaveChangesAsync();
        }

        public async Task SubmitDailyForecastAsync(int plantId, DateOnly date,  List<HourDataDto> forecasts)
        {
            var plant = await _uow.PowerPlants.GetByIdAsync(plantId);
            if (plant == null) throw new Exception("Станцію не знайдено.");

            if (plant.Status != PlantStatus.Active)
                throw new InvalidOperationException($"Неможливо подати прогноз: поточний статус станції - {plant.Status}.");

            var commissioningDate = DateOnly.FromDateTime(plant.DateCommissioning);
            if (date < commissioningDate)
                throw new InvalidOperationException($"Неможливо подати прогноз: {date} передує даті запуску станції ({commissioningDate}).");

            if (plant.DateDecommissioning.HasValue)
            {
                var decommissioningDate = DateOnly.FromDateTime(plant.DateDecommissioning.Value);
                if (date > decommissioningDate)
                    throw new InvalidOperationException($"Неможливо подати прогноз: станцію було виведено з експлуатації {decommissioningDate}.");
            }

            var existingForecasts = await _uow.PowerPlantDays.FindAsync(d => d.PlantId == plantId && d.ForecastDate == date);

            if (existingForecasts.Any(f => f.Status == DayStatus.Pending || f.Status == DayStatus.Approved))
            {
                throw new InvalidOperationException("На цю дату вже існує активний прогноз (очікує підтвердження або затверджений). Скасуйте його, щоб подати новий.");
            }

            int nextRetryCount = 0;
            if (existingForecasts.Any())
            {
                nextRetryCount = existingForecasts.Max(f => f.RetryCount) + 1;
            }

            if (forecasts.Count != 24)
               throw new ArgumentException("Має бути рівно 24 записи для кожної години доби.");    

            var newDay = new PowerPlantDay
            {
                PlantId = plantId,
                ForecastDate = date,
                Status = DayStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var hr in forecasts)
            {
                newDay.AddHourForecast(hr.Hour, hr.ForecastedKw, plant.MaxCapacityKw);
            }

            await _uow.PowerPlantDays.AddAsync(newDay);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<ForecastSummaryDto>> GetForecastHistoryAsync(int plantId, DateOnly startDate, DateOnly endDate)
        {
            var history = await _uow.PowerPlantDays.FindAsync(
                d => d.PlantId == plantId &&
                d.ForecastDate >= startDate &&
                d.ForecastDate <= endDate,
                d => d.HourDatas
            );

            return history.Select(d => new ForecastSummaryDto
            {
                Id = d.Id,
                ForecastDate = d.ForecastDate,
                Status = d.Status.ToString(),
                TotalDailyKw = d.HourDatas.Sum(h => h.ForecastedKw),
                HourlyForecasts = d.HourDatas.Select(h => new HourDataDto
                {
                    Hour = h.ForecastTimestamp.Hour,
                    ForecastedKw = h.ForecastedKw
                }).OrderBy(h => h.Hour).ToList()
            }).ToList();
        }

        public async Task CancelPendingForecastAsync(int forecastId, int ownerId)
        {
            var forecast = await _uow.PowerPlantDays.GetByIdAsync(forecastId);
            if (forecast == null) throw new Exception("Прогноз не знайдено.");

            // Перевірка станція:=власник
            var plant = await _uow.PowerPlants.GetByIdAsync(forecast.PlantId);
            if (plant == null || plant.OwnerId != ownerId)
                throw new UnauthorizedAccessException("Ви не маєте доступу до цієї станції.");

            if (forecast.Status != DayStatus.Pending)
                throw new InvalidOperationException("Тільки прогнози в статусі 'Pending' можна скасувати.");

            forecast.Status = DayStatus.Canceled;

            _uow.PowerPlantDays.Update(forecast);
            await _uow.SaveChangesAsync();
        }
        public async Task SignDealAsync(int ownerId, CreateDealDto dto)
        {
            var deal = new OwnerDeal
            {
                OwnerId = ownerId,
                OwnerName = dto.OwnerName,
                NumberPhone = dto.NumberPhone,
                PlaceLocation = dto.PlaceLocation,
                ConclusionDeal = DateOnly.FromDateTime(dto.ConclusionDeal),
                CompetionDeal = DateOnly.FromDateTime(dto.CompletionDeal) 
            };

            await _uow.OwnerDeals.AddAsync(deal);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<PowerPlantDto>> GetOwnerPlantsAsync(int ownerId)
        {
            var plants = await _uow.PowerPlants.FindAsync(p => p.OwnerId == ownerId);
        
            return plants.Select(p => new PowerPlantDto
            {
                Id = p.Id,
                Type = p.Type.ToString(),
                MaxCapacityKw = p.MaxCapacityKw,
                Location = p.Location,
                Status = p.Status.ToString()
            }).ToList();
        }

        public async Task UpdatePlantStatusAsync(int plantId, PlantStatus newStatus)
        {
            var plant = await _uow.PowerPlants.GetByIdAsync(plantId);
            if (plant == null) throw new Exception("Станцію не знайдено.");

            plant.Status = newStatus;
            _uow.PowerPlants.Update(plant);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateForecastStatusAsync(int forecastId, DayStatus status)
        {
            var forecast = await _uow.PowerPlantDays.GetByIdAsync(forecastId);
            if (forecast == null) throw new Exception("Прогноз не знайдено.");

            forecast.Status = status;
            forecast.ProcessedAt = DateTime.UtcNow;

            _uow.PowerPlantDays.Update(forecast);
            await _uow.SaveChangesAsync();
        }
    }
}
