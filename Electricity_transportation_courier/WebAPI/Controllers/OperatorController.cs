using Application.Features.Operators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class OperatorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OperatorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Дозволяємо доступ без токена
        public async Task<IActionResult> Login([FromBody] AuthenticateOperatorQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

        // [Authorize(Roles = "Admin")] 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOperatorCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Message = "Оператора успішно створено.", Id = id });
        }

        // [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOperatorsQuery());
            return Ok(result);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOperatorCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Дані оператора оновлено." });
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteOperatorCommand(id));
            return Ok(new { Message = "Оператора видалено." });
        }
    }
}