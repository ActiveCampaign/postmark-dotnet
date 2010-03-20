#region License

// Postmark
// http://postmarkapp.com
// (c) 2010 Wildbit
// 
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
// 
// RestSharp
// http://github.com/johnsheehan/RestSharp 
// 
// Copyright (c) 2010 John Sheehan
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License. 

#endregion

using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Web;

namespace PostmarkDotNet
{
    /// <summary>
    /// A message destined for the Postmark service.
    /// </summary>
    public class PostmarkMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostmarkMessage"/> class.
        /// </summary>
        public PostmarkMessage()
        {
            Headers = new NameValueCollection(0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostmarkMessage"/> class.
        /// </summary>
        /// <param name="from">An email address for a sender.</param>
        /// <param name="to">An email address for a recipient.</param>
        /// <param name="subject">The message subject line.</param>
        /// <param name="body">The message body.</param>
        public PostmarkMessage(string from, string to, string subject, string body) : this(from, to, subject, body, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostmarkMessage"/> class.
        /// </summary>
        /// <param name="from">An email address for a sender.</param>
        /// <param name="to">An email address for a recipient.</param>
        /// <param name="subject">The message subject line.</param>
        /// <param name="body">The message body.</param>
        /// <param name="headers">A collection of additional mail headers to send with the message.</param>
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
        /// Initializes a new instance of the <see cref="PostmarkMessage"/> class
        /// based on an existing <see cref="MailMessage" /> instance. 
        /// 
        /// Only the first recipient of the message is added to the <see cref="PostmarkMessage" />
        /// instance.
        /// </summary>
        /// <param name="message">The existing message.</param>
        public PostmarkMessage(MailMessage message)
        {
            From = message.From.DisplayName;
            To = message.To.Count > 0 ? message.To[0].DisplayName : null;
            Subject = message.Subject;
            HtmlBody = message.IsBodyHtml ? message.Body : null;
            TextBody = message.IsBodyHtml ? null : message.Body;
            ReplyTo = message.ReplyTo.DisplayName;
            Headers = message.Headers;
        }

        /// <summary>
        /// The sender's email address.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The recipients's email address.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The email address to reply to. This is optional.
        /// </summary>
        public string ReplyTo { get; set; }

        /// <summary>
        /// The message subject line.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The message body, if the message contains HTML.
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// The message body, if the message is plain text.
        /// </summary>
        public string TextBody { get; set; }

        /// <summary>
        /// A collection of optional message headers.
        /// </summary>
        public NameValueCollection Headers { get; set; }
    }
}