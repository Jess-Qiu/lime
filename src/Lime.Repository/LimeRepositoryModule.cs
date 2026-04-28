using Lime.Core;
using Volo.Abp.Modularity;

namespace Lime.Repository;

/// <summary>
/// Lime 仓储模块
/// </summary>
[DependsOn(typeof(LimeCoreModule))]
public class LimeRepositoryModule : AbpModule { }
