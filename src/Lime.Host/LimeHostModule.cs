using Lime.Core;
using Lime.Extensions;
using Lime.Middlewares;
using Lime.Repository;
using Lime.Service;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
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
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAuditingModule)
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
        var services = context.Services;
        ConfigureAuditing();
        ConfigureException();
        await base.ConfigureServicesAsync(context);
    }

    /// <summary>
    ///     配置异常处理选项
    /// </summary>
    private void ConfigureException()
    {
        Configure<AbpExceptionHandlingOptions>(options =>
        {
            options.SendExceptionsDetailsToClients = true;
            options.SendStackTraceToClients = false;
        });
    }

    /// <summary>
    ///     配置审计选项
    /// </summary>
    private void ConfigureAuditing()
    {
        Configure<AbpAuditingOptions>(options => { });
        Configure<AbpAspNetCoreAuditingUrlOptions>(options =>
        {
            options.IncludeSchema = true;
            options.IncludeHost = true;
        });
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
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
        await base.OnApplicationInitializationAsync(context);
    }
}
