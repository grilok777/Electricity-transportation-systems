using Application.Deals;
using Application.Forecasts;
using Application.PowerPlants;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GenerationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenerationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("plant")]
        public async Task<IActionResult> RegisterPlant([FromBody] PowerPlantDto dto)
        {
            try
            {
                int ownerId = User.GetUserId();
                await _mediator.Send(new RegisterPowerPlantCommand(ownerId, dto));
                return Ok(new { Message = "Станцію зареєстровано" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("plant/{plantId}/forecast")]
        public async Task<IActionResult> SubmitForecast(int plantId, [FromBody] SubmitForecastDto request)
        {
            try
            {
                await _mediator.Send(new SubmitDailyForecastCommand(plantId, request.Date, request.HourlyForecasts));
                return Ok(new { Message = "Прогноз успішно подано" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("deal")]
        public async Task<IActionResult> SignDeal( [FromBody] CreateDealDto dto)
        {
            try
            {
                int ownerId = User.GetUserId();
                await _mediator.Send(new SignDealCommand(ownerId, dto));
                return Ok(new { Message = "Угоду успішно укладено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("owner/plants")]
        public async Task<IActionResult> GetOwnerPlants()
        {
            int ownerId = User.GetUserId();
            var plants = await _mediator.Send(new GetUserPlantsQuery(ownerId));
            return Ok(plants);
        }

        [HttpPatch("plant/{plantId}/status")]
        public async Task<IActionResult> UpdatePlantStatus(int plantId, [FromQuery] PlantStatus status)
        {
            try
            {
                await _mediator.Send(new UpdatePlantStatusCommand(plantId, status));
                return Ok(new { Message = "Статус станції оновлено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPatch("forecast/{forecastId}/status")]
        public async Task<IActionResult> UpdateForecastStatus(int forecastId, [FromQuery] DayStatus status)
        {
            try
            {
                await _mediator.Send(new UpdateForecastStatusCommand(forecastId, status));
                return Ok(new { Message = "Статус прогнозу оновлено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPatch("forecast/{forecastId}/cancelpending")]
        public async Task<IActionResult> CancelPendingForecast(int forecastId)
        {
            try
            {
                int ownerId = User.GetUserId();
                await _mediator.Send(new CancelPendingForecastCommand(forecastId, ownerId));
                return Ok(new { Message = "Статус прогнозу скасовано" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("plant/{plantId}/forecast/history")]
        public async Task<IActionResult> GetForecastHistory(int plantId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            try
            {
                var history = await _mediator.Send(new GetForecastHistoryQuery(plantId, startDate, endDate));
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
