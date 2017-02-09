using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;
using PostmarkDotNet;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientMessageSearchingTests : ClientBaseFixture
    {
        protected override async Task SetupAsync()
        {
            _client = new PostmarkClient(READ_SELENIUM_TEST_SERVER_TOKEN, requestTimeoutInSeconds: 60);
            await CompletionSource;
        }

        [TestCase]
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
            Assert.AreEqual(searchedList.Messages.Count, 33);

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, recipient);
            Assert.Greater(searchedList.Messages.Count, 0);
            Assert.Greater(searchedList.TotalCount, 0);
            Assert.IsTrue(searchedList.Messages.All(k => k.Recipients.Contains(recipient)));

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, subject: subject);
            Assert.Greater(searchedList.Messages.Count, 0);
            Assert.Greater(searchedList.TotalCount, 0);
            Assert.IsTrue(searchedList.Messages.All(k => k.Subject == subject));

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, tag: tag);
            Assert.Greater(searchedList.Messages.Count, 0);
            Assert.Greater(searchedList.TotalCount, 0);
            Assert.IsTrue(searchedList.Messages.All(k => k.Tag == tag));

            searchedList = await _client.GetOutboundMessagesAsync(0, 20, subject: subject);
            Assert.Greater(searchedList.Messages.Count, 0);
            Assert.Greater(searchedList.TotalCount, 0);
            Assert.IsTrue(searchedList.Messages.All(k => k.Subject == subject));
        }

        [TestCase]
        public async void Client_CanGetOutboundMessageDetails()
        {
            var baseMessage = (await _client.GetOutboundMessagesAsync()).Messages.Last();

            var requestedMessage = await _client.GetOutboundMessageDetailsAsync(baseMessage.MessageID);

            Assert.NotNull(requestedMessage);
            Assert.AreEqual(baseMessage.MessageID, requestedMessage.MessageID);
        }

        [TestCase]
        public async void Client_CanGetMessageDump()
        {
            var baseDetails = (await _client.GetOutboundMessagesAsync()).Messages.First();
            var messageDump = await _client.GetOutboundMessageDumpAsync(baseDetails.MessageID);
            Assert.NotNull(messageDump);
            Assert.NotNull(messageDump.Body);
        }

        [TestCase]
        public async void Client_CanSearchInboundMessages()
        {
            var messages = await _client.GetInboundMessagesAsync(0, 10);
            Assert.Greater(messages.TotalCount, 0);
            Assert.Greater(messages.InboundMessages.Count, 0);
        }

        [TestCase]
        public async void Client_CanGetInboundMessageDetail()
        {
            var messages = await _client.GetInboundMessagesAsync(0, 10);
            var inboundMessageId = messages.InboundMessages.First().MessageID;
            var inboundMessage = await _client.GetInboundMessageDetailsAsync(inboundMessageId);

            Assert.NotNull(inboundMessage);
            Assert.NotNull(inboundMessage.To);
            Assert.AreEqual(inboundMessageId, inboundMessage.MessageID);
        }

        [Ignore("We can't run this test because can't do a write on the inbound testing server.")]
        [TestCase]
        public void Client_CanBypassRulesForInboundMessage()
        {
            //_client.BypassBlockedInboundMessage(/*a message id to bypass*/);
        }

        [TestCase]
        public async void Client_CanGetOpenStatsForMessages()
        {
            var messagestats = await _client.GetOpenEventsForMessagesAsync();
            Assert.Greater(messagestats.TotalCount, 0);
            Assert.Greater(messagestats.Opens.Count(), 0);
        }

        [TestCase]
        public async void Client_CanGetOpenStatsForSingleMessage()
        {
            var statsbatch = await _client.GetOpenEventsForMessagesAsync(0, 10);
            var messagestats = await _client.GetOpenEventsForMessageAsync
                (statsbatch.Opens.First().MessageID);

            Assert.IsTrue(messagestats.Opens.Any());
            Assert.Greater(messagestats.TotalCount, 0);
            Assert.NotNull(messagestats.Opens.First().MessageID);
        }
    }
}
