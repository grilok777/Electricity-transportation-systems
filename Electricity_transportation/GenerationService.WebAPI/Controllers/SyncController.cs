using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly GenerationDbContext _context;

        public SyncController(GenerationDbContext context) => _context = context;

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