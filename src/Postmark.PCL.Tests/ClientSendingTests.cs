using NUnit.Framework;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientSendingTests : ClientBaseFixture
    {
        protected override async Task SetupAsync()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            await CompletionSource;
        }

        [TestCase]
        public async void Client_CanSendASingleMessage()
        {
            var result = await _client.SendMessageAsync(WRITE_TEST_SENDER_EMAIL_ADDRESS,
                WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                String.Format("Integration Test - {0}", TESTING_DATE),
                String.Format("Testing the Postmark .net client, <b>{0}</b>", TESTING_DATE)
                , new Dictionary<string, string>()
                {
                  {  "X-Integration-Testing" , TESTING_DATE.ToString("o")}
                });

            Assert.AreEqual(PostmarkStatus.Success, result.Status);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreNotEqual(Guid.Empty, result.MessageID);
        }

        [TestCase]
        public async void Client_CanSendAPostmarkMessage()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress);
            var result = await _client.SendMessageAsync(message);

            Assert.AreEqual(PostmarkStatus.Success, result.Status);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreNotEqual(Guid.Empty, result.MessageID);
        }

        private PostmarkMessage ConstructMessage(string inboundAddress, int number = 0)
        {
            var message = new PostmarkMessage()
            {
                From = WRITE_TEST_SENDER_EMAIL_ADDRESS,
                To = WRITE_TEST_SENDER_EMAIL_ADDRESS,
                Cc = WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                Bcc = "testing@example.com",
                Subject = String.Format("Integration Test - {0} - Message #{1}", TESTING_DATE, number),
                HtmlBody = String.Format("Testing the Postmark .net client, <b>{0}</b>", TESTING_DATE),
                TextBody = "This is plain text.",
                TrackOpens = true,
                Headers = new HeaderCollection(){
                  {  "X-Integration-Testing-Postmark-Type-Message" , TESTING_DATE.ToString("o")}
                },
                ReplyTo = inboundAddress,
                Tag = "integration-testing"
            };

            var content = "{ \"name\" : \"data\", \"age\" : null }";

            message.Attachments.Add(new PostmarkMessageAttachment()
            {
                ContentType = "application/json",
                Name = "test.json",
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content))
            });
            return message;
        }

        [TestCase]
        public async void Client_CanSendABatchOfMessages()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var messages = Enumerable.Range(0, 10)
                .Select(k => ConstructMessage(inboundAddress, k)).ToArray();

            var results = await _client.SendMessagesAsync(messages);

            Assert.IsTrue(results.All(k => k.ErrorCode == 0));
            Assert.IsTrue(results.All(k => k.Status == PostmarkStatus.Success));
            Assert.AreEqual(messages.Length, results.Count());
        }


    }
}
