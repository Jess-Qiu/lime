using Volo.Abp.ObjectMapping;

namespace Lime.Extensions.Mapster;

/// <summary>
///     Mapster 对象映射器实现，提供对象间的映射功能
/// </summary>
public class MapsterObjectMapper : IObjectMapper
{
    /// <summary>
    ///     初始化 MapsterObjectMapper 实例
    /// </summary>
    /// <param name="autoObjectMappingProvider">自动对象映射提供者</param>
    public MapsterObjectMapper(IAutoObjectMappingProvider autoObjectMappingProvider)
    {
        AutoObjectMappingProvider = autoObjectMappingProvider;
    }

    /// <summary>
    ///     将源对象映射到目标类型
    /// </summary>
    /// <typeparam name="TSource">源对象类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source">源对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return AutoObjectMappingProvider.Map<TSource, TDestination>(source);
    }

    /// <summary>
    ///     将源对象映射到已存在的目标对象
    /// </summary>
    /// <typeparam name="TSource">源对象类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source">源对象</param>
    /// <param name="destination">已存在的目标对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return AutoObjectMappingProvider.Map(source, destination);
    }

    /// <summary>
    ///     自动对象映射提供者
    /// </summary>
    public IAutoObjectMappingProvider AutoObjectMappingProvider { get; }
}
