using System.IO;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace PostmarkDotNet.Serializers
{
    internal class PostmarkSerializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        
        public PostmarkSerializer(JsonSerializerSettings settings) {
            _serializer = new Newtonsoft.Json.JsonSerializer
                              {
                                  ConstructorHandling = settings.ConstructorHandling,
                                  ContractResolver = settings.ContractResolver,
                                  ObjectCreationHandling = settings.ObjectCreationHandling,
                                  MissingMemberHandling = settings.MissingMemberHandling,
                                  DefaultValueHandling = settings.DefaultValueHandling,
                                  NullValueHandling = settings.NullValueHandling
                              };

            foreach(var converter in settings.Converters) {
                _serializer.Converters.Add(converter);
            }
        }

		public string Serialize(object obj) {
			using (var stringWriter = new StringWriter()) {
				using (var jsonTextWriter = new JsonTextWriter(stringWriter)) {
					jsonTextWriter.Formatting = Formatting.Indented;
					jsonTextWriter.QuoteChar = '"';

					_serializer.Serialize(jsonTextWriter, obj);

					var result = stringWriter.ToString();
					return result;
				}
			}
		}

		public string DateFormat { get; set; }
		public string RootElement { get; set; } 
		public string Namespace { get; set; }
    }
}
