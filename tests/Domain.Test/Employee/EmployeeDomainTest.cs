using VacationSystem.Application.Domain.Vacation;
using CommonTestUtilities.Entities;
using SharedKernel.ValueObjects;

namespace Domain.Test.Employee;

public class EmployeeDomainTest
{
    [Fact]
    public void CreateEmployee_ShouldInitializeCorrectly()
    {
        // Arrange
        var email = Email.Create("employee@test.com");
        var password = Password.Create("password");
        var name = Name.Create("Test", "User");
        var startDate = DateTime.Today.AddYears(-2);
        var departmentId = Guid.NewGuid();

        // Act
        var employee = VacationSystem.Application.Domain.Employee.Employee.Create(email, password, name, startDate, departmentId);

        // Assert
        Assert.Equal(email, employee.Email);
        Assert.Equal(password, employee.Password);
        Assert.Equal(name, employee.Name);
        Assert.Equal(startDate, employee.StartDate);
        Assert.Equal(departmentId, employee.DepartmentId);
        Assert.Null(employee.LastVacationDate);
        Assert.Empty(employee.VacationRequests);
    }

    [Fact]
    public void AddVacationRequest_ShouldAddToVacationRequests()
    {
        // Arrange
        var (employee, _) = EmployeeBuilder.Build(DateTime.Today.AddYears(-2));
        var vacationRequest = VacationRequest.Create(DateTime.Today.AddDays(10), 5, employee);

        // Act
        employee.AddVacationRequest(vacationRequest);

        // Assert
        Assert.Contains(vacationRequest, employee.VacationRequests);
    }

    [Fact]
    public void UpdateLastVacationDate_ShouldSetLastVacationDate()
    {
        // Arrange
        var (employee, _) = EmployeeBuilder.Build();
        var lastVacationDate = DateTime.Today.AddDays(-30);

        // Act
        employee.UpdateLastVacationDate(lastVacationDate);

        // Assert
        Assert.Equal(lastVacationDate, employee.LastVacationDate);
    }
}
