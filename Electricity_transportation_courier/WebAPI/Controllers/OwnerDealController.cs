using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.OwnerDeals;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/operator/[controller]")]
    public class OwnerDealController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnerDealController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOwnerDealCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Message = "Угоду успішно створено.", Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOwnerDealCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Дані угоди оновлено." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteOwnerDealCommand(id));
            return Ok(new { Message = "Угоду видалено." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetOwnerDealByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? searchTerm, [FromQuery] bool? onlyActive)
        {
            var result = await _mediator.Send(new SearchOwnerDealsQuery(searchTerm, onlyActive));
            return Ok(result);
        }
    }
}