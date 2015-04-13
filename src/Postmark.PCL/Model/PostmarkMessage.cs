using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PostmarkDotNet
{
    /// <summary>
    ///   A message destined for the Postmark service.
    /// </summary>
    public class PostmarkMessage
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class.
        /// </summary>
        public PostmarkMessage()
        {
            Headers = new HeaderCollection();
            Attachments = new List<PostmarkMessageAttachment>(0);
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
        ///   The sender's email address.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///   Any recipients. Separate multiple recipients with a comma.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        ///   Any CC recipients. Separate multiple recipients with a comma.
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        ///   Any BCC recipients. Separate multiple recipients with a comma.
        /// </summary>
        public string Bcc { get; set; }

        /// <summary>
        ///   The email address to reply to. This is optional.
        /// </summary>
        public string ReplyTo { get; set; }

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

        /// <summary>
        ///   An optional message tag, that is used for breaking down
        ///   statistics using the Postmark web administration UI.
        ///   For example, you can break email down into application specific
        ///   areas like "Invitation", or "BillingReminder".
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Track this message using Postmark's OpenTracking feature. Message should be in html
        /// to allow for tracking techniques
        /// </summary>
        public bool TrackOpens { get; set; }

        /// <summary>
        ///   A collection of optional message headers.
        /// </summary>
        public HeaderCollection Headers { get; set; }

        /// <summary>
        ///   A collection of optional file attachments.
        /// </summary>
        public ICollection<PostmarkMessageAttachment> Attachments { get; set; }

        private static byte[] ReadStream(Stream input, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        ///  Adds a file attachment stream with inline support.
        /// </summary>
        /// <param name = "contentStream">An opened stream of the file attachments.</param>
        /// <param name="attachmentName">The file name assocatiated with the attachment.</param>
        /// <param name = "contentType">The content type.</param>
        /// <param name="contentId">The ContentId for inlined images.</param>
        public void AddAttachment(Stream contentStream, string attachmentName, string contentType = "application/octet-stream", string contentId = null)
        {

            var content = ReadStream(contentStream, 8067);
            var payload = Convert.ToBase64String(content);


            var attachment = new PostmarkMessageAttachment
            {
                Name = attachmentName,
                ContentType = contentType,
                //TODO: split on 76 character bounds.
                Content = payload
            };

            if (!String.IsNullOrWhiteSpace(contentId))
            {
                attachment.ContentId = contentId;
            }

            Attachments.Add(attachment);
        }

        /// <summary>
        ///  Adds a file attachment using a byte[] array with inline support
        /// </summary>
        /// <param name="content"> The file contents</param>
        /// <param name="attachmentName">The file name of the attachment</param>
        /// <param name = "contentType">The content type.</param>
        /// <param name = "contentId">The ContentId for inline images.</param>
        public void AddAttachment(byte[] content, string attachmentName, string contentType = "application/octet-stream", string contentId = null)
        {
            using (var ms = new MemoryStream(content))
            {
                this.AddAttachment(ms, attachmentName, contentType, contentId);
            }
        }

    }
}