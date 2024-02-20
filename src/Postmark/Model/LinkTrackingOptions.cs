namespace PostmarkDotNet
{
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<PostmarkBounceType>))]
    public enum LinkTrackingOptions
    {
        None = 0,
        HtmlAndText = 1,
        HtmlOnly = 2,
        TextOnly = 3,
    }
}
