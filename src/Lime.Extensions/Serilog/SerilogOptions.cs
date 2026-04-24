using Serilog.Events;

namespace Lime.Extensions.Serilog;

/// <summary>
/// Serilog 日志配置选项
/// </summary>
public class SerilogOptions
{
    /// <summary>
    /// 最小日志级别，默认 Debug（开发环境）或 Information（生产环境）
    /// </summary>
    public LogEventLevel? MinimumLevel { get; set; }

    /// <summary>
    /// 日志文件保留天数，默认 30 天
    /// </summary>
    public int RetainedFileCountLimit { get; set; } = 30;

    /// <summary>
    /// 日志目录，默认 logs
    /// </summary>
    public string LogsPath { get; set; } = "logs";

    /// <summary>
    /// 控制台输出模板
    /// </summary>
    public string ConsoleOutputTemplate { get; set; } =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 文件输出模板
    /// </summary>
    public string FileOutputTemplate { get; set; } =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
}
