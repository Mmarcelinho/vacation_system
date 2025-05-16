using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VacationSystem.Application.Infrastructure.Persistence.Extensions;

public static class MigrateExtension
{
    public async static Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<VacationSystemDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}