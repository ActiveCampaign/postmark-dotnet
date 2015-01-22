using Newtonsoft.Json;
using PostmarkDotNet.Model;
using System;
using System.Linq;

namespace PostmarkDotNet.Converters
{
    internal class NameValuePair
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    internal class DictionaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is HeaderCollection))
            {
                return;
            }

            var collection = (HeaderCollection)value;
            var container = collection.Keys.Select(key => new
            {
                Name = key,
                Value = collection[key]
            }).ToList();

            var serialized = JsonConvert.SerializeObject(container);

            writer.WriteRawValue(serialized);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HeaderCollection);
        }

    }
}