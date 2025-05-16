using SharedKernel.Abstractions;

namespace VacationSystem.Application.Common.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(User user);
}