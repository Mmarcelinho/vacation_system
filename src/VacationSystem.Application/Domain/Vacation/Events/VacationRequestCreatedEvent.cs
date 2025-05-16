using SharedKernel.Abstractions;

namespace VacationSystem.Application.Domain.Vacation.Events;

public record VacationRequestCreatedEvent(
    Guid VacationRequestId,
    DateTime RequestDate,
    DateTime StartDate,
    DateTime EndDate,
    int Days,
    Guid EmployeeId) : IDomainEvent;

