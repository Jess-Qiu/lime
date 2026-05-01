using Lime.Extensions.Tenant;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Lime.Extensions.SqlSugar;

public abstract class LimeDbContext : ISqlSugarDbContext
{
    protected LimeDbContext(AbpLazyServiceProvider lazyServiceProvider)
    {
        LoggerFactory = lazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();
        CurrentTenantService = lazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();
        DataFilterService = lazyServiceProvider.LazyGetRequiredService<IDataFilter>();
        IsMultiTenantFilterEnabled = DataFilterService?.IsEnabled<IMultiTenant>() ?? false;
        IsSoftDeleteFilterEnabled = DataFilterService?.IsEnabled<ISoftDelete>() ?? false;
        DbConnOptions = lazyServiceProvider
            .LazyGetRequiredService<IOptions<LimeDbConnOptions>>()
            .Value;
        TenantConfigurationWrapper =
            lazyServiceProvider.LazyGetRequiredService<TenantConfigurationWrapper>();
    }

    public TenantConfigurationWrapper TenantConfigurationWrapper { get; set; }

    /// <summary>
    ///     数据库连接配置选项
    /// </summary>
    public LimeDbConnOptions DbConnOptions { get; set; }

    /// <summary>
    ///     日志工厂
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; }

    /// <summary>
    ///     当前租户服务
    /// </summary>
    protected ICurrentTenant CurrentTenantService { get; }

    /// <summary>
    ///     数据过滤服务
    /// </summary>
    protected IDataFilter DataFilterService { get; }

    /// <summary>
    ///     是否启用多租户过滤
    /// </summary>
    protected virtual bool IsMultiTenantFilterEnabled { get; }

    /// <summary>
    ///     是否启用软删除过滤
    /// </summary>
    protected virtual bool IsSoftDeleteFilterEnabled { get; }

    /// <summary>
    ///     SqlSugar 客户端实例
    /// </summary>
    public ISqlSugarClient SqlSugarClient { get; private set; }
}
