using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Lime.Extensions.Kestrel;

/// <summary>
/// Kestrel 端口配置扩展
/// </summary>
public static class KestrelWebApplicationBuilderExtension
{
    /// <summary>
    /// 配置监听端口，优先级：命令行参数 > appsettings.json > 默认端口
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// <returns>WebApplicationBuilder</returns>
    public static WebApplicationBuilder ConfigureListeningPorts(this WebApplicationBuilder builder)
    {
        var port = builder.Configuration["port"];
        var urls = builder.Configuration["urls"];

        // 如果没有通过命令行参数指定，则从 appsettings.json 读取
        if (string.IsNullOrEmpty(urls) && string.IsNullOrEmpty(port))
        {
            var kestrelConfig = builder.Configuration.GetSection("Kestrel");
            if (!kestrelConfig.Exists())
            {
                // 默认配置：监听 5000 端口
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(5000);
                });
            }
            // 如果 appsettings.json 中有 Kestrel 配置，ASP.NET Core 会自动读取
        }
        else if (!string.IsNullOrEmpty(port) && int.TryParse(port, out var portNumber))
        {
            // 使用 --port 参数指定的端口
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(portNumber);
            });
        }
        // --urls 参数由 ASP.NET Core 自动处理，无需额外配置

        return builder;
    }
}
