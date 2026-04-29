using SqlSugar;

namespace Lime.Extensions.SqlSugar;

public interface ISqlSugarDbContext
{
    /// <summary>
    ///     获取SqlSugar客户端实例
    /// </summary>
    ISqlSugarClient SqlSugarClient { get; }
}
