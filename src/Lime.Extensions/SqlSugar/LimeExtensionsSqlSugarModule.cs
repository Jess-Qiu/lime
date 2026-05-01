using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;

namespace Lime.Extensions.SqlSugar;

public class LimeExtensionsSqlSugarModule : AbpModule
{
    private const string ConfigurationSection = "Db";

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = GetSqlSugarOptions(services.GetConfiguration());

        if (configuration == null)
            return;

        ConfigureDbOptions(configuration);
        ConfigureDefaultTenant(configuration);
        ConfigureGuidGenerator(configuration!.ParsedDbType);

        await base.ConfigureServicesAsync(context);
    }

    private void ConfigureDbOptions(LimeDbConnOptions? dbConnOptions)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = dbConnOptions!.ConnectionString;
        });
    }

    private void ConfigureDefaultTenant(LimeDbConnOptions? dbConnOptions)
    {
        Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            var tenants = options.Tenants.ToList();

            // 规范化租户名称
            foreach (var tenant in tenants)
                tenant.NormalizedName = tenant.Name.Contains("@")
                    ? tenant.Name.Substring(0, tenant.Name.LastIndexOf("@"))
                    : tenant.Name;

            // 添加默认租户
            tenants.Insert(
                0,
                new TenantConfiguration
                {
                    Id = Guid.Empty,
                    Name = ConnectionStrings.DefaultConnectionStringName,
                    NormalizedName = ConnectionStrings.DefaultConnectionStringName,
                    ConnectionStrings = new ConnectionStrings
                    {
                        {
                            ConnectionStrings.DefaultConnectionStringName,
                            dbConnOptions!.ConnectionString
                        },
                    },
                    IsActive = true,
                }
            );

            options.Tenants = tenants.ToArray();
        });
    }

    private void ConfigureGuidGenerator(DbType parseDbType)
    {
        var guidType = parseDbType switch
        {
            DbType.MySql or DbType.PostgreSQL => SequentialGuidType.SequentialAsString,
            DbType.SqlServer => SequentialGuidType.SequentialAtEnd,
            DbType.Oracle => SequentialGuidType.SequentialAsBinary,
            _ => SequentialGuidType.SequentialAtEnd,
        };

        Configure<AbpSequentialGuidGeneratorOptions>(options =>
        {
            options.DefaultSequentialGuidType = guidType;
        });
    }

    private static LimeDbConnOptions? GetSqlSugarOptions(IConfiguration configuration)
    {
        var section = configuration.GetSection(ConfigurationSection);
        return section.Exists() ? section.Get<LimeDbConnOptions>() : null;
    }
}
