using Lime.Core;
using Lime.Extensions;
using Lime.Middlewares;
using Lime.Repository;
using Lime.Service;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Lime.Host;

/// <summary>
///     Lime 主机模块
/// </summary>
[DependsOn(
    typeof(LimeCoreModule),
    typeof(LimeExtensionsModule),
    typeof(LimeMiddlewaresModule),
    typeof(LimeRepositoryModule),
    typeof(LimeServiceModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class LimeHostModule : AbpModule
{
    /// <summary>
    ///     预配置服务，设置 API 控制器路由
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    public override async Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(
                typeof(LimeServiceModule).Assembly,
                opt => opt.RemoteServiceName = "default"
            );

            foreach (var i in options.ConventionalControllers.ConventionalControllerSettings)
                i.RootPath = "api";
        });

        await base.PreConfigureServicesAsync(context);
    }

    /// <summary>
    ///     配置服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }

    /// <summary>
    ///     应用初始化，配置中间件管道
    /// </summary>
    /// <param name="context">应用初始化上下文</param>
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
