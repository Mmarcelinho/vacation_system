using SharedKernel.Abstractions;
using VacationSystem.Application.Common.Exceptions;
using VacationSystem.Application.Domain.Vacation.Enums;
using VacationSystem.Application.Domain.Vacation.Errors;
using VacationSystem.Application.Domain.Vacation.Events;

namespace VacationSystem.Application.Domain.Vacation;

public sealed class VacationRequest : Entity
{
    private VacationRequest() { }

    private VacationRequest(
        DateTime requestDate,
        DateTime startDate,
        DateTime endDate,
        int days,
        EStatus status,
        Guid employeeId,
        Employee.Employee employee,
        Guid? adminId,
        Admin.Admin? admin)
    {
        RequestDate = requestDate;
        StartDate = startDate;
        EndDate = endDate;
        Days = days;
        Status = status;
        EmployeeId = employeeId;
        Employee = employee;
        AdminId = adminId;
        Admin = admin;
    }

    public DateTime RequestDate { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public int Days { get; private set; }

    public EStatus Status { get; private set; }

    public Guid EmployeeId { get; private set; }

    public Employee.Employee Employee { get; private set; } = default!;

    public Guid? AdminId { get; private set; }

    public Admin.Admin? Admin { get; private set; }

    public static VacationRequest Create(DateTime startDate, int days, Employee.Employee employee)
    {
        ValidateHoliday(employee);
        var request = new VacationRequest(
            requestDate: DateTime.Today,
            startDate: startDate,
            endDate: startDate.AddDays(days),
            days: days,
            status: EStatus.Pending,
            employeeId: employee.Id,
            employee: employee,
            adminId: null,
            admin: null);

        request.AddDomainEvent(new VacationRequestCreatedEvent(
            request.Id,
            request.RequestDate,
            request.StartDate,
            request.EndDate,
            request.Days,
            request.EmployeeId));

        return request;
    }

    public static void ValidateHoliday(Employee.Employee employee)
    {
        if (!ElegibleForVacation(employee.StartDate, employee.LastVacationDate))
            throw new BadRequestException(VacationRequestErrors.EMPLOYEE_NOT_ELEGIBLE);
    }

    private static bool ElegibleForVacation(DateTime startDate, DateTime? lastVacationDate)
    {
        if (lastVacationDate.HasValue)
            return DateTime.Today.Subtract(lastVacationDate.Value).TotalDays >= 365;
        else
            return DateTime.Today.Subtract(startDate).TotalDays >= 365;
    }

    public void ReviewVacationRequest(Admin.Admin admin, EStatus status)
    {
        ValidateStatus();

        if (status == EStatus.Approved)
            Approve(admin);
        else
            Reject(admin);

        AddDomainEvent(new VacationRequestReviewedEvent(
            Id,
            Status,
            admin.Id));
    }

    private void Approve(Admin.Admin admin)
    {
        Status = EStatus.Approved;
        Admin = admin;
        AdminId = admin.Id;
    }

    private void Reject(Admin.Admin admin)
    {
        Status = EStatus.Denied;
        Admin = admin;
        AdminId = admin.Id;
    }

    private void ValidateStatus()
    {
        if (Status != EStatus.Pending)
            throw new Exception(VacationRequestErrors.VACATION_ALREADY_REVIEWED);
    }
}
