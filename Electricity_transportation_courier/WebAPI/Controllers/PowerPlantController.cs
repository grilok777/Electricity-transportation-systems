using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.PowerPlants;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/operator/[controller]")]
    public class PowerPlantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PowerPlantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePowerPlantCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Message = "Електростанцію успішно зареєстровано.", Id = id });
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePowerPlantCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Дані електростанції оновлено." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePowerPlantCommand(id));
            return Ok(new { Message = "Електростанцію видалено." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPowerPlantByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? dealId,
            [FromQuery] int? lineId,
            [FromQuery] float? minCapacity,
            [FromQuery] Status? status)
        {
            var result = await _mediator.Send(new SearchPowerPlantsQuery(dealId, lineId, minCapacity, status));
            return Ok(result);
        }
    }
}