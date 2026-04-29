using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lime.Core.Json;

public class DatetimeJsonConverter : JsonConverter<DateTime>
{
    private readonly string _dateFormat;

    /// <summary>
    ///     初始化DateTime转换器
    /// </summary>
    /// <param name="format">日期格式化字符串,默认为yyyy-MM-dd HH:mm:ss</param>
    public DatetimeJsonConverter(string format = "yyyy-MM-dd HH:mm:ss")
    {
        _dateFormat = format;
    }

    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String)
            return DateTime.TryParse(reader.GetString(), out var dateTime)
                ? dateTime
                : reader.GetDateTime();

        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_dateFormat));
    }
}
