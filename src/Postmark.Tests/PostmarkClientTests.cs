using Xunit;
using PostmarkDotNet;
using System;
using System.Linq;

namespace Postmark.Tests
{
    public class PostmarkClientTests : ClientBaseFixture
    {
        public PostmarkClientTests()
        {
            Client = new PostmarkClient(WriteTestServerToken, BaseUrl);
        }
        
        [Fact]
        public async void ClientCanSendMessage()
        {
            var response = await Client
                .SendMessageAsync(WriteTestSenderEmailAddress, WriteTestEmailRecipientAddress,
                    "Testing the postmark client: " + DateTime.Now, "Plain text body", "<b>This is only a test!</b>");

            // This should successfully send.
            Assert.Equal(0, response.ErrorCode);
            Assert.Equal("OK", response.Message);
        }

        [Fact]
        public async void ClientProducesDeliveryStats()
        {
            var stats = await Client.GetDeliveryStatsAsync();
            Assert.True(stats.Bounces.Any());
        }

        [Fact]
        public async void ClientCanRetrieveBounces()
        {
            var bounces = await Client.GetBouncesAsync();
            Assert.NotNull(bounces);
        }

        [Fact]
        public async void ClientCanRetrieveServerInformation()
        {
            var server = await Client.GetServerAsync();

            Assert.NotNull(server);
            Assert.Contains(WriteTestServerToken, server.ApiTokens);
        }


        [Fact]
        public async void ClientCanGetClientUsageStats()
        {
            var stats = await Client.GetOutboundClientUsageCountsAsync();
            Assert.NotNull(stats);
        }

        [Fact]
        public async void ClientCanGetReadtimeStats()
        {
            var stats = await Client.GetOutboundReadtimeStatsAsync();
            Assert.NotNull(stats);
        }
    }
}