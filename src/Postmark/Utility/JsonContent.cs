#if NETSTANDARD1_2
using Newtonsoft.Json;
using PostmarkDotNet.Converters;
using System;
using System.Net.Http;
using System.Text;

namespace PostmarkDotNet.Utility
{
    /// <summary>
    /// Creates a json content body suitable for posting to the API.
    /// </summary>
    internal class JsonContent<T> : StringContent
    {
        private static Lazy<JsonSerializerSettings> _settings = new Lazy<JsonSerializerSettings>(() =>
        {
            var retval = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include
            };

            retval.Converters.Add(new UnicodeJsonStringConverter());
            return retval;
        });

        internal JsonContent(T content) :
            base(JsonConvert.SerializeObject(content, Formatting.Indented, _settings.Value),
            Encoding.UTF8, "application/json")
        {
        }
    }
}
#endif