using PostmarkDotNet;
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public static class SystemMailExtensions
    {
        /// <summary>
        /// Send a System.Net.MailMessage (transparently converts to the PostmarkMessage).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<PostmarkResponse> SendMessageAsync
            (this PostmarkClient client, MailMessage message)
        {
            return await client.SendMessageAsync(ConvertSystemMailMessage(message));
        }

        /// <summary>
        /// Send a System.Net.MailMessage(s) (transparently converts to the PostmarkMessages).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<PostmarkResponse>> SendMessagesAsync
            (this PostmarkClient client, params MailMessage[] messages)
        {
            return await client.SendMessagesAsync(messages.Select(ConvertSystemMailMessage));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkMessage" /> class
        ///   based on an existing <see cref = "MailMessage" /> instance. 
        /// 
        ///   Only the first recipient of the message is added to the <see cref = "PostmarkMessage" />
        ///   instance.
        /// </summary>
        /// <param name = "message">The existing message.</param>
        private static PostmarkMessage ConvertSystemMailMessage(MailMessage message)
        {
            var pm = new PostmarkMessage();

            if (message.From != null)
                pm.From = !string.IsNullOrEmpty(message.From.DisplayName)
                           ? string.Format("{0} <{1}>", message.From.DisplayName, message.From.Address)
                           : message.From.Address;

            GetMailMessageRecipients(pm, message);

            pm.Subject = message.Subject;
            pm.HtmlBody = message.IsBodyHtml ? message.Body : null;
            pm.TextBody = message.IsBodyHtml ? null : message.Body;

            GetHtmlBodyFromAlternateViews(pm, message);

            //Disabling "ReplyTo" obsolecense warning, as we need to continue to map this for users of the API that cannot upgrade."
#pragma warning disable 0618

            if (message.ReplyTo != null)
            {
                pm.ReplyTo = !string.IsNullOrEmpty(message.ReplyTo.DisplayName)
                              ? string.Format("{0} <{1}>", message.ReplyTo.DisplayName, message.ReplyTo.Address)
                              : message.ReplyTo.Address;
            }
#pragma warning restore 0618
            var header = message.Headers.Get("X-PostmarkTag");
            if (header != null)
            {
                pm.Tag = header;
            }

            pm.Headers = new HeaderCollection();

            if (message.Headers != null)
            {
                foreach (var h in message.Headers.AllKeys)
                {
                    pm.Headers.Add(h, message.Headers[h]);
                }
            }

            pm.Attachments = new List<PostmarkMessageAttachment>(0);

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
                pm.Attachments.Add(item);
            }
            return pm;
        }

        private static void GetMailMessageRecipients(PostmarkMessage m, MailMessage message)
        {
            GetMailMessageTo(m, message);
            GetMailMessageCc(m, message);
            GetMailMessageBcc(m, message);
        }

        private static void GetMailMessageCc(PostmarkMessage m, MailMessage message)
        {
            var sb = new StringBuilder(0);

            if (message.CC.Count > 0)
            {
                foreach (var cc in message.CC)
                {
                    sb.AppendFormat("{0},", cc.Address);
                }
            }

            m.Cc = sb.ToString();
        }

        private static void GetMailMessageBcc(PostmarkMessage m, MailMessage message)
        {
            var sb = new StringBuilder(0);

            if (message.Bcc.Count > 0)
            {
                foreach (var bcc in message.Bcc)
                {
                    sb.AppendFormat("{0},", bcc.Address);
                }
            }

            m.Bcc = sb.ToString();
        }

        private static void GetMailMessageTo(PostmarkMessage m, MailMessage message)
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
            m.To = sb.ToString();
        }

        private static void GetHtmlBodyFromAlternateViews(PostmarkMessage m, MailMessage message)
        {
            if (message.AlternateViews.Count <= 0)
            {
                return;
            }

            var plainTextView = message.AlternateViews.Where(v => v.ContentType.MediaType.Equals(MediaTypeNames.Text.Plain)).FirstOrDefault();
            if (plainTextView != null)
            {
                m.TextBody = GetStringFromView(plainTextView);
            }

            var htmlView = message.AlternateViews.Where(v => v.ContentType.MediaType.Equals(MediaTypeNames.Text.Html)).FirstOrDefault();
            if (htmlView != null)
            {
                m.HtmlBody = GetStringFromView(htmlView);
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
    }
}
