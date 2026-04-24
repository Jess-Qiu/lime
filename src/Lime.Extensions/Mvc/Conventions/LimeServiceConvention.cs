using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Lime.Extensions.Mvc.Conventions.Conventions;

/// <summary>
/// Lime 服务约定，自定义 API 路由模板格式
/// </summary>
public class LimeServiceConvention : AbpServiceConvention
{
    public LimeServiceConvention(
        IOptions<AbpAspNetCoreMvcOptions> options,
        IConventionalRouteBuilder conventionalRouteBuilder
    )
        : base(options, conventionalRouteBuilder) { }

    /// <summary>
    /// 配置选择器，为路由模板添加 api 前缀
    /// </summary>
    /// <param name="rootPath">根路径</param>
    /// <param name="controllerName">控制器名称</param>
    /// <param name="action">操作模型</param>
    /// <param name="configuration">约定控制器配置</param>
    protected override void ConfigureSelector(
        string rootPath,
        string controllerName,
        ActionModel action,
        ConventionalControllerSetting? configuration
    )
    {
        base.ConfigureSelector(rootPath, controllerName, action, configuration);

        var selectors = action.Selectors;

        foreach (var selector in selectors)
        {
            var template = selector.AttributeRouteModel?.Template;

            if (!string.IsNullOrEmpty(template) && !template.StartsWith("/api"))
                selector.AttributeRouteModel?.Template = $"api/{template}";
        }
    }
}
