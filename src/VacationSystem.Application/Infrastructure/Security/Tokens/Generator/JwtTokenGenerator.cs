using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Abstractions;
using VacationSystem.Application.Common.Security.Tokens;

namespace VacationSystem.Application.Infrastructure.Security.Tokens.Generator;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;

    private readonly string _signingKey = signingKey;

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email.Value),
            new(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
