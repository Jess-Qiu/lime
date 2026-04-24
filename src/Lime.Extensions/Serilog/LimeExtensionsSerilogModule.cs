using Microsoft.AspNetCore.Builder;
using Serilog;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;

namespace Lime.Extensions.Serilog;

[DependsOn(typeof(AbpAspNetCoreSerilogModule))]
public class LimeExtensionsSerilogModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        context.GetApplicationBuilder().UseAbpSerilogEnrichers().UseSerilogRequestLogging();

        await base.OnApplicationInitializationAsync(context);
    }
}
