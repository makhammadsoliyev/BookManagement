using BookManagement.BusinessLogic.Options;
using BookManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManagement.BusinessLogic.Providers;

public class TokenProvider(
    UserManager<User> userManager,
    IOptions<JwtOptions> jwtOptions,
    JwtSecurityTokenHandler jwtSecurityTokenHandler)
    : ITokenProvider
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;

    public async Task<string> CreateAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var claims = await userManager.GetClaimsAsync(user);

        claims.Add(new Claim(ClaimTypes.Email, user.Email));

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationMinutes),
            signingCredentials: credentials);
        var tokenAsString = jwtSecurityTokenHandler.WriteToken(token);

        return tokenAsString;
    }
}
