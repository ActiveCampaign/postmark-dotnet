using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PostmarkDotNet.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime);
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value.EndsWith(" (GMT)", StringComparison.Ordinal))
            {
                value = value.Substring(0, value.Length - " (GMT)".Length);
            }
            if (value.EndsWith(" (UTC)", StringComparison.Ordinal))
            {
                value = value.Substring(0, value.Length - " (UTC)".Length);
            }
            if (DateTime.TryParse(value, out var dateTime))
            {
                return dateTime;
            }

            // Fallback so we can at least get responses.
            return DateTime.UtcNow;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}
