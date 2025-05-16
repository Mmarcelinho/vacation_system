using SharedKernel.ValueObjects;

namespace SharedKernel.Abstractions;

public abstract class User : Entity
{
    protected User() { }

    protected User(Email email, Password password, string role)
    {
        Email = email;
        Password = password;
        Role = role;
        Active = true;
    }

    public Email Email { get; private set; } = default!;

    public Password Password { get; private set; } = default!;

    public string Role { get; private set; } = string.Empty;

    public bool Active { get; private set; }

    public DateTime? LastLogin { get; private set; }

    public void UpdateLoginDate() => LastLogin = DateTime.UtcNow;

    public void ChangePassword(Password password) => Password = password;
}