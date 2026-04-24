using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Lime.Extensions.Serilog;

public static class SerilogWebApplicationBuilderExtension
{
    public static WebApplicationBuilder RegisterSerilogLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = CreateLogger(builder.Environment);

        builder.Host.UseSerilog(Log.Logger);

        return builder;
    }

    private static ILogger CreateLogger(IHostEnvironment environment)
    {
        // 根据环境设置最小日志级别
        var minimumLevel = environment.IsDevelopment()
            ? LogEventLevel.Debug
            : LogEventLevel.Information;

        var logsPath = Path.Combine(environment.ContentRootPath, "logs");

        return new LoggerConfiguration()
            .MinimumLevel.Is(minimumLevel)
            .MinimumLevel.Override(
                "Microsoft",
                environment.IsDevelopment() ? LogEventLevel.Information : LogEventLevel.Warning
            )
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .Enrich.WithProperty("Application", environment.ApplicationName)
            .WriteTo.Async(sink =>
                sink.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
            )
            .WriteTo.Async(sink =>
                sink.File(
                    path: Path.Combine(logsPath, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    retainedFileCountLimit: 30
                )
            )
            .WriteTo.Async(sink =>
                sink.File(
                    path: Path.Combine(logsPath, "error-.txt"),
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    retainedFileCountLimit: 30
                )
            )
            .CreateLogger();
    }
}
