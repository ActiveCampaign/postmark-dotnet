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
            Client = new PostmarkClient(ReadSeleniumOpenTrackingToken, BaseUrl);
        }

        [Fact]
        public async void Client_CanGetOpenStatsForMessages()
        {
            var messagestats = await Client.GetOpenEventsForMessagesAsync();
            Assert.True(messagestats.TotalCount > 0);
            Assert.True(messagestats.Opens.Count() > 0);
        }

        [Fact]
        public async void Client_CanGetOpenStatsForSingleMessage()
        {
            var statsbatch = await Client.GetOpenEventsForMessagesAsync(0, 10);
            var messagestats = await Client.GetOpenEventsForMessageAsync
                (statsbatch.Opens.First().MessageID);

            Assert.True(messagestats.Opens.Any());
            Assert.True(messagestats.TotalCount > 0);
            Assert.NotNull(messagestats.Opens.First().MessageID);
        }
    }
}
