#region License

// Postmark
// http://postmarkapp.com
// (c) 2010 Wildbit
// 
// Postmark.NET
// http://github.com/lunarbits/postmark-dotnet
// 
// The MIT License
// 
// Copyright (c) 2010 Daniel Crenna
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Json.NET 
// http://codeplex.com/json
//  
// Copyright (c) 2007 James Newton-King
// 
// The MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Hammock for REST
// http://hammock.codeplex.com
// 
// The MIT License
// 
// Copyright (c) 2010 Daniel Crenna and Jason Diller
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using PostmarkDotNet.Validation;

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
                                                                         "ps",
                                                                         "eps",
                                                                         "log"
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
            if (message.From != null) {
                if (!string.IsNullOrEmpty (message.From.DisplayName))
                    From = string.Format ("{0} <{1}>", message.From.DisplayName, message.From.Address);
                else
                    From = message.From.Address;
            }
            To = message.To.Count > 0 ? message.To[0].Address : null;
            Subject = message.Subject;
            HtmlBody = message.IsBodyHtml ? message.Body : null;
            TextBody = message.IsBodyHtml ? null : message.Body;
            if (message.ReplyTo != null) {
                if (!string.IsNullOrEmpty (message.ReplyTo.DisplayName))
                    ReplyTo = string.Format ("{0} <{1}>", message.ReplyTo.DisplayName, message.ReplyTo.Address);
                else
                    ReplyTo = message.ReplyTo.Address;
            }
            var header = message.Headers.Get ("X-PostmarkTag");
            if (header != null) {
                Tag = (string) header;
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

            //Image files: gif, jpg, jpeg, png, swf, flv, avi, mpg, mp3, rm, mov, psd, ai, tif, tiff
            //Documents: txt, rtf, htm, html, pdf, doc, docx, ppt, pptx, xls, xlsx, ps, eps
            //Miscellaneous: log

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