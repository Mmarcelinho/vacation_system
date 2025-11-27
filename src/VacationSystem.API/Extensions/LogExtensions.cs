using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace VacationSystem.API.Extensions;

public static class LogExtensions
{
    public static void AddLog(
        this IServiceCollection services,
        IConfiguration configuration,
        string appName,
        string appVersion
    )
    {
        services.AddSerilog((_, options) =>
        {
            var elasticSearchUri = new Uri(configuration["ElasticSearch:Uri"]!);

            options.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticSearchUri!)
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{configuration["Elasticsearch:IndexFormatPrefix"]}-{{0:yyyy.MM.dd}}",
                NumberOfReplicas = 1,
                NumberOfShards = 2
            });

            #if DEBUG
            options.WriteTo.Console();
            options.MinimumLevel.Debug();
            #else
            options.WriteTo.Console(new CompactJsonFormatter());
            #endif

            options.Enrich.FromLogContext();
            options.Enrich.WithExceptionDetails();
        });

        services.AddOpenTelemetry(appName, appVersion);
    }

    public static void UseLog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }

    private static void AddOpenTelemetry(
        this IServiceCollection services,
        string appName,
        string appVersion
    )
    {
        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName, appVersion));
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddSqlClientInstrumentation();
                tracing.AddOtlpExporter();
                tracing.AddConsoleExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName, appVersion));

                metrics.AddRuntimeInstrumentation();
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();

                metrics.AddOtlpExporter();
                metrics.AddConsoleExporter();
            });
    }
}
