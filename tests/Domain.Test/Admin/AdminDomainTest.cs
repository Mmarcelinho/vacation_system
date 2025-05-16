using VacationSystem.Application.Domain.Vacation;
using CommonTestUtilities.Entities;
using SharedKernel.ValueObjects;

namespace Domain.Test.Admin;

public class AdminDomainTest
{
    [Fact]
    public void CreateAdmin_ShouldInitializeCorrectly()
    {
        // Arrange
        var email = Email.Create("admin@test.com");
        var password = Password.Create("password");
        var position = "Manager";
        var departmentId = Guid.NewGuid();

        // Act
        var admin = VacationSystem.Application.Domain.Admin.Admin.Create(email, password, position, departmentId);

        // Assert
        Assert.Equal(email, admin.Email);
        Assert.Equal(password, admin.Password);
        Assert.Equal(position, admin.Position);
        Assert.Equal(departmentId, admin.DepartmentId);
        Assert.Equal("Admin", admin.Role);
        Assert.Empty(admin.VacationRequests);
    }

    [Fact]
    public void AddVacationRequest_ShouldAddToVacationRequests()
    {
        // Arrange
        var (admin, _) = AdminBuilder.Build();

        var (employee, _) = EmployeeBuilder.Build(DateTime.Today.AddYears(-2));
        var vacationRequest = VacationRequest.Create(DateTime.Today.AddDays(7), 3, employee);

        // Act
        admin.AddVacationRequest(vacationRequest);

        // Assert
        Assert.Contains(vacationRequest, admin.VacationRequests);
    }
}
