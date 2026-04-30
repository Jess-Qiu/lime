using SqlSugar;

namespace Lime.Extensions.SqlSugar;

/// <summary>
///     Lime SqlSugar 配置选项
/// </summary>
public class LimeDbConnOptions
{
    /// <summary>
    ///     数据库类型，默认 MySQL
    ///     支持：MySql, SqlServer, PostgreSQL, Sqlite
    /// </summary>
    public string DbType { get; set; } = string.Empty;

    /// <summary>
    ///     数据库连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///     是否启用自动同步结构（开发环境使用），默认 false
    /// </summary>
    public bool AutoSyncStructure { get; set; }

    /// <summary>
    ///     是否启用 SQL 执行日志，默认 false
    /// </summary>
    public bool EnableSqlLog { get; set; }

    /// <summary>
    ///     解析数据库类型
    /// </summary>
    public DbType ParsedDbType =>
        DbType?.ToLower() switch
        {
            "mysql" => global::SqlSugar.DbType.MySql,
            "sqlserver" => global::SqlSugar.DbType.SqlServer,
            "postgresql" => global::SqlSugar.DbType.PostgreSQL,
            "sqlite" => global::SqlSugar.DbType.Sqlite,
            _ => global::SqlSugar.DbType.Sqlite,
        };
}
