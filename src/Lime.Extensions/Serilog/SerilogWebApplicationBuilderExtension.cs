using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Lime.Extensions.Serilog;

public static class SerilogWebApplicationBuilderExtension
{
    public static WebApplicationBuilder RegisterSerilogLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = CreateLogger(builder.Configuration);

        builder.Host.UseSerilog(Log.Logger);

        return builder;
    }

    private static ILogger CreateLogger(ConfigurationManager builderConfiguration)
    {
        var logger = new LoggerConfiguration();

        return logger.CreateLogger();
    }
}
