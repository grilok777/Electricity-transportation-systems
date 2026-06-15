using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.AvailablePowers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/operator/[controller]")]
    public class AvailablePowerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AvailablePowerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAvailablePowerCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Message = "Запис про доступну потужність створено.", Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAvailablePowerCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Кількість доступних станцій оновлено." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAvailablePowerCommand(id));
            return Ok(new { Message = "Запис видалено." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAvailablePowerByIdQuery(id));
            return Ok(result);
        }

        // Цей метод може віддати без фільтрів ВСЕ, або відфільтроване
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? substationLineId,
            [FromQuery] int? datetimeId)
        {
            var result = await _mediator.Send(new SearchAvailablePowerQuery(substationLineId, datetimeId));
            return Ok(result);
        }
    }
}
