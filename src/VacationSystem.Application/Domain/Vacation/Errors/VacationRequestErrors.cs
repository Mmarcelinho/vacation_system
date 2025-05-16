namespace VacationSystem.Application.Domain.Vacation.Errors;

public static class VacationRequestErrors
{
    public const string EMPLOYEE_NOT_ELEGIBLE = "Employee is not elegible for vacation.";

    public const string VACATION_ALREADY_REVIEWED = "Vacation request already reviewed.";

    public const string START_DATE_REQUIRED = "Start date is required.";

    public const string START_DATE_IN_PAST = "Start date must be in the future.";

    public const string NUMBER_OF_DAYS_INVALID = "Number of days must be greater than zero.";
}
