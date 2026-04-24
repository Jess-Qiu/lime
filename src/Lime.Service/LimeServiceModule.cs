using Lime.Core;
using Lime.Repository;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Lime.Service;

/// <summary>
/// Lime 服务模块
/// </summary>
[DependsOn(typeof(LimeCoreModule), typeof(LimeRepositoryModule), typeof(AbpDddApplicationModule))]
public class LimeServiceModule : AbpModule { }
