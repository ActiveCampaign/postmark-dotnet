using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientMessageSearchingTests : ClientBaseFixture
    {
        public override async Task Setup()
        {
            _client = new PostmarkDotNet.PostmarkClient(READ_SELENIUM_TEST_SERVER_TOKEN);
        }

        [TestCase]
        public async void Client_CanSearchOutboundMessages()
        {
            //this is a somewhat evil "unit" test, the number of asserts is large, 
            //but necessary so that we don't need to commit expicit
            //addresses/subjects to git.

            var baseList = (await _client.GetOutboundMessagesAsync());

            var fromemail = baseList.Messages
                .First(k => !String.IsNullOrWhiteSpace(k.From)).From;
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
            var messages = await _client.GetInboundMessagesAsync();
            Assert.Greater(0, messages.TotalCount);
            Assert.Greater(0, messages.InboundMessages.Count);
        }

        [TestCase]
        public async void Client_CanGetInboundMessageDetail()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanBypassRulesForInboundMessage()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOpenStatsForMessageS()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOpenStatsForSingleMessage()
        {
            throw new NotImplementedException();
        }
    }
}
