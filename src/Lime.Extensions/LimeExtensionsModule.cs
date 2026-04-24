using Lime.Core;
using Lime.Extensions.Kestrel;
using Lime.Extensions.Serilog;
using Volo.Abp.Modularity;

namespace Lime.Extensions;

/// <summary>
/// Lime 扩展模块，整合 Serilog 日志和 Kestrel 端口配置
/// </summary>
[DependsOn(typeof(LimeCoreModule))]
[DependsOn(typeof(LimeExtensionsSerilogModule))]
public class LimeExtensionsModule : AbpModule { }
