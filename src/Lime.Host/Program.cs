using Lime.Extensions.Serilog;
using Lime.Host;
using Serilog;

var builder = WebApplication.CreateBuilder(args).RegisterSerilogLogger();

try
{
    builder.Host.UseAutofac();
    await builder.Services.AddApplicationAsync<LimeHostModule>();
    var app = builder.Build();
    await app.InitializeApplicationAsync();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    await Log.CloseAndFlushAsync();
}
