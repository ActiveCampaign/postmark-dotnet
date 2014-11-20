using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientMessageSearchingTests : ClientBaseFixture
    {
        private PostmarkDotNet.PostmarkMessage _sentMessage;
        public override async Task Setup()
        {

            _client = new PostmarkDotNet.PostmarkClient(READ_TEST_SERVER_TOKEN);
            var server = await _client.GetServer();

            await _client.SendMessageAsync(_sentMessage);
        }

        [TestCase]
        public async void Client_CanSearchOutboundMessages()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOutboundMessageDetail()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetMessageDump()
        {
            throw new NotImplementedException();
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
