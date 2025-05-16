using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VacationSystem.Application.Common.Interfaces;
using VacationSystem.Application.Common.Security.Tokens;

namespace VacationSystem.Application.Infrastructure.Services.LoggedUser;

public class LoggedUser(ITokenProvider tokenProvider) : ILoggedUser
{
    public Task<Guid> User()
    {
        var token = tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return Task.FromResult(userIdentifier);
    }
}
