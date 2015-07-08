using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet.Model
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
        /// </summary>
        public object TemplateModel { get; set; }
    }
}
