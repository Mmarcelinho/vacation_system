using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VacationSystem.Application.Common.Interfaces;
using VacationSystem.Application.Domain.Vacation.Errors;
using VacationSystem.Application.Infrastructure.Persistence;

namespace VacationSystem.Application.Features.Employee.RequestVacation;

public record RequestVacationCommand(DateTime StartDate, int Days) : IRequest<RequestVacationResult>;
public record RequestVacationResult(Guid Id);

public class RequestVacationCommandValidator : AbstractValidator<RequestVacationCommand>
{

    public RequestVacationCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .Must(date => date != DateTime.MinValue).WithMessage(VacationRequestErrors.START_DATE_REQUIRED)
            .GreaterThan(DateTime.Today).WithMessage(VacationRequestErrors.START_DATE_IN_PAST);

        RuleFor(x => x.Days)
            .GreaterThan(0).WithMessage(VacationRequestErrors.NUMBER_OF_DAYS_INVALID);
    }
}

internal class RequestVacationHandler(
    VacationSystemDbContext dbContext,
    ILoggedUser loggedUser,
    IMediator mediator) : IRequestHandler<RequestVacationCommand, RequestVacationResult>
{
    public async Task<RequestVacationResult> Handle(RequestVacationCommand command, CancellationToken cancellationToken)
    {
        var userId = await loggedUser.User();

        var employee = await dbContext.Employees.FirstAsync(e => e.Id == userId, cancellationToken);

        Domain.Vacation.VacationRequest.ValidateHoliday(employee);

        var vacation = Domain.Vacation.VacationRequest.Create(command.StartDate, command.Days, employee);

        employee.AddVacationRequest(vacation);
        employee.UpdateLastVacationDate(vacation.StartDate);

        await dbContext.VacationRequests.AddAsync(vacation, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var domainEvents = vacation.DomainEvents.ToList();
        vacation.ClearDomainEvents();

        foreach (var domainEvent in domainEvents) await mediator.Publish(domainEvent, cancellationToken);

        return new(vacation.Id);
    }
}
