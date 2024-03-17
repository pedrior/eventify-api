using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eventify.Presentation.Common.Json.Serialization;

internal sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type target, JsonSerializerOptions options)
    {
        return reader.TokenType is JsonTokenType.String
            ? DateTimeOffset.Parse(reader.GetString()!)
            : throw new JsonException("Expected string token");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) => 
        writer.WriteStringValue(value.ToString("O"));
}