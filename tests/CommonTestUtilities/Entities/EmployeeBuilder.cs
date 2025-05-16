using Bogus;
using CommonTestUtilities.Criptography;
using SharedKernel.ValueObjects;
using VacationSystem.Application.Domain.Employee;

namespace CommonTestUtilities.Entities;

public class EmployeeBuilder
{
    public static (Employee, string password) Build(DateTime? startDate = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();

        var password = new Faker().Internet.Password();

        var NameFaker = new Faker<Name>()
            .CustomInstantiator(f => Name.Create(f.Name.FirstName(), f.Name.LastName()));

        var emailFaker = new Faker<Email>()
            .CustomInstantiator(f => Email.Create(f.Internet.Email()));

        var passwordFaker = new Faker<Password>()
            .CustomInstantiator(f => Password.Create(passwordEncripter.Encrypt(password)));

        if (!startDate.HasValue) startDate = DateTime.Now;

        var employee = new Faker<Employee>()
            .CustomInstantiator(f => Employee.Create(
                emailFaker.Generate(),
                passwordFaker.Generate(),
                NameFaker.Generate(),
                startDate.Value,
                Guid.NewGuid()));    

        return (employee.Generate(), password);
    }
}
