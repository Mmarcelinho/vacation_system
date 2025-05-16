using MediatR;
using Microsoft.EntityFrameworkCore;
using VacationSystem.Application.Common.Security.Cryptography;
using VacationSystem.Application.Common.Security.Tokens;
using VacationSystem.Application.Infrastructure.Persistence;

namespace VacationSystem.Application.Features.Login.DoLogin;

public record DoLoginCommand(string Email, string Password) : IRequest<DoLoginResult>;
public record DoLoginResult(string Token);

internal class DoLoginHandler(
    VacationSystemDbContext dbContext,
    IPasswordEncripter passwordEncripter,
    IAccessTokenGenerator accessTokenGenerator) : IRequestHandler<DoLoginCommand, DoLoginResult>
{
    public async Task<DoLoginResult> Handle(DoLoginCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.Value.Equals(command.Email) && u.Active, cancellationToken);

        if (user is null || passwordEncripter.IsValid(command.Password, user.Password.Value) == false)
            throw new UnauthorizedAccessException("Invalid email or password.");

        user.UpdateLoginDate();

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var token = accessTokenGenerator.Generate(user);

        return new(token);
    }
}
