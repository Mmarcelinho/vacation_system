using SharedKernel.Abstractions;
using SharedKernel.Constants;
using SharedKernel.ValueObjects;
using VacationSystem.Application.Domain.Vacation;

namespace VacationSystem.Application.Domain.Employee;

public sealed class Employee : User
{
    private readonly List<VacationRequest> _vacationRequests;

    private Employee() : base()
    {
        _vacationRequests = [];
    }

    private Employee(Email email, Password password, Name name, string role, DateTime startDate, Guid departmentId)
        : base(email, password, role)
    {
        Name = name;
        StartDate = startDate;
        DepartmentId = departmentId;
        _vacationRequests = [];
    }

    public Name Name { get; private set; } = default!;

    public DateTime StartDate { get; private set; }

    public DateTime? LastVacationDate { get; private set; }

    public Guid DepartmentId { get; private set; }

    public ICollection<VacationRequest> VacationRequests => _vacationRequests.AsReadOnly();

    public void UpdateLastVacationDate(DateTime lastVacationDate) => LastVacationDate = lastVacationDate;

    public void AddVacationRequest(VacationRequest vacationRequest) => _vacationRequests.Add(vacationRequest);

    public static Employee Create(Email email, Password password, Name name, Guid departmentId)
        => new(email, password, name, Roles.Employee, DateTime.Now, departmentId);

    public static Employee Create(Email email, Password password, Name name, DateTime startDate, Guid departmentId)
        => new(email, password, name, Roles.Employee, startDate, departmentId);
}
