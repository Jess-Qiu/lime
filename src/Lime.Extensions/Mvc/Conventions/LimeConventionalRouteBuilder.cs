using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Lime.Extensions.Mvc.Conventions;

/// <summary>
/// Lime 约定路由构建器，自定义 API 路由前缀
/// </summary>
public class LimeConventionalRouteBuilder : ConventionalRouteBuilder
{
    public LimeConventionalRouteBuilder(IOptions<AbpConventionalControllerOptions> options)
        : base(options) { }

    /// <summary>
    /// 获取 API 路由前缀，返回空字符串以移除默认的 app 前缀
    /// </summary>
    /// <param name="actionModel">操作模型</param>
    /// <param name="configuration">约定控制器配置</param>
    /// <returns>空字符串</returns>
    protected override string GetApiRoutePrefix(
        ActionModel actionModel,
        ConventionalControllerSetting? configuration
    )
    {
        return string.Empty;
    }
}
