using PostmarkDotNet.Model;
using PostmarkDotNet.Validation;
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


        //#if !WINDOWS_PHONE
        //        /// <summary>
        //        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class
        //        ///   based on an existing <see cref = "MailMessage" /> instance. 
        //        /// 
        //        ///   Only the first recipient of the message is added to the <see cref = "PostmarkMessage" />
        //        ///   instance.
        //        /// </summary>
        //        /// <param name = "message">The existing message.</param>
        //        public PostmarkMessage(MailMessage message)
        //        {
        //            if (message.From != null)
        //                From = !string.IsNullOrEmpty(message.From.DisplayName)
        //                           ? string.Format("{0} <{1}>", message.From.DisplayName, message.From.Address)
        //                           : message.From.Address;

        //            GetMailMessageRecipients(message);

        //            Subject = message.Subject;
        //            HtmlBody = message.IsBodyHtml ? message.Body : null;
        //            TextBody = message.IsBodyHtml ? null : message.Body;

        //            GetHtmlBodyFromAlternateViews(message);

        //            if (message.ReplyTo != null)
        //            {
        //                ReplyTo = !string.IsNullOrEmpty(message.ReplyTo.DisplayName)
        //                              ? string.Format("{0} <{1}>", message.ReplyTo.DisplayName, message.ReplyTo.Address)
        //                              : message.ReplyTo.Address;
        //            }

        //            var header = message.Headers.Get("X-PostmarkTag");
        //            if (header != null)
        //            {
        //                Tag = header;
        //            }

        //            Headers = message.Headers;
        //            Attachments = new List<PostmarkMessageAttachment>(0);

        //            foreach (var item in from attachment in message.Attachments
        //                                 where attachment.ContentStream != null
        //                                 let content = ReadStream(attachment.ContentStream, 8192)
        //                                 select new PostmarkMessageAttachment
        //                                            {
        //                                                Name = attachment.Name,
        //                                                ContentType = attachment.ContentType.ToString(),
        //                                                Content = Convert.ToBase64String(content)
        //                                            })
        //            {
        //                Attachments.Add(item);
        //            }
        //        }

        //        private void GetMailMessageRecipients(MailMessage message)
        //        {
        //            GetMailMessageTo(message);
        //            GetMailMessageCc(message);
        //            GetMailMessageBcc(message);
        //        }

        //        private void GetMailMessageCc(MailMessage message)
        //        {
        //            var sb = new StringBuilder(0);

        //            if (message.CC.Count > 0)
        //            {
        //                foreach (var cc in message.CC)
        //                {
        //                    sb.AppendFormat("{0},", cc.Address);
        //                }
        //            }

        //            Cc = sb.ToString();
        //        }

        //        private void GetMailMessageBcc(MailMessage message)
        //        {
        //            var sb = new StringBuilder(0);

        //            if (message.Bcc.Count > 0)
        //            {
        //                foreach (var bcc in message.Bcc)
        //                {
        //                    sb.AppendFormat("{0},", bcc.Address);
        //                }
        //            }

        //            Bcc = sb.ToString();
        //        }

        //        private void GetMailMessageTo(MailMessage message)
        //        {
        //            var sb = new StringBuilder(0);
        //            foreach (var to in message.To)
        //            {
        //                if (!string.IsNullOrEmpty(to.DisplayName))
        //                {
        //                    sb.AppendFormat("{0} <{1}>,", to.DisplayName, to.Address);
        //                }
        //                else
        //                {
        //                    sb.AppendFormat("{0},", to.Address);
        //                }
        //            }
        //            To = sb.ToString();
        //        }

        //        // http://msdn.microsoft.com/en-us/library/system.net.mail.mailmessage.alternateviews.aspx



        //        private void GetHtmlBodyFromAlternateViews(MailMessage message)
        //        {
        //            if (message.AlternateViews.Count <= 0)
        //            {
        //                return;
        //            }

        //            var plainTextView = message.AlternateViews.Where(v => v.ContentType.MediaType.Equals(MediaTypeNames.Text.Plain)).FirstOrDefault();
        //            if (plainTextView != null)
        //            {
        //                TextBody = GetStringFromView(plainTextView);
        //            }

        //            var htmlView = message.AlternateViews.Where(v => v.ContentType.MediaType.Equals(MediaTypeNames.Text.Html)).FirstOrDefault();
        //            if (htmlView != null)
        //            {
        //                HtmlBody = GetStringFromView(htmlView);
        //            }
        //        }

        //        private static string GetStringFromView(AttachmentBase view)
        //        {

        //          Encoding encoding = resolveViewEncoding(view, Encoding.ASCII);

        //          var data = new byte[view.ContentStream.Length];
        //          view.ContentStream.Read(data, 0, data.Length);
        //          return encoding.GetString(data);
        //        }

        //        private static Encoding resolveViewEncoding(AttachmentBase view, Encoding fallbackEncoding)
        //        {
        //          String charSet = view.ContentType.CharSet;
        //          try
        //          {
        //            return Encoding.GetEncoding(charSet);
        //          }
        //          catch
        //          {
        //            return fallbackEncoding;
        //          }
        //        }
        //#endif

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

            //NOTE: JP suggests that ALL validation of messages should be done on the server. For now, this check remains:
            var payload = Convert.ToBase64String(content);
            ValidateAttachmentLength(payload.Length);


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

        private void ValidateAttachmentLength(int length)
        {
            if (length > 10 * 1024 * 1024)
            {
                throw new ValidationException("Attachments must be less than 10MB in length.");
            }
        }
    }
}