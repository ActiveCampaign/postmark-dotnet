namespace PostmarkDotNet
{
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<LinkTrackingOptions>))]
    public enum LinkTrackingOptions
    {
        None = 0,
        HtmlAndText = 1,
        HtmlOnly = 2,
        TextOnly = 3,
    }
}
