using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public class PostmarkMessage : PostmarkMessageBase
    {

        public PostmarkMessage()
            : base()
        {

        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class.
        /// </summary>
        /// <param name = "from">An email address for a sender.</param>
        /// <param name = "to">An email address for a recipient.</param>
        /// <param name = "subject">The message subject line.</param>
        /// <param name = "body">The message body.</param>
        /// <param name = "headers" oo>A collection of additional mail headers to send with the message. (optional)</param>
        public PostmarkMessage(string from, string to, string subject, string body, IDictionary<string, string> headers = null)
            : base()
        {
            var isHtml = !body.Equals(WebUtility.HtmlEncode(body));
            From = from;
            To = to;
            Subject = subject;
            TextBody = isHtml ? null : body;
            HtmlBody = isHtml ? body : null;
            Headers = new HeaderCollection(headers ?? new Dictionary<string, string>());
        }

        /// <summary>
        ///   The message subject line.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///   The message body, if the message contains
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        ///   The message body, if the message is plain text.
        /// </summary>
        public string TextBody { get; set; }
    }
}
