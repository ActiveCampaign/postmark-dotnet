using PostmarkDotNet.Converters;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PostmarkDotNet.Utility
{
    /// <summary>
    /// Creates a json content body suitable for posting to the API.
    /// </summary>
    internal class JsonContent<T> : StringContent
    {
        private static JsonSerializerOptions _settings = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new UnicodeJsonStringConverter() }
        };

        internal JsonContent(T content) :
            base(JsonSerializer.Serialize(content, _settings),
            Encoding.UTF8, "application/json")
        {
        }
    }
}