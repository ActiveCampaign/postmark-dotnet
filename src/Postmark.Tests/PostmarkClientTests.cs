using Newtonsoft.Json;
using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace Postmark.Tests
{
    [TestFixture]
    public partial class PostmarkClientTests
    {

        /// <summary>
        /// Retrieve the config variable from the environment, 
        /// or app.config if the environment doesn't specify it.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get.</param>
        /// <returns></returns>
        private static string ConfigVariable(string variableName)
        {
            string retval = null;
            //this is here to allow us to have a config that isn't committed to source control, but still allows the project to build
            try
            {
                var json_parameters = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/../../../../testing_keys.json");

                var values = JsonConvert.DeserializeObject<Dictionary<String, String>>(json_parameters);

                retval = values[variableName];
            }
            catch
            {
                //This is OK, it just doesn't exist.. no big deal.
            }
            return String.IsNullOrWhiteSpace(retval) ? Environment.GetEnvironmentVariable(variableName) : retval;
        }


        [SetUp]
        public void SetUp()
        {
            _serverToken = ConfigVariable("WRITE_TEST_SERVER_TOKEN");
            Assert.IsNotNullOrEmpty(_serverToken);

            _from = ConfigVariable("WRITE_TEST_SENDER_EMAIL_ADDRESS");
            Assert.IsNotNullOrEmpty(_from);

            _to = ConfigVariable("WRITE_TEST_EMAIL_RECIPIENT_ADDRESS");
            Assert.IsNotNullOrEmpty(_to);
        }

        private const string Subject = "Postmark test";
        private const string HtmlBody = "<html><body><strong>Hello</strong> dear Postmark user.</body></html>";
        private const string TextBody = "Hello dear Postmark user.";
        private const string InvalidRecipient = "test@mctesterton.com";

        private static string _serverToken;
        private static string _from;
        private static string _to;

        [Test]
        public void Can_detect_html_in_message()
        {
            var message = new PostmarkMessage(_from, _to, Subject, "Have a <b>great</b> day!");

            Assert.IsNotNull(message);
            Assert.IsTrue(IsBodyHtml(message));
        }

        [Test]
        public void Can_detect_plain_text_message()
        {
            var message = new PostmarkMessage(_from, _to, Subject, "Have a great day!");

            Assert.IsNotNull(message);
            Assert.IsFalse(IsBodyHtml(message));
        }

        private static bool IsBodyHtml(PostmarkMessage message)
        {
            return !string.IsNullOrEmpty(message.HtmlBody);
        }

        [Test]
        //[Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> {0}", response.Message);
        }

        [Test]
        //[Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature_and_name_based_email()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = string.Format("The Team <{0}>", _from), // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> " + response.Message);
        }

        [Test]
        //[Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature_and_headers()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            email.Headers.Add("X-Header-Test-1", "This is a header value");
            email.Headers.Add("X-Header-Test-2", "This is another header value");

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);
            Assert.AreNotEqual(default(DateTime), response.SubmittedAt, "Missing submitted time value.");

            Console.WriteLine("Postmark -> {0}", response.Message);
        }

        [Test]
        //[Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature_and_headers_and_timeout()
        {
            var postmark = new PostmarkClient(_serverToken, 10);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            email.Headers.Add("X-Header-Test-1", "This is a header value");
            email.Headers.Add("X-Header-Test-2", "This is another header value");

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);
            Assert.AreNotEqual(default(DateTime), response.SubmittedAt, "Missing submitted time value.");

            Console.WriteLine("Postmark -> {0}", response.Message);
        }

        [Test]
        //[Ignore("This test sends a real email.")]
        public void Can_send_message_with_file_attachment()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            email.AddAttachment("logo.png", "image/png");

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);
        }

        [Test]
        public void Can_send_message_without_signature_and_receive_422()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = InvalidRecipient,
                From = InvalidRecipient, // This must not be a verified sender signature
                Subject = Subject,
                TextBody = TextBody
            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.UserError);
        }

        [Test]
        public void Can_send_message_without_token_and_receive_401()
        {
            var postmark = new PostmarkClient("");

            var email = new PostmarkMessage
            {
                To = InvalidRecipient,
                From = InvalidRecipient,
                Subject = Subject,
                TextBody = TextBody
            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.UserError);

            Console.WriteLine("Postmark -> {0}", response.Message);
        }

        [Test]
        public void Can_send_message_with_cc_and_bcc()
        {
            var postmark = new PostmarkClient("POSTMARK_API_TEST");

            var email = new PostmarkMessage
            {
                To = InvalidRecipient,
                Cc = "test-cc@example.com",
                Bcc = "test-bcc@example.com",
                From = InvalidRecipient,
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> {0}", response.Message);
        }

        [Test]
        public void Can_generate_postmarkmessage_from_mailmessage()
        {
            var mm = new MailMessage
            {
                Subject = "test",
                Body = "test"
            };
            mm.To.Add("me@me.com");
            mm.Bcc.Add("me@me.com");
            mm.CC.Add("me@me.com");
            //legacy tag header handling.
            mm.Headers.Add("X-PostmarkTag", "mytag");

            var pm = new PostmarkMessage(mm);
            Assert.AreEqual(mm.Subject, pm.Subject);
            Assert.AreEqual(mm.Body, pm.TextBody);
            Assert.AreEqual("mytag", pm.Tag);
        }

        [Test]
        public void Can_generate_postmarkmessage_using_correct_tag_header_from_mailmessage()
        {
            var mm = new MailMessage
            {
                Subject = "test",
                Body = "test"
            };
            mm.To.Add("me@me.com");
            mm.Headers.Add("X-PM-Tag", "correct tag");
            //This header should be overridden by using the correct 'X-PM-Tag'
            mm.Headers.Add("X-PostmarkTag", "overridden tag");

            var pm = new PostmarkMessage(mm);
            Assert.AreEqual("correct tag", pm.Tag);
        }

        [Test]
        //[Ignore("This test sends two real emails.")]
        public void Can_send_batched_messages()
        {
            var postmark = new PostmarkClient(_serverToken);

            var first = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };
            var second = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            var responses = postmark.SendMessages(first, second);
            Assert.AreEqual(2, responses.Count());

            foreach (var response in responses)
            {
                Assert.IsNotNull(response);
                Assert.IsNotNullOrEmpty(response.Message);
                Assert.IsTrue(response.Status == PostmarkStatus.Success);
                Console.WriteLine("Postmark -> {0}", response.Message);
            }
        }

        [Test]
        //[Ignore("This test sends a real email.")]
        public void Can_send_message_with_zipfile_attachment()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody
            };

            email.AddAttachment("zipfile.zip", "application/zip");

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);
            Console.WriteLine("Postmark -> {0}", response.Message);
        }


        // Running the messages API tests require at least 3 messages having been sent for the
        // API token that is used to run the live integration tests.

        [Test]
        //[Ignore("Must use a valid token with messages available to check for")]
        public void Can_retrieve_outbound_messages_from_messages_api()
        {
            var postmark = new PostmarkClient(_serverToken);
            var messages = postmark.GetOutboundMessages(3, 0);

            Assert.AreEqual(3, messages.Messages.Count);
        }

        [Test]
        //[Ignore("Must use a valid token with messages available to check for")]
        public void Can_get_outbound_message_details_and_dump_by_messages_id()
        {
            var postmark = new PostmarkClient(_serverToken);
            var messages = postmark.GetOutboundMessages(1, 0);

            var messagedetails = postmark.GetOutboundMessageDetail(messages.Messages.FirstOrDefault().MessageID);

            var messagedump = postmark.GetOutboundMessageDump(messages.Messages.FirstOrDefault().MessageID);

            Assert.IsNotNull(messagedetails.Body);
            Assert.IsNotNull(messagedump.Body);
        }

        [Test]
        [Ignore("Must use a valid token with messages available to check for")]
        public void Can_get_inbound_messages_from_messages_api()
        {
            var postmark = new PostmarkClient(_serverToken);
            var inboundmessages = postmark.GetInboundMessages(10, 0);

            Assert.AreEqual(1, inboundmessages.InboundMessages.Count);
        }

        [Test]
        [Ignore("Must use a valid token with messages available to check for")]
        public void Can_get_inbound_message_detail_from_api()
        {
            var postmark = new PostmarkClient(_serverToken);
            var inboundmessages = postmark.GetInboundMessages(10, 1);

            var inboundMessage =
                postmark.GetInboundMessageDetail(inboundmessages.InboundMessages.FirstOrDefault().MessageID);

            Assert.IsNotNull(inboundMessage);
        }

        [Test]
        //[Ignore("This test sends a real email.")]
        public void Can_send_message_with_tracking_enabled()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
                HtmlBody = HtmlBody,
                TrackOpens = true
            };

            var response = postmark.SendMessage(email);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> {0}", response.Message);
        }
    }
}
