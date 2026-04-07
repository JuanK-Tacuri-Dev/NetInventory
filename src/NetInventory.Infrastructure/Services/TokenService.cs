using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetInventory.Application.Auth;
using NetInventory.Resources;

namespace NetInventory.Infrastructure.Services;

public sealed class TokenService(IConfiguration configuration) : ITokenService
{
    public (string Token, DateTime ExpiresAt) GenerateToken(string userId, string email)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException(Messages.Error_JwtSecretNotConfigured);
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiresInMinutes = int.TryParse(jwtSettings["ExpiresInMinutes"], out var minutes) ? minutes : 60;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, email)
        };

        var jwtObj = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(jwtObj), expiresAt);
    }
}
