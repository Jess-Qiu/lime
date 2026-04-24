using Lime.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace Lime.Extensions.Swagger;

/// <summary>
///     Lime Swagger 扩展模块，配置 API 文档生成
/// </summary>
[DependsOn(typeof(AbpSwashbuckleModule))]
public class LimeExtensionsSwaggerModule : AbpModule
{
    /// <summary>
    ///     配置服务，注册 Swagger 生成器
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var mvcOptions = context
            .Services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>()
            .Configure();

        var settings = mvcOptions.ConventionalControllers.ConventionalControllerSettings;

        var services = context.Services;

        services.AddSwaggerGen(opt =>
        {
            ConfigureApiGroups(opt, settings);
            ConfigureApiFilter(opt, settings);
            IncludeXmlComments(opt);
        });
    }

    /// <summary>
    ///     配置 API 文档过滤器，根据控制器所属模块筛选文档
    /// </summary>
    /// <param name="opt">Swagger 生成选项</param>
    /// <param name="settings">约定控制器设置集合</param>
    private void ConfigureApiFilter(
        SwaggerGenOptions opt,
        IEnumerable<ConventionalControllerSetting> settings
    )
    {
        opt.DocInclusionPredicate(
            (docName, apiDesc) =>
            {
                if (apiDesc.ActionDescriptor is ControllerActionDescriptor controllerDesc)
                {
                    var matchedSetting = settings.FirstOrDefault(x =>
                        x.Assembly == controllerDesc.ControllerTypeInfo.Assembly
                    );
                    return matchedSetting?.RemoteServiceName == docName;
                }

                return false;
            }
        );
    }

    /// <summary>
    ///     配置 API 文档分组，为每个模块创建独立的 Swagger 文档
    /// </summary>
    /// <param name="opt">Swagger 生成选项</param>
    /// <param name="settings">约定控制器设置集合</param>
    private static void ConfigureApiGroups(
        SwaggerGenOptions opt,
        IEnumerable<ConventionalControllerSetting> settings
    )
    {
        foreach (var setting in settings.OrderBy(x => x.RemoteServiceName))
            if (!opt.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey(setting.RemoteServiceName))
                opt.SwaggerDoc(
                    setting.RemoteServiceName,
                    new OpenApiInfo { Title = InternalApp.ApplicationName, Version = "v1" }
                );
    }

    /// <summary>
    ///     包含 XML 注释文档，用于 API 描述信息展示
    /// </summary>
    /// <param name="opt">Swagger 生成选项</param>
    private void IncludeXmlComments(SwaggerGenOptions opt)
    {
        var fileNames = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");

        foreach (var fileName in fileNames)
            opt.IncludeXmlComments(fileName);
    }

    /// <summary>
    ///     应用初始化，启用 Swagger UI
    /// </summary>
    /// <param name="context">应用初始化上下文</param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        var mvcOptions = app
            .ApplicationServices.GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>()
            .Value;

        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            var settings = mvcOptions.ConventionalControllers.ConventionalControllerSettings;
            foreach (var setting in settings)
                opt.SwaggerEndpoint(
                    $"/swagger/{setting.RemoteServiceName}/swagger.json",
                    setting.RemoteServiceName
                );

            // 如果没有配置任何终结点，使用默认配置
            if (!settings.Any())
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Lime");
        });
    }
}
