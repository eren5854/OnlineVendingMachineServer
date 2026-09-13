using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineVendingMachineServer.WebAPI.DTOs;
using OnlineVendingMachineServer.WebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnlineVendingMachineServer.WebAPI.Options;

namespace OnlineVendingMachineServer.WebAPI.Services;

internal sealed class JwtProvider(
    IOptions<JwtOption> jwtOption,
    UserManager<AppUser> userManager) : IJwtProvider
{
    public async Task<LoginResponseDto> CreateToken(AppUser user)
    {
        List<Claim> claims = new()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email",  user.Email ?? ""),
            new Claim("UserName", user.UserName ?? ""),
        };

        DateTime expires = DateTime.UtcNow.AddMonths(6);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Value.SecretKey));

        JwtSecurityToken jwtSecurityToken = new(
            issuer: jwtOption.Value.Issuer,
            audience: jwtOption.Value.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512));

        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(jwtSecurityToken);

        string refreshToken = Guid.NewGuid().ToString();
        DateTime refreshToneExpires = expires;

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = refreshToneExpires;

        await userManager.UpdateAsync(user);

        return new(token, refreshToken, refreshToneExpires);
    }
}
