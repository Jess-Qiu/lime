using Lime.Core;
using Lime.Extensions.Cache;
using Lime.Extensions.Mvc;
using Lime.Extensions.Serilog;
using Lime.Extensions.Swagger;
using Volo.Abp.Modularity;

namespace Lime.Extensions;

/// <summary>
///     Lime 扩展模块，整合 Serilog 日志和 Kestrel 端口配置
/// </summary>
[DependsOn(typeof(LimeCoreModule))]
[DependsOn(typeof(LimeExtensionsSerilogModule))]
[DependsOn(typeof(LimeExtensionsSwaggerModule))]
[DependsOn(typeof(LimeExtensionsMvcModule))]
[DependsOn(typeof(LimeExtensionsCacheModule))]
public class LimeExtensionsModule : AbpModule { }
