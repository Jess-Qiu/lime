using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace Lime.Service.Test;

/// <summary>
/// 测试服务，用于验证 API 路由配置
/// </summary>
public class TestService : ApplicationService
{
    /// <summary>
    /// 默认 GET 接口
    /// </summary>
    /// <returns>Hello World 字符串</returns>
    public string Get() => "Hello World!";

    /// <summary>
    /// 自定义路由的 GET 接口
    /// </summary>
    /// <returns>Hello World 字符串</returns>
    [HttpGet("hello")]
    public string Hello() => "Hello World!";
}
