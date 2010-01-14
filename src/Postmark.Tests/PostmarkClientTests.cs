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
using System.Configuration;
using NUnit.Framework;
using PostmarkDotNet;
using PostmarkDotNet.Validation;

namespace Postmark.Tests
{
    [TestFixture]
    public class PostmarkClientTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            var settings = ConfigurationManager.AppSettings;
            Assert.IsNotNull(
                settings,
                "You must include an 'app.config' file in your unit test project. See 'app.config.example'."
                );

            _serverToken = settings["ServerToken"];
            _from = settings["From"];
            _to = settings["To"];
        }

        #endregion

        private const string _subject = "A test from Postmark.NET";
        private const string _textBody = "This is a test message!";
        private const string _invalidRecipient = "test@mctesterton.com";

        private static string _serverToken;
        private static string _from;
        private static string _to;

        [Test]
        public void Can_detect_html_in_message()
        {
            var message = new PostmarkMessage(_from, _to, _subject, "Have a <b>great</b> day!");

            Assert.IsNotNull(message);
            Assert.IsTrue(IsBodyHtml(message));
        }

        [Test]
        public void Can_detect_plain_text_message()
        {
            var message = new PostmarkMessage(_from, _to, _subject, "Have a great day!");

            Assert.IsNotNull(message);
            Assert.IsFalse(IsBodyHtml(message));
        }

        private static bool IsBodyHtml(PostmarkMessage message)
        {
            return !string.IsNullOrEmpty(message.HtmlBody);
        }

        [Test]
        [Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
                            {
                                To = _to,
                                From = _from, // This must be a verified sender signature
                                Subject = _subject,
                                TextBody = _textBody
                            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> " + response.Message);
        }

        [Test]
        [Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature_and_name_based_email()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = string.Format("The Team <{0}>", _from), // This must be a verified sender signature
                Subject = _subject,
                TextBody = _textBody
            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> " + response.Message);
        }

        [Test]
        [Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature_and_headers()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = _subject,
                TextBody = _textBody,
            };

            email.Headers.Add("X-Header-Test-1", "This is a header value");
            email.Headers.Add("X-Header-Test-2", "This is another header value");

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> " + response.Message);
        }

        [Test]
        [ExpectedException(typeof (ValidationException))]
        public void Can_send_message_with_token_and_signature_and_invalid_recipient_and_throw_validation_exception()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
                            {
                                To = "earth",
                                From = _from,
                                Subject = _subject,
                                TextBody = _textBody
                            };

            postmark.SendMessage(email);
        }

        [Test]
        public void Can_send_message_without_signature_and_receive_422()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
                            {
                                To = _invalidRecipient,
                                From = _invalidRecipient, // This must not be a verified sender signature
                                Subject = _subject,
                                TextBody = _textBody
                            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.UserError);

            Console.WriteLine("Postmark -> " + response.Message);
        }

        [Test]
        public void Can_send_message_without_token_and_receive_401()
        {
            var postmark = new PostmarkClient("");

            var email = new PostmarkMessage
                            {
                                To = _invalidRecipient,
                                From = _invalidRecipient,
                                Subject = _subject,
                                TextBody = _textBody
                            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.UserError);

            Console.WriteLine("Postmark -> " + response.Message);
        }
    }
}