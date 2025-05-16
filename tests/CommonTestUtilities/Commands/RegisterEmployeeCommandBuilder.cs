using Bogus;
using VacationSystem.Application.Features.Employee.Register;

namespace CommonTestUtilities.Commands;

public class RegisterEmployeeCommandBuilder
{
    public static RegisterEmployeeCommand Build(int passwordLength = 10)
    {
        var faker = new Faker();

        return new(
            faker.Person.FirstName,
            faker.Person.LastName,
            faker.Person.Email,
            faker.Internet.Password(passwordLength)
        );
    }
}
