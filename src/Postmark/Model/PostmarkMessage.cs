using PostmarkDotNet.Model;
using System.Collections.Generic;

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
        /// <param name="textBody">The Plain Text Body to be used for the message, this may be null if HtmlBody is set.</param>
        /// <param name="htmlBody">The HTML Body to be used for the message, this may be null if TextBody is set.</param>
        /// <param name = "headers">A collection of additional mail headers to send with the message. (optional)</param>
        /// <param name = "metadata">A dictionary of metadata to send with the message. (optional)</param>
        public PostmarkMessage(string from, string to, string subject, string textBody, string htmlBody, HeaderCollection headers = null, IDictionary<string, string> metadata = null)
            : base()
        {
            From = from;
            To = to;
            Subject = subject;
            TextBody = textBody;
            HtmlBody = htmlBody;
            Headers = headers ?? new HeaderCollection();
            Metadata = metadata;
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
