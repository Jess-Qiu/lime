using Lime.Core;
using Lime.Extensions.Serilog;
using Volo.Abp.Modularity;

namespace Lime.Extensions;

/// <summary>
/// Lime 扩展模块
/// </summary>
[DependsOn(typeof(LimeCoreModule))]
[DependsOn(typeof(LimeExtensionsSerilogModule))]
public class LimeExtensionsModule : AbpModule { }
