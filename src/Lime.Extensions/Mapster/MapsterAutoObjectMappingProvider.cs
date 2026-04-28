using Mapster;
using Volo.Abp.ObjectMapping;

namespace Lime.Extensions.Mapster;

/// <summary>
///     Mapster 自动对象映射提供者，实现 ABP 的对象映射接口
/// </summary>
public class MapsterAutoObjectMappingProvider : IAutoObjectMappingProvider
{
    /// <summary>
    ///     将源对象映射到目标类型
    /// </summary>
    /// <typeparam name="TSource">源对象类型</typeparam>
    /// <typeparam name="TDestination">目标对象类型</typeparam>
    /// <param name="source">源对象</param>
    /// <returns>映射后的目标对象</returns>
    public TDestination Map<TSource, TDestination>(object source)
    {
        return source.Adapt<TDestination>();
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
        return source.Adapt(destination);
    }
}
