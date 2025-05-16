using SharedKernel.Errors;

namespace SharedKernel.ValueObjects;

public sealed record Password
{
    public string Value { get; }

    private Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception(DomainErrors.PASSWORD_EMPTY);

        if (value.Length < 6)
            throw new Exception(DomainErrors.PASSWORD_TOO_SHORT);

        Value = value;
    }

    public static Password Create(string value) => new(value);

    public override string ToString() => Value;
}