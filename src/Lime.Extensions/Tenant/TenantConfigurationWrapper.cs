using Lime.Extensions.SqlSugar;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Lime.Extensions.Tenant;

public class TenantConfigurationWrapper : ITransientDependency
{
    public TenantConfigurationWrapper(IAbpLazyServiceProvider lazyServiceProvider)
    {
        CurrentTenantService = lazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

        TenantStoreService = lazyServiceProvider.LazyGetRequiredService<ITenantStore>();

        DbConnectionOptions = lazyServiceProvider
            .LazyGetRequiredService<IOptions<LimeDbConnOptions>>()
            .Value;
    }

    public LimeDbConnOptions DbConnectionOptions { get; set; }

    public ITenantStore TenantStoreService { get; set; }

    public ICurrentTenant CurrentTenantService { get; set; }

    // /// <summary>
    // ///     获取当前连接字符串
    // /// </summary>
    // /// <returns></returns>
    // public async Task<string> GetCurrentConnectionStringAsync()
    // {
    //     return (await GetAsync()).ConnectionStrings.Default!;
    // }
    //
    // private async Task<TenantConfiguration?> GetAsync()
    // {
    //     if (!DbConnectionOptions.EnabledSaasMultiTenancy)
    //         return await TenantStoreService.FindAsync(
    //             ConnectionStrings.DefaultConnectionStringName
    //         );
    //
    //     return await GetTenantConfigurationByCurrentTenant();
    // }
    //
    // /// <summary>
    // ///     获取当前连接名
    // /// </summary>
    // /// <returns></returns>
    // public async Task<string> GetCurrentConnectionNameAsync()
    // {
    //     return (await GetAsync()).Name;
    // }
}
