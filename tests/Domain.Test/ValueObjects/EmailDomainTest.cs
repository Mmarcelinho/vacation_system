using SharedKernel.Errors;
using SharedKernel.ValueObjects;

namespace Domain.Test.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co")]
    public void CreateEmail_WithValidEmail_ShouldSucceed(string emailValue)
    {
        // Act
        var email = Email.Create(emailValue);

        // Assert
        Assert.Equal(emailValue, email.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateEmail_WithNullOrWhitespace_ShouldThrowException(string? emailValue)
    {
        // Act & Assert
        var ex = Assert.Throws<Exception>(() => Email.Create(emailValue));
        Assert.Equal(DomainErrors.EMAIL_EMPTY, ex.Message);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("invalid@")]
    [InlineData("@invalid.com")]
    public void CreateEmail_WithInvalidFormat_ShouldThrowException(string emailValue)
    {
        // Act & Assert
        var ex = Assert.Throws<Exception>(() => Email.Create(emailValue));
        Assert.Equal(DomainErrors.EMAIL_INVALID, ex.Message);
    }
}



