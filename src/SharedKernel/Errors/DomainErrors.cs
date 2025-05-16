namespace SharedKernel.Errors;

public static class DomainErrors
{
    public const string NAME_EMPTY = "Name cannot be empty or whitespace.";

    public const string NAME_LIMIT_CHARACTERS = "Name must be between 3 and 20 characters.";

    public const string NAME_EXCEED_LIMIT_CHARACTERS = "Name must not exceed 50 characters.";

    public const string NAME_INVALID_CHARACTERS = "Name contains invalid characters.";

    public const string EMAIL_EMPTY = "Email cannot be empty or whitespace.";

    public const string EMAIL_INVALID = "Invalid email format.";

    public const string PASSWORD_EMPTY = "Password cannot be null or empty.";

    public const string PASSWORD_TOO_SHORT = "Password must be at least 6 characters long.";

    public const string PASSWORD_DIFFERENT_CURRENT_PASSWORD = "The password entered is different from the current password.";
}