using System.Text.Json;
using System.Text.Json.Serialization;

namespace TavernHelios.AdministrationService.ClickHouse.Converters
{
    // Кастомный конвертер для DateTime
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("o")); // Формат ISO 8601
        }
    }
}
