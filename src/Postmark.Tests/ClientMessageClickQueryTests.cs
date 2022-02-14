using Xunit;
using System.Linq;
using PostmarkDotNet;

namespace Postmark.Tests
{
    public class ClientMessageClickQueryTests : ClientBaseFixture
    {
        public ClientMessageClickQueryTests()
        {
            Client = new PostmarkClient(ReadLinkTrackingTestServerToken, BaseUrl);
        }

        [Fact]
        public async void Client_CanGetClickStatsForMessages()
        {
            var messagestats = await Client.GetClickEventsForMessagesAsync();
            Assert.True(messagestats.TotalCount > 0);
            Assert.True(messagestats.Clicks.Count() > 0);
        }

        [Fact]
        public async void Client_CanGetClickStatsForSingleMessage()
        {
            var statsbatch = await Client.GetClickEventsForMessagesAsync(0, 10);
            var messagestats = await Client.GetClickEventsForMessageAsync
                (statsbatch.Clicks.First().MessageID);

            Assert.True(messagestats.Clicks.Any());
            Assert.True(messagestats.TotalCount > 0);
            Assert.NotNull(messagestats.Clicks.First().MessageID);
        }
    }
}