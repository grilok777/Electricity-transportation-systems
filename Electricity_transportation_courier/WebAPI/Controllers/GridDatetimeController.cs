using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.GridDatetimes;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/operator/[controller]")]
    public class GridDatetimeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GridDatetimeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGridDatetimeCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Message = "Запис часу створено.", Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateGridDatetimeCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Запис часу оновлено." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteGridDatetimeCommand(id));
            return Ok(new { Message = "Запис часу видалено." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetGridDatetimeByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllGridDatetimesQuery());
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] bool? deficitOnly)
        {
            var query = new SearchGridDatetimesQuery(startDate, endDate, deficitOnly);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}