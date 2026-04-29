using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Lime.Extensions.SqlSugar;

public abstract class LimeSqlSugarContext : ISqlSugarDbContext, ISqlSugarDbContextDependencies
{
    protected LimeSqlSugarContext(AbpLazyServiceProvider lazyServiceProvider)
    {
        LazyServiceProvider = lazyServiceProvider;
        LoggerFactory = lazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();
        CurrentTenantService = lazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();
        DataFilterService = lazyServiceProvider.LazyGetRequiredService<IDataFilter>();
        IsMultiTenantFilterEnabled = DataFilterService?.IsEnabled<IMultiTenant>() ?? false;
        IsSoftDeleteFilterEnabled = DataFilterService?.IsEnabled<ISoftDelete>() ?? false;
        DbConnOptions = LazyServiceProvider
            .LazyGetRequiredService<IOptions<LimeSqlSugarOptions>>()
            .Value;
    }

    /// <summary>
    ///     数据库连接配置选项
    /// </summary>
    public LimeSqlSugarOptions DbConnOptions { get; set; }

    /// <summary>
    ///     服务提供者
    /// </summary>
    protected IAbpLazyServiceProvider LazyServiceProvider { get; }

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

    /// <summary>
    ///     执行顺序
    /// </summary>
    public int ExecutionOrder { get; }

    public void OnSqlSugarClientConfig(ISqlSugarClient sqlSugarClient)
    {
        SqlSugarClient = sqlSugarClient;
        CustomDataFilter(sqlSugarClient);
    }

    public void DataExecuted(object oldValue, DataAfterModel entityInfo)
    {
        // 配置软删除过滤器
        if (IsSoftDeleteFilterEnabled)
            SqlSugarClient.QueryFilter.AddTableFilter<ISoftDelete>(entity => !entity.IsDeleted);

        // 配置多租户过滤器
        if (IsMultiTenantFilterEnabled)
        {
            var currentTenantId = CurrentTenantService.Id;
            SqlSugarClient.QueryFilter.AddTableFilter<IMultiTenant>(entity =>
                entity.TenantId == currentTenantId
            );
        }
    }

    public void DataExecuting(object oldValue, DataFilterModel entityInfo) { }

    public void OnLogExecuting(string sql, SugarParameter[] parameters)
    {
        if (DbConnOptions.EnableSqlLog)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("==========Lime-SQL执行:==========");
            sb.AppendLine(UtilMethods.GetSqlString(DbConnOptions.ParsedDbType, sql, parameters));
            sb.AppendLine("===============================");
            LoggerFactory.CreateLogger(GetType()).LogDebug(sb.ToString());
        }
    }

    public void OnLogExecuted(string sql, SugarParameter[] parameters)
    {
        if (DbConnOptions.EnableSqlLog)
            LoggerFactory
                .CreateLogger(GetType())
                .LogDebug(
                    "=========Lime-SQL耗时 {0} 毫秒=====",
                    SqlSugarClient.Ado.SqlExecutionTime.TotalMilliseconds
                );
    }

    public void EntityService(PropertyInfo propertyInfo, EntityColumnInfo entityColumnInfo) { }

    protected virtual void CustomDataFilter(ISqlSugarClient sqlSugarClient) { }
}
