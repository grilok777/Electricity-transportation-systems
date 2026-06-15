using Application.Features.SubstationLines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/operator/[controller]")]
    public class SubstationLineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubstationLineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubstationLineCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Message = "Лінію успішно створено.", Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSubstationLineCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Дані лінії оновлено." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteSubstationLineCommand(id));
            return Ok(new { Message = "Лінію видалено." });
        }

        [HttpGet("by-substation/{substationId}")]
        public async Task<IActionResult> GetBySubstationId(int substationId)
        {
            var result = await _mediator.Send(new GetSubstationLineByIdQuery(substationId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSubstationLineByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllSubstationLinesQuery());
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search( [FromQuery] int? substationId,[FromQuery] float? minBaseLoad,
                                                 [FromQuery] float? maxBaseLoad,[FromQuery] string? searchTerm)
        {
            var query = new SearchSubstationLinesQuery(substationId, minBaseLoad, maxBaseLoad, searchTerm);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}