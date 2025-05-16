namespace VacationSystem.Application.Common.Security.Tokens;

public interface IAccessTokenValidator
{
    public Guid ValidateAndGetUserIdentifier(string token);
}
