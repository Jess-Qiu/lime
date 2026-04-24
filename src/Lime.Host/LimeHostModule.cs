using Lime.Core;
using Lime.Extensions;
using Lime.Middlewares;
using Lime.Repository;
using Lime.Service;
using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Lime.Host;

/// <summary>
/// Lime 主机模块
/// </summary>
[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(LimeCoreModule),
    typeof(LimeExtensionsModule),
    typeof(LimeMiddlewaresModule),
    typeof(LimeRepositoryModule),
    typeof(LimeServiceModule)
)]
public class LimeHostModule : AbpModule
{
    public override async Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(
                typeof(LimeHostModule).Assembly,
                opt => opt.RemoteServiceName = "default"
            );

            foreach (var i in options.ConventionalControllers.ConventionalControllerSettings)
                i.RootPath = "api";
        });

        await base.PreConfigureServicesAsync(context);
    }

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }

    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseRouting();

        app.UseConfiguredEndpoints();
        await base.OnApplicationInitializationAsync(context);
    }
}
