using System;
using Xunit;
using VacationSystem.Application.Domain.Vacation;
using VacationSystem.Application.Domain.Vacation.Enums;
using VacationSystem.Application.Domain.Vacation.Errors;
using VacationSystem.Application.Common.Exceptions;
using CommonTestUtilities.Entities;

namespace Domain.Test.Vacation;

public class VacationRequestDomainTest
{
    [Fact]
    public void CreateVacationRequest_WithEligibleEmployee_ShouldSucceed()
    {
        // Arrange
        var (employee, _) = EmployeeBuilder.Build(DateTime.Today.AddYears(-2));
        var startDate = DateTime.Today.AddDays(10);
        int days = 5;
        
        // Act
        var vacationRequest = VacationRequest.Create(startDate, days, employee);

        // Assert
        Assert.Equal(DateTime.Today, vacationRequest.RequestDate);
        Assert.Equal(startDate, vacationRequest.StartDate);
        Assert.Equal(startDate.AddDays(days), vacationRequest.EndDate);
        Assert.Equal(days, vacationRequest.Days);
        Assert.Equal(EStatus.Pending, vacationRequest.Status);
        Assert.Equal(employee.Id, vacationRequest.EmployeeId);
    }

    [Fact]
    public void CreateVacationRequest_WithIneligibleEmployee_ShouldThrowException()
    {
        // Arrange
        var (employee, _) = EmployeeBuilder.Build(DateTime.Today.AddMonths(-6));
        var startDate = DateTime.Today.AddDays(10);
        int days = 5;
        
        // Act & Assert
        var ex = Assert.Throws<BadRequestException>(() => VacationRequest.Create(startDate, days, employee));
        Assert.Equal(VacationRequestErrors.EMPLOYEE_NOT_ELEGIBLE, ex.Message);
    }

    [Fact]
    public void ReviewVacationRequest_Approve_ShouldSetStatusToApprovedAndAssignAdmin()
    {
        // Arrange
        var (employee, _) = EmployeeBuilder.Build(DateTime.Today.AddYears(-2));
        var vacationRequest = VacationRequest.Create(DateTime.Today.AddDays(10), 5, employee);
        var (admin, _) = AdminBuilder.Build();

        // Act
        vacationRequest.ReviewVacationRequest(admin, EStatus.Approved);

        // Assert
        Assert.Equal(EStatus.Approved, vacationRequest.Status);
        Assert.Equal(admin, vacationRequest.Admin);
    }

    [Fact]
    public void ReviewVacationRequest_Reject_ShouldSetStatusToDeniedAndAssignAdmin()
    {
        // Arrange
        var (employee, _) = EmployeeBuilder.Build(DateTime.Today.AddYears(-2));
        var vacationRequest = VacationRequest.Create(DateTime.Today.AddDays(10), 5, employee);
        var (admin, _) = AdminBuilder.Build();

        // Act
        vacationRequest.ReviewVacationRequest(admin, EStatus.Denied);

        // Assert
        Assert.Equal(EStatus.Denied, vacationRequest.Status);
        Assert.Equal(admin, vacationRequest.Admin);
    }

    [Fact]
    public void ReviewVacationRequest_WhenAlreadyReviewed_ShouldThrowException()
    {
        // Arrange
        var (employee, _) = EmployeeBuilder.Build(DateTime.Today.AddYears(-2));
        var vacationRequest = VacationRequest.Create(DateTime.Today.AddDays(10), 5, employee);
        var (admin, _) = AdminBuilder.Build();
        // Primeira revisão
        vacationRequest.ReviewVacationRequest(admin, EStatus.Approved);

        var (newAdmin, _) = AdminBuilder.Build();

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => vacationRequest.ReviewVacationRequest(newAdmin, EStatus.Denied));
        Assert.Equal(VacationRequestErrors.VACATION_ALREADY_REVIEWED, ex.Message);
    }
}
