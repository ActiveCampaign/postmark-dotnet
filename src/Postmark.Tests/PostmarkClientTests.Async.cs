using System;
using NUnit.Framework;
using PostmarkDotNet;

namespace Postmark.Tests
{
    partial class PostmarkClientTests
    {
        [Test]
        [Ignore("This test sends a real email.")]
        public void Can_send_message_with_token_and_signature_async()
        {
            var postmark = new PostmarkClient(_serverToken);

            var email = new PostmarkMessage
            {
                To = _to,
                From = _from, // This must be a verified sender signature
                Subject = Subject,
                TextBody = TextBody,
            };
            email.Headers.Add("MIME-Version", "babbaboey!");
            
            var result = postmark.BeginSendMessage(email);
            var response = postmark.EndSendMessage(result);

            Assert.IsNotNull(response);
            Assert.IsNotNullOrEmpty(response.Message);
            Assert.IsTrue(response.Status == PostmarkStatus.Success);

            Console.WriteLine("Postmark -> {0}", response.Message);
        }
    }
}
