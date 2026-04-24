using Lime.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Lime.Extensions.Serilog;

/// <summary>
/// Serilog 日志配置扩展类
/// </summary>
public static class SerilogWebApplicationBuilderExtension
{
    private const string ConfigSectionName = "Serilog";

    /// <summary>
    /// 注册 Serilog 日志记录器
    /// </summary>
    /// <param name="builder">Web 应用构建器</param>
    /// <returns>Web 应用构建器</returns>
    public static WebApplicationBuilder RegisterSerilogLogger(this WebApplicationBuilder builder)
    {
        var options = GetOptions(builder.Configuration);

        Log.Logger = CreateLogger(builder.Environment, options);

        builder.Host.UseSerilog(Log.Logger);

        return builder;
    }

    /// <summary>
    /// 从配置文件读取 Serilog 选项
    /// </summary>
    /// <param name="configuration">配置对象</param>
    /// <returns>Serilog 配置选项</returns>
    private static SerilogOptions GetOptions(IConfiguration configuration)
    {
        var options = new SerilogOptions();
        configuration.GetSection(ConfigSectionName).Bind(options);
        return options;
    }

    /// <summary>
    /// 创建 Serilog 日志记录器
    /// </summary>
    /// <param name="environment">主机环境信息</param>
    /// <param name="options">Serilog 配置选项</param>
    /// <returns>配置好的日志记录器</returns>
    private static ILogger CreateLogger(IHostEnvironment environment, SerilogOptions options)
    {
        // 根据环境设置最小日志级别
        var minimumLevel =
            options.MinimumLevel
            ?? (environment.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information);

        var logsPath = Path.Combine(environment.ContentRootPath, options.LogsPath);

        return new LoggerConfiguration()
            .MinimumLevel.Is(minimumLevel)
            .MinimumLevel.Override(
                "Microsoft",
                environment.IsDevelopment() ? LogEventLevel.Information : LogEventLevel.Warning
            )
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .Enrich.WithProperty("Application", LimeOptions.ApplicationName)
            .WriteTo.Async(sink => sink.Console(outputTemplate: options.ConsoleOutputTemplate))
            .WriteTo.Async(sink =>
                sink.File(
                    path: Path.Combine(logsPath, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: options.FileOutputTemplate,
                    retainedFileCountLimit: options.RetainedFileCountLimit
                )
            )
            .WriteTo.Async(sink =>
                sink.File(
                    path: Path.Combine(logsPath, "error-.txt"),
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: options.FileOutputTemplate,
                    retainedFileCountLimit: options.RetainedFileCountLimit
                )
            )
            .CreateLogger();
    }
}
