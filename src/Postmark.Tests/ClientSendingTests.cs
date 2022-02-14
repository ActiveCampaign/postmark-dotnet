using Xunit;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostmarkDotNet.Exceptions;

namespace Postmark.Tests
{
    public class ClientSendingTests : ClientBaseFixture
    {
        public ClientSendingTests()
        {
            Client = new PostmarkClient(WriteTestServerToken, BaseUrl);
        }

        [Fact]
        public async void Client_CanSendAndGetOutboundMessageDetailsWithMetadata()
        {
            // prime the test for a future go-round (this ensures this test can run quickly,
            // but it's also a bit of a hack.
            var metadata = new Dictionary<string, string>()
            {
                {"client-test", "value-goes-here"}
            };

            await Client.SendMessageAsync(
                WriteTestSenderEmailAddress,
                WriteTestEmailRecipientAddress,
                $"Integration Test - {TestingDate}",
                $"Plain text body, {TestingDate}",
                $"Testing the Postmark .net client, <b>{TestingDate}</b>",
                null,
                metadata
            );

            var result = await Client.GetOutboundMessagesAsync(metadata: metadata);

            Assert.True(result.TotalCount > 0,
                "Messages with the supplied metadata were not found. " +
                "If it has been more than 45 days since these tests have been run on this server, try re-running this test."
            );
        }

        [Fact]
        public async void Client_CanSendASingleMessage()
        {
            var result = await Client.SendMessageAsync(WriteTestSenderEmailAddress,
                WriteTestEmailRecipientAddress,
                String.Format("Integration Test - {0}", TestingDate),
                String.Format("Plain text body, {0}", TestingDate),
                String.Format("Testing the Postmark .net client, <b>{0}</b>", TestingDate),
                new Dictionary<string, string>()
                {
                    {"X-Integration-Testing", TestingDate.ToString("o")}
                },
                new Dictionary<string, string>()
                {
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
            var inboundAddress = (await Client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress);
            var result = await Client.SendMessageAsync(message);

            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);
        }

        [Fact]
        public async void Client_CanSendAPostmarkMessageWithEmptyTrackLinks()
        {
            var inboundAddress = (await Client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress, 0, null);
            var result = await Client.SendMessageAsync(message);

            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);
        }

        [Fact]
        public async void UnknownMessageStream_ThrowsException()
        {
            var inboundAddress = (await Client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress, 0, null);
            message.MessageStream = "outbound";

            // Testing the default transactional stream
            var result = await Client.SendMessageAsync(message);
            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);

            // Testing an invalid non-existing stream
            message.MessageStream = "unknown-stream";

            await Assert.ThrowsAsync<PostmarkValidationException>(() => Client.SendMessageAsync(message));
        }

        private PostmarkMessage ConstructMessage(
            string inboundAddress, int number = 0, LinkTrackingOptions? trackLinks = LinkTrackingOptions.HtmlAndText)
        {
            var message = new PostmarkMessage()
            {
                From = WriteTestSenderEmailAddress,
                To = WriteTestSenderEmailAddress,
                Cc = WriteTestEmailRecipientAddress,
                Bcc = "testing@example.com",
                Subject = String.Format("Integration Test - {0} - Message #{1}", TestingDate, number),
                HtmlBody = String.Format("Testing the Postmark .net client, <b>{0}</b>", TestingDate),
                TextBody = "This is plain text.",
                TrackOpens = true,
                TrackLinks = trackLinks,
                Headers = new HeaderCollection()
                {
                    new MailHeader("X-Integration-Testing-Postmark-Type-Message", TestingDate.ToString("o"))
                },
                Metadata = new Dictionary<string, string>() {{"stuff", "very-interesting"}, {"client-id", "42"}},
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
            var inboundAddress = (await Client.GetServerAsync()).InboundAddress;
            var messages = Enumerable.Range(0, 10)
                .Select(k => ConstructMessage(inboundAddress, k))
                .ToArray();

            var results = await Client.SendMessagesAsync(messages);

            Assert.True(results.All(k => k.ErrorCode == 0));
            Assert.True(results.All(k => k.Status == PostmarkStatus.Success));
            Assert.Equal(messages.Length, results.Count());
        }
    }
}