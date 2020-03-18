using Xunit;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostmarkDotNet.Exceptions;

namespace Postmark.Tests
{
    public class ClientSendingTests : ClientBaseFixture
    {
        protected override void Setup()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
        }

        [Fact]
        public async void Client_CanSendAndGetOutboundMessageDetailsWithMetadata()
        {
            // prime the test for a future go-round (this ensures this test can run quickly,
            // but it's also a bit of a hack.
            var metadata = new Dictionary<string, string>() {
                    {"client-test", "value-goes-here"}
                };

            await _client.SendMessageAsync(
                WRITE_TEST_SENDER_EMAIL_ADDRESS,
                WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                $"Integration Test - {TESTING_DATE}",
                $"Plain text body, {TESTING_DATE}",
                $"Testing the Postmark .net client, <b>{TESTING_DATE}</b>",
                null,
                metadata
            );

            var result = await _client.GetOutboundMessagesAsync(metadata: metadata);
            
            Assert.True(result.TotalCount > 0, 
                "Messages with the supplied metadata were not found. " +
                "If it has been more than 45 days since these tests have been run on this server, try re-running this test."
            );
        }

        [Fact]
        public async void Client_CanSendASingleMessage()
        {
            var result = await _client.SendMessageAsync(WRITE_TEST_SENDER_EMAIL_ADDRESS,
                WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                String.Format("Integration Test - {0}", TESTING_DATE),
                String.Format("Plain text body, {0}", TESTING_DATE),
                String.Format("Testing the Postmark .net client, <b>{0}</b>", TESTING_DATE),
                new Dictionary<string, string>()
                {
                  {  "X-Integration-Testing" , TESTING_DATE.ToString("o")}
                },
                new Dictionary<string, string>() {
                    {"test-metadata", "value-goes-here"},
                    {"more-metadata", "more-goes-here"}
                });

            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);
        }

        [Fact]
        public async void Client_CanSendAPostmarkMessage()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress);
            var result = await _client.SendMessageAsync(message);

            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);
        }

        [Fact]
        public async void Client_CanSendAPostmarkMessageWithEmptyTrackLinks()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress, 0, null);
            var result = await _client.SendMessageAsync(message);

            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);
        }

        [Fact]
        public async void UnknownMessageStream_ThrowsException()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress, 0, null);
            message.MessageStream = "outbound";

            var result = await _client.SendMessageAsync(message);
            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);

            message.MessageStream = "unknown-stream";

            await Assert.ThrowsAsync<PostmarkValidationException>(() => _client.SendMessageAsync(message));
        }

        private PostmarkMessage ConstructMessage(string inboundAddress, int number = 0, LinkTrackingOptions? trackLinks = LinkTrackingOptions.HtmlAndText)
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
                TrackLinks = trackLinks,
                Headers = new HeaderCollection(){
                  new MailHeader( "X-Integration-Testing-Postmark-Type-Message" , TESTING_DATE.ToString("o"))
                },
                Metadata = new Dictionary<string, string>() { { "stuff", "very-interesting" }, {"client-id", "42"} },
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

        [Fact]
        public async void Client_CanSendABatchOfMessages()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var messages = Enumerable.Range(0, 10)
                .Select(k => ConstructMessage(inboundAddress, k)).ToArray();

            var results = await _client.SendMessagesAsync(messages);

            Assert.True(results.All(k => k.ErrorCode == 0));
            Assert.True(results.All(k => k.Status == PostmarkStatus.Success));
            Assert.Equal(messages.Length, results.Count());
        }
    }
}
