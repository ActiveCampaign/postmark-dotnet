using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;
using PostmarkDotNet;

namespace Postmark.Tests
{
    public class ClientMessageOpenQueryTests : ClientBaseFixture
    {
        protected override void Setup()
        {
            _client = new PostmarkClient(READ_SELENIUM_OPEN_TRACKING_TOKEN);
        }

        [Fact]
        public async void Client_CanGetOpenStatsForMessages()
        {
            var messagestats = await _client.GetOpenEventsForMessagesAsync();
            Assert.True(messagestats.TotalCount > 0);
            Assert.True(messagestats.Opens.Count() > 0);
        }

        [Fact]
        public async void Client_CanGetOpenStatsForSingleMessage()
        {
            var statsbatch = await _client.GetOpenEventsForMessagesAsync(0, 10);
            var messagestats = await _client.GetOpenEventsForMessageAsync
                (statsbatch.Opens.First().MessageID);

            Assert.True(messagestats.Opens.Any());
            Assert.True(messagestats.TotalCount > 0);
            Assert.NotNull(messagestats.Opens.First().MessageID);
        }
    }
}
