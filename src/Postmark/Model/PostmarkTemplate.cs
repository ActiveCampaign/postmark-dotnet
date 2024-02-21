namespace PostmarkDotNet.Model
{
    public class PostmarkTemplate
    {
        public long TemplateId { get; set; }

        public string Alias { get; set; }

        public string Subject { get; set; }

        public string Name { get; set; }

        public string HtmlBody { get; set; }

        public string TextBody { get; set; }

        public int AssociatedServerId { get; set; }

        public bool Active { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<TemplateType>))]
        public TemplateType TemplateType { get; set; }

        public string LayoutTemplate { get; set; }
    }
}
