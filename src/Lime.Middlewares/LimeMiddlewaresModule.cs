using Lime.Core;
using Volo.Abp.Modularity;

namespace Lime.Middlewares;

/// <summary>
/// Lime 中间件模块
/// </summary>
[DependsOn(typeof(LimeCoreModule))]
public class LimeMiddlewaresModule : AbpModule { }
