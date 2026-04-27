namespace Lime.Extensions.Cache;

/// <summary>
/// Lime 缓存配置选项
/// </summary>
public class LimeCacheOptions
{
    /// <summary>
    /// 是否启用分布式缓存，默认为 false
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Redis 连接字符串
    /// </summary>
    public string Configuration { get; set; } = string.Empty;
}
