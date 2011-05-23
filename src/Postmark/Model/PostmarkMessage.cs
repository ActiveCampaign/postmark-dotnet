using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using PostmarkDotNet.Validation;

#if !WINDOWS_PHONE
using System.Collections.Specialized;
using System.Compat.Web;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
#else
using Hammock.Silverlight.Compat;
#endif

namespace PostmarkDotNet
{
    /// <summary>
    ///   A message destined for the Postmark service.
    /// </summary>
    public class PostmarkMessage
    {
        private static readonly ICollection<string> _whitelist = new List<string>
                                                                     {
                                                                         "gif",
                                                                         "jpg",
                                                                         "jpeg",
                                                                         "png",
                                                                         "swf",
                                                                         "flv",
                                                                         "avi",
                                                                         "mpg",
                                                                         "mp3",
                                                                         "rm",
                                                                         "mov",
                                                                         "psd",
                                                                         "ai",
                                                                         "tif",
                                                                         "tiff",
                                                                         "txt",
                                                                         "rtf",
                                                                         "htm",
                                                                         "html",
                                                                         "pdf",
                                                                         "doc",
                                                                         "docx",
                                                                         "ppt",
                                                                         "pptx",
                                                                         "xls",
                                                                         "xlsx",
                                                                         "ps",
                                                                         "eps",
                                                                         "log",
                                                                         "csv"
                                                                     };

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class.
        /// </summary>
        public PostmarkMessage()
        {
            Headers = new NameValueCollection(0);
            Attachments = new List<PostmarkMessageAttachment>(0);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class.
        /// </summary>
        /// <param name = "from">An email address for a sender.</param>
        /// <param name = "to">An email address for a recipient.</param>
        /// <param name = "subject">The message subject line.</param>
        /// <param name = "body">The message body.</param>
        public PostmarkMessage(string from, string to, string subject, string body) : this(from, to, subject, body, null)
        {

        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class.
        /// </summary>
        /// <param name = "from">An email address for a sender.</param>
        /// <param name = "to">An email address for a recipient.</param>
        /// <param name = "subject">The message subject line.</param>
        /// <param name = "body">The message body.</param>
        /// <param name = "headers">A collection of additional mail headers to send with the message.</param>
        public PostmarkMessage(string from, string to, string subject, string body, NameValueCollection headers)
        {
            var isHtml = !body.Equals(HttpUtility.HtmlEncode(body));

            From = from;
            To = to;
            Subject = subject;
            TextBody = isHtml ? null : body;
            HtmlBody = isHtml ? body : null;
            Headers = headers ?? new NameValueCollection(0);
        }


#if !WINDOWS_PHONE
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class
        ///   based on an existing <see cref = "MailMessage" /> instance. 
        /// 
        ///   Only the first recipient of the message is added to the <see cref = "PostmarkMessage" />
        ///   instance.
        /// </summary>
        /// <param name = "message">The existing message.</param>
        public PostmarkMessage(MailMessage message)
        {
            if (message.From != null)
                From = !string.IsNullOrEmpty(message.From.DisplayName)
                           ? string.Format("{0} <{1}>", message.From.DisplayName, message.From.Address)
                           : message.From.Address;
            
            GetMailMessageRecipients(message);
            
            Subject = message.Subject;
            HtmlBody = message.IsBodyHtml ? message.Body : null;
            TextBody = message.IsBodyHtml ? null : message.Body;

            GetHtmlBodyFromAlternateViews(message);
            
            if (message.ReplyTo != null)
            {
                ReplyTo = !string.IsNullOrEmpty(message.ReplyTo.DisplayName)
                              ? string.Format("{0} <{1}>", message.ReplyTo.DisplayName, message.ReplyTo.Address)
                              : message.ReplyTo.Address;
            }
            
            var header = message.Headers.Get("X-PostmarkTag");
            if (header != null)
            {
                Tag = header;
            }

            Headers = message.Headers;
            Attachments = new List<PostmarkMessageAttachment>(0);

            foreach (var item in from attachment in message.Attachments
                                 where attachment.ContentStream != null
                                 let content = ReadStream(attachment.ContentStream, 8192)
                                 select new PostmarkMessageAttachment
                                            {
                                                Name = attachment.Name,
                                                ContentType = attachment.ContentType.ToString(),
                                                Content = Convert.ToBase64String(content)
                                            })
            {
                Attachments.Add(item);
            }
        }

        private void GetMailMessageRecipients(MailMessage message)
        {
            GetMailMessageTo(message);
            GetMailMessageCc(message);
            GetMailMessageBcc(message);
        }

        private void GetMailMessageCc(MailMessage message)
        {
            var sb = new StringBuilder(0);
            
            if (message.CC.Count > 0)
            {
                foreach (var cc in message.CC)
                {
                    sb.AppendFormat("{0},", cc.Address);
                }
            }

            Cc = sb.ToString();
        }

        private void GetMailMessageBcc(MailMessage message)
        {
            var sb = new StringBuilder(0);
            
            if (message.Bcc.Count > 0)
            {
                foreach (var bcc in message.Bcc)
                {
                    sb.AppendFormat("{0},", bcc.Address);
                }
            }

            Bcc = sb.ToString();
        }

        private void GetMailMessageTo(MailMessage message)
        {
            var sb = new StringBuilder(0);
            foreach (var to in message.To)
            {
                if (!string.IsNullOrEmpty(to.DisplayName))
                {
                    sb.AppendFormat("{0} <{1}>,", to.DisplayName, to.Address);
                }
                else
                {
                    sb.AppendFormat("{0},", to.Address);
                }
            }
            To = sb.ToString();
        }

        // http://msdn.microsoft.com/en-us/library/system.net.mail.mailmessage.alternateviews.aspx

        private void GetHtmlBodyFromAlternateViews(MailMessage message)
        {
            if (message.AlternateViews.Count <= 0)
            {
                return;
            }

            var plainTextView = message.AlternateViews.Where(v => v.ContentType.MediaType.Equals(MediaTypeNames.Text.Plain)).FirstOrDefault();
            if (plainTextView != null)
            {
                TextBody = GetStringFromView(plainTextView);
            }

            var htmlView = message.AlternateViews.Where(v => v.ContentType.MediaType.Equals(MediaTypeNames.Text.Html)).FirstOrDefault();
            if (htmlView != null)
            {
                HtmlBody = GetStringFromView(htmlView);
            }
        }

        private static string GetStringFromView(AttachmentBase view)
        {
            var data = new byte[view.ContentStream.Length];
            view.ContentStream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data);
        }
#endif

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
        ///   The message body, if the message contains HTML.
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
        ///   A collection of optional message headers.
        /// </summary>
        public NameValueCollection Headers { get; set; }

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
        ///   Adds a file attachment.
        ///   Assumes the content type default of "application/octet-stream".
        /// </summary>
        /// <param name = "path">The full path to the file to attach</param>
        public void AddAttachment(string path)
        {
            AddAttachment(path, "application/octet-stream");
        }

        /// <summary>
        ///   Adds a file attachment.
        /// </summary>
        /// <param name = "path">The full path to the file to attach</param>
        /// <param name = "contentType">The content type.</param>
        public void AddAttachment(string path, string contentType)
        {
            ValidateAttachment(path);

            var stream = File.OpenRead(path);

            var content = ReadStream(stream, 8067);

            var attachment = new PostmarkMessageAttachment
                                 {
                                     Name = new FileInfo(path).Name,
                                     ContentType = contentType,
                                     Content = Convert.ToBase64String(content)
                                 };

            Attachments.Add(attachment);
        }

        private static void ValidateAttachment(string path)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Length > 10485760)
            {
                throw new ValidationException("Attachments must be less than 10MB in length.");
            }
            if (fileInfo.Length == 0)
            {
                throw new ValidationException("File path provided has no length.");
            }

            // Image files: gif, jpg, jpeg, png, swf, flv, avi, mpg, mp3, rm, mov, psd, ai, tif, tiff
            // Documents: txt, rtf, htm, html, pdf, doc, docx, ppt, pptx, xls, xlsx, ps, eps
            // Miscellaneous: log

            var extension = fileInfo.Extension.ToLowerInvariant().Substring(1);
            if (!_whitelist.Contains(extension))
            {
                throw new ValidationException(
                    "Attachments must have a whitelisted extension. The whitelist is available at: " +
                    "http://developer.postmarkapp.com/developer-build.html#attachments");
            }
        }
    }
}