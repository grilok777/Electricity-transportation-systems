using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly GridDbContext _context;

        public SyncController(GridDbContext context) => _context = context;

        [HttpGet("deals")]
        public async Task<IActionResult> GetDealsSync([FromQuery] DateTime? since)
        {
            var query = _context.OwnerDeals.AsQueryable();
            if (since.HasValue) query = query.Where(d => d.LastModifiedAt > since.Value);

            return Ok(await query.ToListAsync());
        }

        [HttpGet("plants")]
        public async Task<IActionResult> GetPlantsSync([FromQuery] DateTime? since)
        {
            var query = _context.PowerPlants.AsQueryable();
            if (since.HasValue) query = query.Where(p => p.LastModifiedAt > since.Value);

            return Ok(await query.ToListAsync());
        }

        [HttpGet("forecasts-full")]
        public async Task<IActionResult> GetFullForecastsSync([FromQuery] DateTime? since)
        {
            var query = _context.PowerPlantDays
                .Include(d => d.HourDatas) 
                .AsQueryable();

            if (since.HasValue)
                query = query.Where(d => d.LastModifiedAt > since.Value);

            return Ok(await query.ToListAsync());
        }
    }
}