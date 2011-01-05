using System;
using System.IO;
using System.Text;
using Hammock.Serialization;
using Newtonsoft.Json;

namespace PostmarkDotNet.Serializers
{
    internal class PostmarkSerializer : ISerializer, IDeserializer
    {
        private readonly JsonSerializer _serializer;

        public PostmarkSerializer(JsonSerializerSettings settings)
        {
            _serializer = new JsonSerializer
                              {
                                  ConstructorHandling = settings.ConstructorHandling,
                                  ContractResolver = settings.ContractResolver,
                                  ObjectCreationHandling = settings.ObjectCreationHandling,
                                  MissingMemberHandling = settings.MissingMemberHandling,
                                  DefaultValueHandling = settings.DefaultValueHandling,
                                  NullValueHandling = settings.NullValueHandling
                              };

            foreach (var converter in settings.Converters)
            {
                _serializer.Converters.Add(converter);
            }
        }

        public virtual object Deserialize(string content, Type type)
        {
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize(jsonTextReader, type);
                }
            }
        }

        public virtual T Deserialize<T>(string content)
        {
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public virtual string Serialize(object instance, Type type)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    _serializer.Serialize(jsonTextWriter, instance);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

        public virtual string ContentType
        {
            get { return "application/json"; }
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.UTF8; }
        }
    }
}