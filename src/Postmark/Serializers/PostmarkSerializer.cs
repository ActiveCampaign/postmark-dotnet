using System;
#if NET40
using System.Dynamic;
#endif
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Hammock;
using Hammock.Serialization;

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

        public virtual object Deserialize(RestResponse response, Type type)
        {
            using (var stringReader = new StringReader(response.Content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize(jsonTextReader, type);
                }
            }
        }

        public virtual T Deserialize<T>(RestResponse<T> response)
        {
            using (var stringReader = new StringReader(response.Content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

#if NET40
        public dynamic DeserializeDynamic(RestResponse<dynamic> response)
        {
            throw new NotImplementedException();
        }
#endif

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