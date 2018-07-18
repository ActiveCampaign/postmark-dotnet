using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PostmarkDotNet;

namespace Postmark.Tests
{
    public class ClientMessageSearchingTests : ClientBaseFixture
    {
        protected override void Setup()
        {
            _client = new PostmarkClient(READ_SELENIUM_TEST_SERVER_TOKEN, requestTimeoutInSeconds: 60);
        }

        [Fact]
        public async void Client_CanSearchOutboundMessages()
        {
            //this is a somewhat evil "unit" test, the number of asserts is large, 
            //but necessary so that we don't need to commit expicit
            //addresses/subjects to git.

            var baseList = (await _client.GetOutboundMessagesAsync());

            var fromemail = baseList.Messages
                .First(k => !string.IsNullOrWhiteSpace(k.From)).From;
            var recipient = baseList.Messages.First().Recipients.First();
            var tag = "test_tag";
            var subject = baseList.Messages.First().Subject;

            var searchedList = await _client.GetOutboundMessagesAsync(0, 33);
            Assert.Equal(searchedList.Messages.Count, 33);

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, recipient);
            Assert.True(searchedList.Messages.Count > 0);
            Assert.True(searchedList.TotalCount > 0);
            Assert.True(searchedList.Messages.All(k => k.Recipients.Contains(recipient)));

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, subject: subject);
            Assert.True(searchedList.Messages.Count > 0);
            Assert.True(searchedList.TotalCount > 0);
            Assert.True(searchedList.Messages.All(k => k.Subject == subject));

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, tag: tag);
            Assert.True(searchedList.Messages.Count > 0);
            Assert.True(searchedList.TotalCount > 0);
            Assert.True(searchedList.Messages.All(k => k.Tag == tag));

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, subject: subject);
            Assert.True(searchedList.Messages.Count > 0);
            Assert.True(searchedList.TotalCount > 0);
            Assert.True(searchedList.Messages.All(k => k.Subject == subject));
        }

        [Fact]
        public async void Client_CanGetOutboundMessageDetails()
        {
            var baseMessage = (await _client.GetOutboundMessagesAsync()).Messages.Last();

            var requestedMessage = await _client.GetOutboundMessageDetailsAsync(baseMessage.MessageID);

            Assert.NotNull(requestedMessage);
            Assert.Equal(baseMessage.MessageID, requestedMessage.MessageID);
        }

        //This is a bad test since it requires a Delay before the message has been saved so that we can retrieve details.
        //In other tests (for link and click tracking), we have test accounts that have plenty of messages to sample from
        //We would need to agree on a convetion for metadata and do something similar to avoid having to create and wait for data in this test
        [Fact]
        public async void Client_CanSendAndGetOutboundMessageDetailsWithMetadata()
        {
            var metadata = new Dictionary<string, string>() {
                    {"test-metadata", "value-goes-here"},
                    {"more-metadata", "more-goes-here"}
                };

            var messageWithMetadata = await _client.SendMessageAsync(READ_TEST_SENDER_EMAIL_ADDRESS,
                WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                String.Format("Integration Test - {0}", TESTING_DATE),
                String.Format("Plain text body, {0}", TESTING_DATE),
                String.Format("Testing the Postmark .net client, <b>{0}</b>", TESTING_DATE),
                null,
                metadata
                );

            await Task.Delay(TimeSpan.FromSeconds(2));

            var details = await _client.GetOutboundMessageDetailsAsync(messageWithMetadata.MessageID.ToString());
            Assert.Equal(details.Metadata, metadata);
        }

        [Fact]
        public async void Client_CanGetMessageDump()
        {
            var baseDetails = (await _client.GetOutboundMessagesAsync()).Messages.First();
            var messageDump = await _client.GetOutboundMessageDumpAsync(baseDetails.MessageID);
            Assert.NotNull(messageDump);
            Assert.NotNull(messageDump.Body);
        }

        [Fact]
        public async void Client_CanSearchInboundMessages()
        {
            var messages = await _client.GetInboundMessagesAsync(0, 10);
            Assert.True(messages.TotalCount > 0);
            Assert.True(messages.InboundMessages.Count > 0);
        }

        [Fact]
        public async void Client_CanGetInboundMessageDetail()
        {
            var messages = await _client.GetInboundMessagesAsync(0, 10);
            var inboundMessageId = messages.InboundMessages.First().MessageID;
            var inboundMessage = await _client.GetInboundMessageDetailsAsync(inboundMessageId);

            Assert.NotNull(inboundMessage);
            Assert.NotNull(inboundMessage.To);
            Assert.Equal(inboundMessageId, inboundMessage.MessageID);
        }

        [Fact(Skip="We can't run this test because can't do a write on the inbound testing server.")]
        public void Client_CanBypassRulesForInboundMessage()
        {
            //_client.BypassBlockedInboundMessage(/*a message id to bypass*/);
        }
    }
}
