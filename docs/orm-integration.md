# ORM 集成开发计划

## Context

将 ORM（SqlSugar）集成到 Lime Framework，采用三层架构方案，与 Extensions 层设计理念保持一致。

## 架构设计

采用三层方案，职责分离：

| 层 | 内容 |
|---|------|
| **Lime.Core** | 实体基类、仓储接口 |
| **Lime.Extensions** | ORM 配置模块（LimeExtensionsOrmModule） |
| **Lime.Repository** | 仓储实现、数据库上下文 |

## 开发步骤

### 1. 添加 SqlSugar NuGet 包

**文件**: `Directory.Packages.props`

```xml
<PackageVersion Include="SqlSugarCore" Version="5.1.4.172" />
```

**文件**: `src/Lime.Extensions/Lime.Extensions.csproj`

```xml
<PackageReference Include="SqlSugarCore" />
```

**文件**: `src/Lime.Repository/Lime.Repository.csproj`

```xml
<PackageReference Include="SqlSugarCore" />
```

### 2. Core 层 - 创建实体基类和仓储接口

**文件**: `src/Lime.Core/Entities/Entity.cs`

```csharp
namespace Lime.Core.Entities;

/// <summary>
/// 实体基类
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// 主键 ID
    /// </summary>
    public long Id { get; set; }
}
```

**文件**: `src/Lime.Core/Repositories/IRepository.cs`

```csharp
namespace Lime.Core.Repositories;

/// <summary>
/// 泛型仓储接口
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(long id);
    Task<List<TEntity>> GetAllAsync();
    Task<int> InsertAsync(TEntity entity);
    Task<int> UpdateAsync(TEntity entity);
    Task<int> DeleteAsync(long id);
}
```

### 3. Extensions 层 - 创建 ORM 配置模块

**文件**: `src/Lime.Extensions/Orm/OrmOptions.cs`

```csharp
namespace Lime.Extensions.Orm;

/// <summary>
/// ORM 配置选项
/// </summary>
public class OrmOptions
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string DbType { get; set; } = "MySql";

    /// <summary>
    /// 是否开启自动关闭连接
    /// </summary>
    public bool IsAutoCloseConnection { get; set; } = true;

    /// <summary>
    /// 是否开启 SQL 日志
    /// </summary>
    public bool IsSqlLog { get; set; } = false;
}
```

**文件**: `src/Lime.Extensions/Orm/LimeExtensionsOrmModule.cs`

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Volo.Abp.Modularity;

namespace Lime.Extensions.Orm;

/// <summary>
/// Lime ORM 扩展模块，提供 SqlSugar 配置
/// </summary>
public class LimeExtensionsOrmModule : AbpModule
{
    private const string ConfigurationSection = "Orm";

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var options = configuration.GetSection(ConfigurationSection).Get<OrmOptions>();

        if (options != null)
        {
            context.Services.AddScoped<ISqlSugarClient>(sp =>
            {
                var db = new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = options.ConnectionString,
                    DbType = ParseDbType(options.DbType),
                    IsAutoCloseConnection = options.IsAutoCloseConnection
                });

                if (options.IsSqlLog)
                {
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Console.WriteLine($"[SQL] {sql}");
                    };
                }

                return db;
            });
        }

        await base.ConfigureServicesAsync(context);
    }

    private static DbType ParseDbType(string dbType) => dbType.ToLower() switch
    {
        "mysql" => SqlSugar.DbType.MySql,
        "sqlserver" => SqlSugar.DbType.SqlServer,
        "sqlite" => SqlSugar.DbType.Sqlite,
        "postgresql" => SqlSugar.DbType.PostgreSQL,
        "oracle" => SqlSugar.DbType.Oracle,
        _ => SqlSugar.DbType.MySql
    };
}
```

**文件**: `src/Lime.Extensions/LimeExtensionsModule.cs` (更新依赖)

```csharp
[DependsOn(typeof(LimeExtensionsOrmModule))]
```

### 4. Repository 层 - 创建仓储实现

**文件**: `src/Lime.Repository/Repositories/SugarRepository.cs`

```csharp
using Lime.Core.Repositories;
using SqlSugar;

namespace Lime.Repository.Repositories;

/// <summary>
/// SqlSugar 泛型仓储实现
/// </summary>
public class SugarRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, new()
{
    protected readonly ISqlSugarClient _db;

    public SugarRepository(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<TEntity?> GetByIdAsync(long id) =>
        await _db.Queryable<TEntity>().InSingleAsync(id);

    public async Task<List<TEntity>> GetAllAsync() =>
        await _db.Queryable<TEntity>().ToListAsync();

    public async Task<int> InsertAsync(TEntity entity) =>
        await _db.Insertable(entity).ExecuteCommandAsync();

    public async Task<int> UpdateAsync(TEntity entity) =>
        await _db.Updateable(entity).ExecuteCommandAsync();

    public async Task<int> DeleteAsync(long id) =>
        await _db.Deleteable<TEntity>().In(id).ExecuteCommandAsync();
}
```

### 5. 添加配置示例

**文件**: `src/Lime.Host/appsettings.json`

```json
{
  "Orm": {
    "ConnectionString": "Server=localhost;Database=lime;Uid=root;Pwd=123456;",
    "DbType": "MySql",
    "IsAutoCloseConnection": true,
    "IsSqlLog": true
  }
}
```

## 文件清单

| 文件路径 | 操作 |
|---------|------|
| `Directory.Packages.props` | 修改 - 添加 SqlSugarCore 包 |
| `src/Lime.Extensions/Lime.Extensions.csproj` | 修改 - 添加包引用 |
| `src/Lime.Repository/Lime.Repository.csproj` | 修改 - 添加包引用 |
| `src/Lime.Core/Entities/Entity.cs` | 新建 |
| `src/Lime.Core/Repositories/IRepository.cs` | 新建 |
| `src/Lime.Extensions/Orm/OrmOptions.cs` | 新建 |
| `src/Lime.Extensions/Orm/LimeExtensionsOrmModule.cs` | 新建 |
| `src/Lime.Extensions/LimeExtensionsModule.cs` | 修改 - 添加依赖 |
| `src/Lime.Repository/Repositories/SugarRepository.cs` | 新建 |
| `src/Lime.Host/appsettings.json` | 修改 - 添加配置节点 |

## 验证方式

1. 运行 `dotnet build` 确保编译通过
2. 运行 `csharpier format .` 格式化代码
3. 创建测试实体验证数据库连接

## 架构优势

- **接口与实现分离**：Core 定义接口，Repository 实现接口
- **配置集中管理**：ORM 配置在 Extensions 层，与 Cache、Mapster 风格一致
- **依赖倒置**：Service 层只依赖 Core 的接口，便于测试和替换实现
