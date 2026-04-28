using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;

namespace Lime.Extensions.Mapster;

/// <summary>
///     Lime Mapster 扩展模块，配置 Mapster 作为对象映射提供者
/// </summary>
[DependsOn(typeof(AbpObjectMappingModule))]
public class LimeExtensionsMapsterModule : AbpModule
{
    /// <summary>
    ///     配置服务，注册 Mapster 映射器和扫描程序集配置
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());

        services.AddTransient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>();
        services.AddTransient<IObjectMapper, MapsterObjectMapper>();
    }
}
