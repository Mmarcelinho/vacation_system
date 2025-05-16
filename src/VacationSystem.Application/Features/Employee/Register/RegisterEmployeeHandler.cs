using FluentValidation;
using MediatR;
using SharedKernel.Errors;
using SharedKernel.ValueObjects;
using VacationSystem.Application.Common.Security.Cryptography;
using VacationSystem.Application.Infrastructure.Persistence;

namespace VacationSystem.Application.Features.Employee.Register;

public record RegisterEmployeeCommand(string FirstName, string LastName, string Email, string Password) : IRequest<RegisterEmployeeResult>;
public record RegisterEmployeeResult(Guid Id);

public class RegisterEmployeeCommandValidator : AbstractValidator<RegisterEmployeeCommand>
{
    public RegisterEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(DomainErrors.NAME_EMPTY)
            .Length(3, 20).WithMessage(DomainErrors.NAME_LIMIT_CHARACTERS)
            .MaximumLength(50).WithMessage(DomainErrors.NAME_EXCEED_LIMIT_CHARACTERS);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(DomainErrors.NAME_EMPTY)
            .Length(3, 20).WithMessage(DomainErrors.NAME_LIMIT_CHARACTERS)
            .MaximumLength(50).WithMessage(DomainErrors.NAME_EXCEED_LIMIT_CHARACTERS);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(DomainErrors.EMAIL_EMPTY)
            .EmailAddress().WithMessage(DomainErrors.EMAIL_INVALID);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(DomainErrors.PASSWORD_EMPTY)
            .MinimumLength(6).WithMessage(DomainErrors.PASSWORD_TOO_SHORT);
    }
}

internal class RegisterEmployeeHandler(
    VacationSystemDbContext dbContext,
    IPasswordEncripter passwordEncripter) : IRequestHandler<RegisterEmployeeCommand, RegisterEmployeeResult>
{
    public async Task<RegisterEmployeeResult> Handle(RegisterEmployeeCommand command, CancellationToken cancellationToken)
    {
        var passwordEncripted = passwordEncripter.Encrypt(command.Password);

        var email = Email.Create(command.Email);
        var password = Password.Create(passwordEncripted);
        var name = Name.Create(command.FirstName, command.LastName);

        var employee = Domain.Employee.Employee.Create(email, password, name, Guid.NewGuid());

        await dbContext.Employees.AddAsync(employee, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new(employee.Id);
    }
}
