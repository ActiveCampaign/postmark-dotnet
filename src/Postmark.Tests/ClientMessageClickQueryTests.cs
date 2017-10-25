using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;
using PostmarkDotNet;

namespace Postmark.Tests
{
    public class ClientMessageClickQueryTests : ClientBaseFixture
    {
        protected override void Setup()
        {
            _client = new PostmarkClient(READ_LINK_TRACKING_TEST_SERVER_TOKEN, requestTimeoutInSeconds: 60);
        }

        [Fact]
        public async void Client_CanGetClickStatsForMessages()
        {
            var messagestats = await _client.GetClickEventsForMessagesAsync();
            Assert.True(messagestats.TotalCount > 0);
            Assert.True(messagestats.Clicks.Count() > 0);
        }

        [Fact]
        public async void Client_CanGetClickStatsForSingleMessage()
        {
            var statsbatch = await _client.GetClickEventsForMessagesAsync(0, 10);
            var messagestats = await _client.GetClickEventsForMessageAsync
                (statsbatch.Clicks.First().MessageID);

            Assert.True(messagestats.Clicks.Any());
            Assert.True(messagestats.TotalCount > 0);
            Assert.NotNull(messagestats.Clicks.First().MessageID);
        }
    }
}
