using System.Security.Claims;

namespace ImagineHubAPI.Interfaces;

public interface ITokenService
{
    string CreateToken<T>(T entity);
    string CreateMagicLinkToken(int userId, string email);
    ClaimsPrincipal? ValidateToken(string token, bool requireMagicLinkType = false);
}