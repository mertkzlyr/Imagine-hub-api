using System.Security.Claims;
using ImagineHubAPI.Config.Identity;
using ImagineHubAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ImagineHubAPI.Config;

public static class AuthenticationConfig
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        var publicKey = RsaKeyUtils.GetPublicKey(jwtSettings.PublicKeyPath); // RsaSecurityKey

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true; // Keep this true in production
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = publicKey
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError("Authentication failed: {Message}", context.Exception.Message);

                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        });

        // 🔐 Custom Authorization Policies (optional)
        services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityData.AdminPolicy, policy => 
                policy.RequireClaim(ClaimTypes.Role, IdentityData.Admin));

            options.AddPolicy(IdentityData.UserPolicy, policy => 
                policy.RequireClaim(ClaimTypes.Role, IdentityData.User));
        });

        return services;
    }
}