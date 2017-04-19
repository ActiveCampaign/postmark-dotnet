using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PostmarkDotNet
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LinkTrackingOptions
    {
        None = 0,
        HtmlAndText = 1,
        HtmlOnly = 2,
        TextOnly = 3,
    }
}
