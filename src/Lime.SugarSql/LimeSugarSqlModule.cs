using Lime.Core;
using Volo.Abp.Modularity;

namespace Lime.SugarSql;

/// <summary>
/// Lime SqlSugar ORM 模块
/// </summary>
[DependsOn(typeof(LimeCoreModule))]
public class LimeSugarSqlModule : AbpModule { }
