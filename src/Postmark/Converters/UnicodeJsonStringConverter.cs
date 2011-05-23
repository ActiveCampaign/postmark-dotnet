using System;
using System.Text;
using Newtonsoft.Json;

namespace PostmarkDotNet.Converters
{
    internal class UnicodeJsonStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var buffer = new StringBuilder();
            buffer.Append("\"");
            var stringValue = (string) value;
            foreach (var c in stringValue)
            {
                var code = (int) c;
                switch (c)
                {
                    case '\"':
                        buffer.Append("\\\"");
                        break;
                    case '\\':
                        buffer.Append("\\\\");
                        break;
                    case '\r':
                        buffer.Append("\\r");
                        break;
                    case '\n':
                        buffer.Append("\\n");
                        break;
                    case '\t':
                        buffer.Append("\\t");
                        break;
                    case '\b':
                        buffer.Append("\\b");
                        break;
                    case '/':
                        buffer.Append("\\/");
                        break;
                    case '\f':
                        buffer.Append("\\f");
                        break;
                    default:
                        if (code > 127)
                        {
                            buffer.AppendFormat("\\u{0:x4}", code);
                        }
                        else
                        {
                            buffer.Append(c);
                        }
                        break;
                }
            }
            buffer.Append("\"");

            writer.WriteRawValue(buffer.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Automatic conversion to string from other types
            if (reader.ValueType != objectType && objectType == typeof(string))
            {
                return reader.Value.ToString();
            }

            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (string);
        }
    }
}