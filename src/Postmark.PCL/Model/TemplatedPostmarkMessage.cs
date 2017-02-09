namespace PostmarkDotNet
{
    /// <summary>
    /// Send a message using a template that you have previously created in Postmark.
    /// </summary>
    public class TemplatedPostmarkMessage : PostmarkMessageBase
    {
        private bool _inlineCss = true;
        public TemplatedPostmarkMessage()
            : base()
        {

        }

        /// <summary>
        /// Should the CSS in the HtmlBody of the template be inlined? Defaults to true.
        /// </summary>
        public bool InlineCss { get { return _inlineCss; } set { _inlineCss = value; } }

        /// <summary>
        /// The template to use when sending this message.
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// The values to merge with the template when creating the content.
        ///
        /// The message sending API requires that this be an *object* as described by the JSON specification.
        ///
        /// This means that you can assign a Dictionary&lt;K,V&gt; to this property. 
        /// The dictionary may contain any keys and or objects that can be serialized to JSON.
        /// 
        /// Additionally, POCOs and anonymous types can be assigned to this property, 
        /// provided they can be serialized to JSON (we use JSON.net internally).
        ///
        /// Objects that would be serialized as "JSON scalars" or arrays should *not* be assigned to this property.
        /// (They MAY be set as values on the TemplateModel, though)
        /// 
        /// See this guide for more information on how this model is used, and how Postmark Templates work:
        /// http://support.postmarkapp.com/article/786-using-a-postmark-starter-template
        /// </summary>
        public object TemplateModel { get; set; }
    }
}
