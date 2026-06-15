
using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto dto)
        {
            try
            {
                await _mediator.Send(new RegisterUserCommand(dto.Username, dto.Password));
                return Ok(new { Message = "Користувача успішно зареєстровано" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var authResult = await _mediator.Send(new AuthenticateUserQuery(dto.Username, dto.Password));

                return Ok(new
                {
                    Token = authResult.Token,
                    OwnerId = authResult.OwnerId
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _mediator.Send(new ResetPasswordCommand(dto.Username, dto.ResetCode, dto.NewPassword));
                return Ok(new { Message = "Пароль успішно відновлено. Тепер ви можете увійти з новим паролем." });
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
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                int ownerId = User.GetUserId();
                var profile = await _mediator.Send(new GetUserProfileQuery(ownerId));
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpPatch("deal/{dealId}/contact")]
        public async Task<IActionResult> UpdateContactInfo(int dealId, [FromBody] ContactInfoDto dto)
        {
            try
            {
                int ownerId = User.GetUserId();
                await _mediator.Send(new UpdateContactInfoCommand(ownerId, dealId, dto));
                return Ok(new { Message = $"Контактну інформацію для угоди №{dealId} успішно оновлено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPatch("password")] 
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                int ownerId = User.GetUserId();

                await _mediator.Send(new ChangePasswordCommand(ownerId, dto.OldPassword, dto.NewPassword));

                return Ok(new { Message = "Пароль успішно змінено" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
