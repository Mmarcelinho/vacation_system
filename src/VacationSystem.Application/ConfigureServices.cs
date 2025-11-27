using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationSystem.Application.Common.Behaviors;
using VacationSystem.Application.Common.Configuration;
using VacationSystem.Application.Common.Interfaces;
using VacationSystem.Application.Common.Security.Cryptography;
using VacationSystem.Application.Common.Security.Tokens;
using VacationSystem.Application.Features.VacationRequests.Services;
using VacationSystem.Application.Infrastructure.Persistence;
using VacationSystem.Application.Infrastructure.Security.Cryptography;
using VacationSystem.Application.Infrastructure.Security.Tokens.Generator;
using VacationSystem.Application.Infrastructure.Security.Validator;
using VacationSystem.Application.Infrastructure.Services.LoggedUser;
using VacationSystem.Application.Infrastructure.Services.Messaging;

namespace VacationSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.AddOpenBehavior(typeof(LoggingBehavior<,>));
            options.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            services.AddDbContext<VacationSystemDbContext>(options =>
                options.UseInMemoryDatabase("VacationSystem"));
        else
        {
            var connectionString = configuration.GetConnectionString("Connection") ??
            throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<VacationSystemDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        AddPasswordEncrpter(services);
        AddTokens(services, configuration);
        AddLoggedUser(services);
        AddRabbitMQ(services, configuration);
        AddBackgroundService(services);

        return services;
    }

    private static void AddPasswordEncrpter(IServiceCollection services) => services.AddScoped<IPasswordEncripter, BCryptNet>();

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(_ => new JwtTokenValidator(signingKey!));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddRabbitMQ(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(
        configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IRabbitMQService, RabbitMQService>();
    }

    private static void AddBackgroundService(IServiceCollection services) =>
        services.AddHostedService<VacationNotificationService>();
}
