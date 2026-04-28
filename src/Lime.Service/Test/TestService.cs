using Lime.Service.Test.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;

namespace Lime.Service.Test;

/// <summary>
///     测试服务，用于验证 API 路由配置
/// </summary>
public class TestService : ApplicationService
{
    private readonly IDistributedCache<TestItemDto> _distributedCache;

    public TestService(IDistributedCache<TestItemDto> distributedCache)
    {
        _distributedCache = distributedCache;
    }

    /// <summary>
    ///     默认 GET 接口
    /// </summary>
    /// <returns>Hello World 字符串</returns>
    public string Get()
    {
        return "Hello World!";
    }

    /// <summary>
    ///     自定义路由的 GET 接口
    /// </summary>
    /// <returns>Hello World 字符串</returns>
    [HttpGet("hello")]
    public string Hello()
    {
        return "Hello World!";
    }

    /// <summary>
    ///     抛出异常的测试接口
    /// </summary>
    /// <returns>不返回，始终抛出异常</returns>
    public string GetException()
    {
        throw new NotImplementedException("异常测试");
    }

    /// <summary>
    ///     设置缓存
    /// </summary>
    /// <param name="dto">要缓存的数据</param>
    public async Task SetCache(TestItemDto dto)
    {
        await _distributedCache.SetAsync(dto.Name, dto);
    }

    /// <summary>
    ///     获取缓存
    /// </summary>
    /// <param name="dto">包含要查询的键名</param>
    /// <returns>缓存的数据，如果不存在则返回 null</returns>
    public async Task<TestItemDto?> GetCache(TestItemDto dto)
    {
        return await _distributedCache.GetAsync(dto.Name);
    }

    /// <summary>
    ///     使用 Mapster 将 TestItemDto 映射为 MapItemDto
    /// </summary>
    /// <param name="dto">源数据传输对象</param>
    /// <returns>映射后的目标对象</returns>
    public MapItemDto MapToDestinationObject(TestItemDto dto)
    {
        return ObjectMapper.Map<TestItemDto, MapItemDto>(dto);
    }
}
