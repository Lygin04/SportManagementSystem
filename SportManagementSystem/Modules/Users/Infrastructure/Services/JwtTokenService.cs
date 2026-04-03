using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SportManagementSystem.BuildingBlocks.Authentication.Jwt.Interfaces;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Users.Contracts.Response;
using SportManagementSystem.Modules.Users.Domain.Services;

namespace SportManagementSystem.Modules.Users.Infrastructure.Services;

public class JwtTokenService(IJwtSettings jwtSettings, IAppClock clock) : IJwtTokenService
{
    public Task<LoginUserResponse> CreateAccessTokenAsync(ICollection<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        var expires = clock.UtcNow.AddHours(jwtSettings.TokenExpiresAfterHours);

        var token = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            claims,
            null,
            expires.UtcDateTime,
            new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return Task.FromResult(new LoginUserResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = expires
        });
    }
}
