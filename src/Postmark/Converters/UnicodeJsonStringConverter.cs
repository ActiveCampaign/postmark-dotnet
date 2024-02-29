using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PostmarkDotNet.Converters
{
    public class UnicodeJsonStringConverter : JsonConverter<string>
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("\"");
            
            foreach (char c in value)
            {
                int num = c;
                switch (c)
                {
                    case '"':
                        stringBuilder.Append("\\\"");
                        continue;
                    case '\\':
                        stringBuilder.Append("\\\\");
                        continue;
                    case '\r':
                        stringBuilder.Append("\\r");
                        continue;
                    case '\n':
                        stringBuilder.Append("\\n");
                        continue;
                    case '\t':
                        stringBuilder.Append("\\t");
                        continue;
                    case '\b':
                        stringBuilder.Append("\\b");
                        continue;
                    case '/':
                        stringBuilder.Append("\\/");
                        continue;
                    case '\f':
                        stringBuilder.Append("\\f");
                        continue;
                }

                if (num < 32 || num > 127)
                {
                    stringBuilder.AppendFormat("\\u{0:x4}", num);
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            stringBuilder.Append("\"");

            writer.WriteRawValue(stringBuilder.ToString());
        }
    }
}