using Lime.Core;
using Lime.Extensions;
using Lime.Middlewares;
using Lime.Repository;
using Lime.Service;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
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
        var env = services.GetHostingEnvironment();
        ConfigureCors(services, env);
        ConfigureDistributedCache();
        ConfigureAuditing();
        ConfigureException();

        await base.ConfigureServicesAsync(context);
    }

    /// <summary>
    ///     配置 CORS 跨域选项
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="env">主机环境</param>
    private static void ConfigureCors(IServiceCollection services, IHostEnvironment env)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "LimeDefault",
                builder =>
                {
                    if (env.IsDevelopment())
                    {
                        // 开发环境：允许任意来源、方法和头部
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    }
                    else
                    {
                        // 生产环境：需要配置具体的允许来源
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(_ => true)
                            .AllowCredentials();
                    }
                }
            );
        });
    }

    /// <summary>
    ///     配置分布式缓存选项
    /// </summary>
    private void ConfigureDistributedCache()
    {
        Configure<AbpDistributedCacheOptions>(cacheOptions =>
        {
            // 设置缓存不过期，默认滑动 20 分钟
            cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = null;
            // 缓存 key 前缀
            cacheOptions.KeyPrefix = "Lime_";
        });
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
        app.UseCors("LimeDefault");
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
        await base.OnApplicationInitializationAsync(context);
    }
}
