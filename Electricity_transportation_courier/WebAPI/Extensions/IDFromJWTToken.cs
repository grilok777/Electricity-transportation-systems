using System.Security.Claims;

namespace WebAPI.Extensions
{
    public static class IDFromJWTToken
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idString, out int id) ? id : throw new UnauthorizedAccessException("Недійсний токен.");
        }
    }
}
