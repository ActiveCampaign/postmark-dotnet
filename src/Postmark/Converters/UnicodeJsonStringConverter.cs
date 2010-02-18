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
            var stringValue = (string)value;
            foreach (var c in stringValue)
            {
                var code = (int)c;
                if (c == '\"')
                {
                    buffer.Append("\\\"");
                }
                else if (code > 255)
                {
                    buffer.AppendFormat("\\u{0:x4}", code);
                }
                else
                {
                    buffer.Append(c);
                }
            }
            buffer.Append("\"");

            writer.WriteRawValue(buffer.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
