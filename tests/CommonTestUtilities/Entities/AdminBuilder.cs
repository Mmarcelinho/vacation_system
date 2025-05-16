using Bogus;
using CommonTestUtilities.Criptography;
using SharedKernel.ValueObjects;
using VacationSystem.Application.Domain.Admin;

namespace CommonTestUtilities.Entities;

public class AdminBuilder
{
    public static (Admin, string password) Build()
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();

        var password = new Faker().Internet.Password();

        var NameFaker = new Faker<Name>()
            .CustomInstantiator(f => Name.Create(f.Name.FirstName(), f.Name.LastName()));

        var emailFaker = new Faker<Email>()
            .CustomInstantiator(f => Email.Create(f.Internet.Email()));

        var passwordFaker = new Faker<Password>()
            .CustomInstantiator(f => Password.Create(passwordEncripter.Encrypt(password)));

        var admin = new Faker<Admin>()
            .CustomInstantiator(f => Admin.Create(
                emailFaker.Generate(),
                passwordFaker.Generate(),
                f.Name.JobTitle(),
                Guid.NewGuid()));

        return (admin.Generate(), password);
    }
}
