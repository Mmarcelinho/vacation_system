using SharedKernel.Errors;
using SharedKernel.ValueObjects;

namespace Domain.Test.ValueObjects;

public class PasswordDomainTest
{
    [Theory]
    [InlineData("password")]
    [InlineData("123456")]
    public void CreatePassword_WithValidPassword_ShouldSucceed(string passwordValue)
    {
        // Act
        var password = Password.Create(passwordValue);

        // Assert
        Assert.Equal(passwordValue, password.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreatePassword_WithNullOrWhitespace_ShouldThrowException(string passwordValue)
    {
        // Act & Assert
        var ex = Assert.Throws<Exception>(() => Password.Create(passwordValue));
        Assert.Equal(DomainErrors.PASSWORD_EMPTY, ex.Message);
    }

    [Theory]
    [InlineData("12345")]
    public void CreatePassword_WithShortPassword_ShouldThrowException(string passwordValue)
    {
        // Act & Assert
        var ex = Assert.Throws<Exception>(() => Password.Create(passwordValue));
        Assert.Equal("Password must be at least 6 characters long.", ex.Message);
    }
}
