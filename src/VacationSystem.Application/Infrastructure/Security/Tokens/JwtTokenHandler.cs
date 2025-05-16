using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VacationSystem.Application.Infrastructure.Security.Tokens;

public abstract class JwtTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);

        return new(bytes);
    }
}