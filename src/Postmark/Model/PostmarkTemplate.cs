namespace PostmarkDotNet.Model
{
    public class PostmarkTemplate
    {
        public long TemplateId { get; set; }

        public string Subject { get; set; }

        public string Name { get; set; }

        public string HtmlBody { get; set; }

        public string TextBody { get; set; }

        public int AssociatedServerId { get; set; }

        public bool Active { get; set; }

    }
}
