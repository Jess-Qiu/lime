using Lime.Extensions.Mvc.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.Modularity;

namespace Lime.Extensions.Mvc;

/// <summary>
///     Lime MVC 扩展模块，自定义约定路由和服务约定
/// </summary>
public class LimeExtensionsMvcModule : AbpModule
{
    /// <summary>
    ///     配置服务，替换默认的路由构建器和服务约定
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Transient<IConventionalRouteBuilder, LimeConventionalRouteBuilder>()
        );
        context.Services.Replace(
            ServiceDescriptor.Transient<IAbpServiceConvention, LimeServiceConvention>()
        );
    }
}
