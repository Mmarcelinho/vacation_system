using Bogus;
using VacationSystem.Application.Features.Employee.RequestVacation;

namespace CommonTestUtilities.Commands;

public class RequestVacationCommandBuilder
{
    public static RequestVacationCommand Build(DateTime? startDate = default, int? days = 0)
    {
        if(!days.HasValue)
            days = new Faker().Random.Int(15, 30);

        var faker = new Faker();

        return new(
            startDate.Value,
            days.Value
        );
    }
}
