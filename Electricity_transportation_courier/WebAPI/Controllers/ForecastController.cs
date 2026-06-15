using Application.Features.Forecasts;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/operator/[controller]")]
    // [Authorize] // Пізніше
    public class ForecastController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForecastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] DayStatus newStatus)
        {
            await _mediator.Send(new UpdateForecastStatusCommand(id, newStatus));
            return Ok(new { Message = $"Статус прогнозу змінено на {newStatus}." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetForecastByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? plantId,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] DayStatus? status)
        {
            var query = new SearchForecastsQuery(plantId, startDate, endDate, status);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}