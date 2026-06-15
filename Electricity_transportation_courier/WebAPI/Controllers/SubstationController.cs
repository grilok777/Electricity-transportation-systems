using Application.Features.Substations;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/operator/[controller]")]
    // [Authorize] // Розкоментуй коли додам логін для оператора
    public class SubstationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubstationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubstationCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Message = "Підстанцію створено.", Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSubstationCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Підстанцію оновлено." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteSubstationCommand(id));
            return Ok(new { Message = "Підстанцію видалено." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSubstationByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllSubstationsQuery());
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] float? minCapacity, [FromQuery] float? maxCapacity, [FromQuery] Status? status)
        {
            var result = await _mediator.Send(new SearchSubstationsQuery(minCapacity, maxCapacity, status));
            return Ok(result);
        }
    }
}