using System.Reflection;
using Carter;
using Microsoft.OpenApi.Models;
using VacationSystem.API.Extensions;
using VacationSystem.API.Infrastructure;
using VacationSystem.API.Token;
using VacationSystem.Application;
using VacationSystem.Application.Common.Security.Tokens;
using VacationSystem.Application.Infrastructure.Persistence.Extensions;

const string AUTHENTICATION_TYPE = "Bearer";

var assemblyName = Assembly.GetExecutingAssembly().GetName();
var appName = assemblyName.Name;
var appVersion = assemblyName.Version?.ToString();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "VSA Vacation System API", Version = "v1" });

    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTHENTICATION_TYPE
                },
                Scheme = "oauth2",
                Name = AUTHENTICATION_TYPE,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddCarter()
    .AddScoped<ITokenProvider, HttpContextTokenValue>()
    .AddHttpContextAccessor()
    .AddLog(builder.Configuration, appName!, appVersion!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseLog();

await UpdateDatabase();

app.Run();

async Task UpdateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();

    await MigrateExtension.MigrateDatabase(scope.ServiceProvider);
}