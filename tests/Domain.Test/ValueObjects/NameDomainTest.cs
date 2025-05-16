using SharedKernel.Errors;
using SharedKernel.ValueObjects;

namespace Domain.Test.ValueObjects;

public class NameDomainTest
{
    [Theory]
    [InlineData("John", "Doe")]
    [InlineData("Alice", "Smith")]
    public void CreateName_WithValidNames_ShouldSucceed(string firstName, string lastName)
    {
        // Act
        var name = Name.Create(firstName, lastName);

        // Assert
        Assert.Equal(firstName, name.Firstname);
        Assert.Equal(lastName, name.Lastname);
    }

    [Theory]
    [InlineData("", "Doe")]
    [InlineData("John", "")]
    [InlineData("   ", "Smith")]
    [InlineData("Alice", "   ")]
    public void CreateName_WithEmptyOrWhitespace_ShouldThrowException(string firstName, string lastName)
    {
        // Act & Assert
        var ex = Assert.Throws<Exception>(() => Name.Create(firstName, lastName));
        Assert.Equal(DomainErrors.NAME_EMPTY, ex.Message);
    }
}