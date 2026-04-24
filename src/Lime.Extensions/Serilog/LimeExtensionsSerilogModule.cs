using Microsoft.AspNetCore.Builder;
using Serilog;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;

namespace Lime.Extensions.Serilog;

/// <summary>
/// Lime Serilog 扩展模块
/// </summary>
[DependsOn(typeof(AbpAspNetCoreSerilogModule))]
public class LimeExtensionsSerilogModule : AbpModule
{
    /// <summary>
    /// 应用初始化时配置 Serilog 请求日志
    /// </summary>
    /// <param name="context">应用初始化上下文</param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context.GetApplicationBuilder().UseAbpSerilogEnrichers().UseSerilogRequestLogging();
    }
}
