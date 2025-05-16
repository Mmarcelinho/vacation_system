using SharedKernel.Abstractions;
using SharedKernel.Constants;
using SharedKernel.ValueObjects;
using VacationSystem.Application.Domain.Vacation;

namespace VacationSystem.Application.Domain.Admin;

public sealed class Admin : User
{
    private readonly List<VacationRequest> _vacationRequests;

    private Admin() : base()
    {
        _vacationRequests = [];
    }

    private Admin(Email email, Password password, string role, string position, Guid departmentId)
        : base(email, password, role)
    {
        Position = position;
        DepartmentId = departmentId;
        _vacationRequests = [];
    }

    public Name Name { get; private set; } = default!;

    public string Position { get; private set; } = string.Empty;

    public Guid DepartmentId { get; private set; }

    public ICollection<VacationRequest> VacationRequests => _vacationRequests.AsReadOnly();

    public void AddVacationRequest(VacationRequest vacationRequest) => _vacationRequests.Add(vacationRequest);

    public static Admin Create(Email email, Password password, string position, Guid departmentId)
        => new(email, password, Roles.Admin, position, departmentId);
}
