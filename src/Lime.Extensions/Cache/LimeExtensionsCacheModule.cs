using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Lime.Extensions.Cache;

/// <summary>
/// Lime 缓存扩展模块，提供 Redis 分布式缓存支持
/// </summary>
[DependsOn(typeof(AbpCachingModule))]
public class LimeExtensionsCacheModule : AbpModule
{
    private const string ConfigurationSection = "Cache";

    /// <summary>
    /// 配置服务，根据配置启用 Redis 分布式缓存
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var options = GetCacheOptions(context.Configuration);

        if (options is { IsEnabled: true })
        {
            ConfigureRedisCache(context.Services, options);
        }

        await base.ConfigureServicesAsync(context);
    }

    /// <summary>
    /// 从配置中获取缓存选项
    /// </summary>
    private static LimeCacheOptions? GetCacheOptions(IConfiguration configuration)
    {
        var section = configuration.GetSection(ConfigurationSection);
        return section.Exists() ? section.Get<LimeCacheOptions>() : null;
    }

    /// <summary>
    /// 配置 Redis 分布式缓存
    /// </summary>
    private static void ConfigureRedisCache(IServiceCollection services, LimeCacheOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Configuration))
        {
            return;
        }

        var redisClient = new RedisClient(options.Configuration);
        services.AddSingleton<IRedisClient>(redisClient);
        services.Replace(
            ServiceDescriptor.Singleton<IDistributedCache>(new DistributedCache(redisClient))
        );
    }
}
