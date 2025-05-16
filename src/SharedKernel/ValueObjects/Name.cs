using SharedKernel.Errors;

namespace SharedKernel.ValueObjects;

public sealed record Name
{
    public string Firstname { get; }

    public string Lastname { get; }

    private Name(string firstname, string lastname)
    {
        if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
            throw new Exception(DomainErrors.NAME_EMPTY);

        Firstname = firstname;
        Lastname = lastname;
    }
    public static Name Create(string firstname, string lastname) => new(firstname, lastname);
}
