using VacationSystem.Application.Common.Security.Cryptography;
using VacationSystem.Application.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Criptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new BCryptNet();
}
