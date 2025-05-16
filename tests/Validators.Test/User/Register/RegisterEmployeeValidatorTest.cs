using CommonTestUtilities.Commands;
using FluentAssertions;
using SharedKernel.Errors;
using VacationSystem.Application.Features.Employee.Register;

namespace Validators.Test.User.Register;

public class RegisterEmployeeValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Firstname_Empty()
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build();
        var requestWithFirstnameEmpty = request with { FirstName = string.Empty };

        // Act
        var result = validator.Validate(requestWithFirstnameEmpty);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2)
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.NAME_EMPTY))
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.NAME_LIMIT_CHARACTERS));
    }

    [Fact]
    public void Error_Lastname_Empty()
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build();
        var requestWithLastnameEmpty = request with { FirstName = string.Empty };

        // Act
        var result = validator.Validate(requestWithLastnameEmpty);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2)
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.NAME_EMPTY))
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.NAME_LIMIT_CHARACTERS));
    }

    [Fact]
    public void Error_Name_Exceed_Characters()
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build();
        var requestWithNameExceedCharacters = request with
        {
            FirstName = new string('A', 51)
        };

        // Act
        var result = validator.Validate(requestWithNameExceedCharacters);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2)
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.NAME_EXCEED_LIMIT_CHARACTERS))
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.NAME_LIMIT_CHARACTERS));
    }

    [Fact]
    public void Error_Email_Empty()
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build();
        var requestWithEmailEmpty = request with { Email = string.Empty };

        // Act
        var result = validator.Validate(requestWithEmailEmpty);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2)
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.EMAIL_EMPTY))
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.EMAIL_INVALID));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build();
        var requestWithEmailInvalid = request with { Email = "email.com" };

        // Act
        var result = validator.Validate(requestWithEmailInvalid);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.EMAIL_INVALID));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build(passwordLength);

        // Act
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        // Assert
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.PASSWORD_TOO_SHORT));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        // Arrange
        var validator = new RegisterEmployeeCommandValidator();

        var request = RegisterEmployeeCommandBuilder.Build();
        var requestWithPasswordEmpty = request with { Password = string.Empty };

        // Act
        var result = validator.Validate(requestWithPasswordEmpty);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2)
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.PASSWORD_EMPTY))
            .And.Contain(e => e.ErrorMessage.Equals(DomainErrors.PASSWORD_TOO_SHORT));
    }
}
