using Xunit;
using System.Linq;
using PostmarkDotNet;

namespace Postmark.Tests
{
    public class ClientMessageSearchingTests : ClientBaseFixture
    {
        public ClientMessageSearchingTests()
        {
            Client = new PostmarkClient(ReadSeleniumTestServerToken, BaseUrl);
        }

        [Fact]
        public async void Client_CanSearchOutboundMessages()
        {
            //this is a somewhat evil "unit" test, the number of asserts is large, 
            //but necessary so that we don't need to commit expicit
            //addresses/subjects to git.

            var baseList = (await Client.GetOutboundMessagesAsync());

            var fromemail = baseList.Messages
                .First(k => !string.IsNullOrWhiteSpace(k.From))
                .From;
            var recipient = baseList.Messages.First().Recipients.First();
            var tag = "test_tag";
            var subject = baseList.Messages.First().Subject;

            var searchedList = await Client.GetOutboundMessagesAsync(0, 33);
            Assert.Equal(33, searchedList.Messages.Count);

            searchedList = await Client.GetOutboundMessagesAsync(0, 20, recipient);
            Assert.True(searchedList.Messages.Count > 0);
            Assert.True(searchedList.TotalCount > 0);
            Assert.True(searchedList.Messages.All(k => k.Recipients.Contains(recipient)));

            searchedList = await Client.GetOutboundMessagesAsync(0, 20, subject: subject);
            Assert.True(searchedList.Messages.Count > 0);
            Assert.True(searchedList.TotalCount > 0);
            Assert.True(searchedList.Messages.All(k => k.Subject.ToLower().Contains(subject.ToLower())));

            searchedList = await Client.GetOutboundMessagesAsync(0, 20, tag: tag);
            Assert.True(searchedList.Messages.Count > 0);
            Assert.True(searchedList.TotalCount > 0);
            Assert.True(searchedList.Messages.All(k => k.Tag == tag));
        }

        [Fact]
        public async void Client_CanGetOutboundMessageDetails()
        {
            var baseMessage = (await Client.GetOutboundMessagesAsync()).Messages.Last();

            var requestedMessage = await Client.GetOutboundMessageDetailsAsync(baseMessage.MessageID);

            Assert.NotNull(requestedMessage);
            Assert.Equal(baseMessage.MessageID, requestedMessage.MessageID);
        }

        [Fact]
        public async void Client_CanGetMessageDump()
        {
            var baseDetails = (await Client.GetOutboundMessagesAsync()).Messages.First();
            var messageDump = await Client.GetOutboundMessageDumpAsync(baseDetails.MessageID);
            Assert.NotNull(messageDump);
            Assert.NotNull(messageDump.Body);
        }

        [Fact]
        public async void Client_CanSearchInboundMessages()
        {
            var messages = await Client.GetInboundMessagesAsync(0, 10);
            Assert.True(messages.TotalCount > 0);
            Assert.True(messages.InboundMessages.Count > 0);
        }

        [Fact]
        public async void Client_CanGetInboundMessageDetail()
        {
            var messages = await Client.GetInboundMessagesAsync(0, 10);
            var inboundMessageId = messages.InboundMessages.First().MessageID;
            var inboundMessage = await Client.GetInboundMessageDetailsAsync(inboundMessageId);

            Assert.NotNull(inboundMessage);
            Assert.NotNull(inboundMessage.To);
            Assert.Equal(inboundMessageId, inboundMessage.MessageID);
        }

        [Fact(Skip = "We can't run this test because can't do a write on the inbound testing server.")]
        public void Client_CanBypassRulesForInboundMessage()
        {
            //_client.BypassBlockedInboundMessage(/*a message id to bypass*/);
        }
    }
}