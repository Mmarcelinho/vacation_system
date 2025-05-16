namespace VacationSystem.Application.Common.Interfaces;

public interface ILoggedUser
{
    public Task<Guid> User();
}