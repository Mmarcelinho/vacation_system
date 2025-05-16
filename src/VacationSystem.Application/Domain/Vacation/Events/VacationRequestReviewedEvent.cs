using SharedKernel.Abstractions;
using VacationSystem.Application.Domain.Vacation.Enums;

namespace VacationSystem.Application.Domain.Vacation.Events;

public record VacationRequestReviewedEvent(
    Guid VacationRequestId,
    EStatus Status,
    Guid AdminId) : IDomainEvent;
