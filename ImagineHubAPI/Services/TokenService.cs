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
    
    private object? GetPropertyValue<T>(T entity, string propertyName)
    {
        var property = entity.GetType().GetProperty(propertyName);
        return property?.GetValue(entity);
    }
}