using Lime.Extensions.Kestrel;
using Lime.Extensions.Serilog;
using Lime.Host;
using Serilog;

// 创建 Web 应用构建器，注册 Serilog 日志和配置监听端口
var builder = WebApplication.CreateBuilder(args).RegisterSerilogLogger().ConfigureListeningPorts();

try
{
    // 配置 Autofac 容器
    builder.Host.UseAutofac();

    // 添加 ABP 应用模块
    await builder.Services.AddApplicationAsync<LimeHostModule>();

    // 构建应用
    var app = builder.Build();

    // 初始化 ABP 应用
    await app.InitializeApplicationAsync();

    // 运行应用
    await app.RunAsync();
}
catch (Exception ex)
{
    // 记录未处理的异常
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    // 关闭并刷新日志
    await Log.CloseAndFlushAsync();
}
