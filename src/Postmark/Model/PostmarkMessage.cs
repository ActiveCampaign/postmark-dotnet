﻿using System;
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
using System.Diagnostics.CodeAnalysis;
#else
using Hammock.Silverlight.Compat;
#endif

namespace PostmarkDotNet
{
    /// <summary>
    ///   A message destined for the Postmark service.
    /// </summary>
    public partial class PostmarkMessage
    {
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
        public PostmarkMessage(string from, string to, string subject, string body)
            : this(from, to, subject, body, null)
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

            //Disabling "ReplyTo" obsolecense warning, as we need to continue to map this for users of the API that cannot upgrade."
#pragma warning disable 0618

            if (message.ReplyTo != null)
            {
                ReplyTo = !string.IsNullOrEmpty(message.ReplyTo.DisplayName)
                              ? string.Format("{0} <{1}>", message.ReplyTo.DisplayName, message.ReplyTo.Address)
                              : message.ReplyTo.Address;
            }
#pragma warning restore 0618
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
                                                Content = Convert.ToBase64String(content),
                                                ContentId = attachment.ContentId
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

            Encoding encoding = resolveViewEncoding(view, Encoding.ASCII);

            var data = new byte[view.ContentStream.Length];
            view.ContentStream.Read(data, 0, data.Length);
            return encoding.GetString(data);
        }

        private static Encoding resolveViewEncoding(AttachmentBase view, Encoding fallbackEncoding)
        {
            String charSet = view.ContentType.CharSet;
            try
            {
                return Encoding.GetEncoding(charSet);
            }
            catch
            {
                return fallbackEncoding;
            }
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
            ValidateAttachmentPath(path);

            using (var stream = File.OpenRead(path))
            {
                var content = ReadStream(stream, 8067);

                ValidateAttachmentLength(Convert.ToBase64String(content));

                var attachment = new PostmarkMessageAttachment
                {
                    Name = new FileInfo(path).Name,
                    ContentType = contentType,
#if !WINDOWS_PHONE
                    Content = Convert.ToBase64String(content, Base64FormattingOptions.InsertLineBreaks)
#else
                    Content = Convert.ToBase64String(content)
#endif
                };

                Attachments.Add(attachment);
            }
        }

        /// <summary>
        ///   Adds a file attachment with an inline image.
        /// </summary>
        /// <param name = "path">The full path to the file to attach</param>
        /// <param name = "contentType">The content type.</param>
        /// <param name = "contentId">ContentID for inline images</param>
        public void AddAttachment(string path, string contentType, string contentId)
        {
            ValidateAttachmentPath(path);

            using (var stream = File.OpenRead(path))
            {
                var content = ReadStream(stream, 8067);

                ValidateAttachmentLength(Convert.ToBase64String(content));

                var attachment = new PostmarkMessageAttachment
                {
                    Name = new FileInfo(path).Name,
                    ContentType = contentType,
#if !WINDOWS_PHONE
                    Content = Convert.ToBase64String(content, Base64FormattingOptions.InsertLineBreaks),
#else
                    Content = Convert.ToBase64String(content),
#endif
                    ContentId = contentId
                };

                Attachments.Add(attachment);
            }
        }

        /// <summary>
        ///   Adds a file attachment using a byte[] array
        /// </summary>
        /// <param name = "path">The full path to the file to attach</param>
        /// <param name = "contentType">ContentID for inline images.</param>
        public void AddAttachment(byte[] content, string contentType, string attachmentName)
        {
            ValidateAttachmentLength(Convert.ToBase64String(content));

            var attachment = new PostmarkMessageAttachment
            {
                Name = attachmentName,
                ContentType = contentType,
#if !WINDOWS_PHONE
                Content = Convert.ToBase64String(content, Base64FormattingOptions.InsertLineBreaks)
#else
                    Content = Convert.ToBase64String(content)
#endif
            };

            Attachments.Add(attachment);
        }

        /// <summary>
        ///   Adds a file attachment using a byte[] array with inline support
        /// </summary>
        /// <param name = "path">The full path to the file to attach</param>
        /// <param name = "contentType">The content type.</param>
        /// <param name = "contentId">The ContentId for inline images.</param>
        public void AddAttachment(byte[] content, string contentType, string attachmentName, string contentId)
        {
            ValidateAttachmentLength(Convert.ToBase64String(content));

            var attachment = new PostmarkMessageAttachment
            {
                Name = attachmentName,
                ContentType = contentType,
#if !WINDOWS_PHONE
                Content = Convert.ToBase64String(content, Base64FormattingOptions.InsertLineBreaks),
#else
                    Content = Convert.ToBase64String(content),
#endif
                ContentId = contentId
            };

            Attachments.Add(attachment);
        }
        /// <summary>
        /// Be sure the path to the attachment actually exists
        /// </summary>
        /// <param name="path"></param>
        private static void ValidateAttachmentPath(string path)
        {
            var fileInfo = new FileInfo(path);

            if (fileInfo.Length == 0)
            {
                throw new ValidationException("File path provided has no length.");
            }
        }

        /// <summary>
        /// Restrict attacments to 10 mb. The API will do this anyway but this is faster
        /// </summary>
        /// <param name="content"></param>
        private static void ValidateAttachmentLength(string content)
        {
            if (content.Length > 10485760)
            {
                throw new ValidationException("Attachments must be less than 10MB in length.");
            }
        }
    }
}