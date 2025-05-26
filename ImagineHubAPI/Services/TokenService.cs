using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ImagineHubAPI.Config;
using ImagineHubAPI.Helpers;
using ImagineHubAPI.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ImagineHubAPI.Services;

public class TokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string CreateToken<T>(T entity)
    {
        // You can customize how you extract claims depending on the type T
        var claims = new List<Claim>();
        
        // Add common claims
        claims.Add(new Claim("id", GetPropertyValue(entity, "Id").ToString()));
        claims.Add(new Claim("email", GetPropertyValue(entity, "Email").ToString()));
        claims.Add(new Claim("role", GetPropertyValue(entity, "Role")?.ToString()));
        
        var privateKey = RsaKeyUtils.GetPrivateKey(_jwtSettings.PrivateKeyPath);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public string CreateMagicLinkToken(int userId, string email)
    {
        var claims = new List<Claim>
        {
            new Claim("id", userId.ToString()),
            new Claim("email", email),
            new Claim("type", "magiclink")  // Optional: add a claim to identify token purpose
        };

        var privateKey = RsaKeyUtils.GetPrivateKey(_jwtSettings.PrivateKeyPath);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30), // shorter expiry for magic link
            signingCredentials: new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Method to validate tokens with optional magic link type check
    public ClaimsPrincipal? ValidateToken(string token, bool requireMagicLinkType = false)
    {
        var publicKey = RsaKeyUtils.GetPublicKey(_jwtSettings.PublicKeyPath);

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = publicKey,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = "email"
            }, out var validatedToken);

            if(requireMagicLinkType)
            {
                var typeClaim = principal.FindFirst("type")?.Value;
                if (typeClaim != "magiclink") return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    
    private object? GetPropertyValue<T>(T entity, string propertyName)
    {
        var property = entity.GetType().GetProperty(propertyName);
        return property?.GetValue(entity);
    }
}