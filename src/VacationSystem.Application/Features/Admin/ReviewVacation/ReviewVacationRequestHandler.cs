using MediatR;
using Microsoft.EntityFrameworkCore;
using VacationSystem.Application.Common.Exceptions;
using VacationSystem.Application.Common.Interfaces;
using VacationSystem.Application.Domain.Vacation.Enums;
using VacationSystem.Application.Infrastructure.Persistence;

namespace VacationSystem.Application.Features.Admin.ReviewVacation;

public record ReviewVacationRequestCommand(Guid VacationRequestId, EStatus Status) : IRequest<Unit>;

internal class ReviewVacationRequestHandler(VacationSystemDbContext dbContext, ILoggedUser loggedUser) : IRequestHandler<ReviewVacationRequestCommand, Unit>
{
    public async Task<Unit> Handle(ReviewVacationRequestCommand command, CancellationToken cancellationToken)
    {
        var userId = await loggedUser.User();

        var admin = await dbContext.Admins.FirstAsync(e => e.Id == userId, cancellationToken);

        var vacationRequest = await dbContext.VacationRequests.FirstOrDefaultAsync(e => e.Id == command.VacationRequestId, cancellationToken);

        var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == vacationRequest!.EmployeeId, cancellationToken);

        if (vacationRequest is null) throw new NotFoundException(nameof(Domain.Vacation.VacationRequest), command.VacationRequestId);

        vacationRequest.ReviewVacationRequest(admin, command.Status);

        admin.AddVacationRequest(vacationRequest);

        if (vacationRequest.Status == EStatus.Approved) employee!.UpdateLastVacationDate(vacationRequest.EndDate);

        dbContext.VacationRequests.Update(vacationRequest);
        dbContext.Employees.Update(employee!);
        dbContext.Admins.Update(admin);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
