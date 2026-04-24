using Lime.Core;
using Lime.SugarSql;
using Volo.Abp.Modularity;

namespace Lime.Repository;

/// <summary>
/// Lime 仓储模块
/// </summary>
[DependsOn(typeof(LimeCoreModule), typeof(LimeSugarSqlModule))]
public class LimeRepositoryModule : AbpModule { }
