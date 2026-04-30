using Volo.Abp.MultiTenancy;

namespace Lime.Extensions.Tenant;

public static class TenantConfigurationExtensions
{
    /// <summary>
    ///     获取当前连接字符串
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentConnectionString(this TenantConfiguration tenantConfiguration)
    {
        return tenantConfiguration.ConnectionStrings.Default!;
    }

    /// <summary>
    ///     获取当前连接名
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentConnectionName(this TenantConfiguration tenantConfiguration)
    {
        return tenantConfiguration.Name;
    }
}
