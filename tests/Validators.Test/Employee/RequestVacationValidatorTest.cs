using CommonTestUtilities.Commands;
using FluentAssertions;
using VacationSystem.Application.Domain.Vacation.Errors;
using VacationSystem.Application.Features.Employee.RequestVacation;

namespace Validators.Test.Employee;

public class RequestVacationValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RequestVacationCommandValidator();

        var request = RequestVacationCommandBuilder.Build(startDate: DateTime.Now, days: 5);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_StartDate_Invalid()
    {
        // Arrange
        var validator = new RequestVacationCommandValidator();

        var request = RequestVacationCommandBuilder.Build(startDate: DateTime.MinValue, days: 5);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2)
            .And.Contain(e => e.ErrorMessage.Equals(VacationRequestErrors.START_DATE_REQUIRED))
            .And.Contain(e => e.ErrorMessage.Equals(VacationRequestErrors.START_DATE_IN_PAST));
    }

    [Fact]
    public void Error_StartDate_InPast()
    {
        // Arrange
        var validator = new RequestVacationCommandValidator();

        var request = RequestVacationCommandBuilder.Build(startDate: DateTime.Today.AddDays(-1), days: 5);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(VacationRequestErrors.START_DATE_IN_PAST));
    }

    [Fact]
    public void Error_Days_Invalid()
    {
        // Arrange
        var validator = new RequestVacationCommandValidator();

        var request = RequestVacationCommandBuilder.Build(startDate: DateTime.Now, days: -1);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(VacationRequestErrors.NUMBER_OF_DAYS_INVALID));
    }
}
