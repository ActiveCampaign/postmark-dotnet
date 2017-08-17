using Xunit;
using PostmarkDotNet;
using System;
using System.Linq;

namespace Postmark.Tests
{
    public class PostmarkClientTests : ClientBaseFixture
    {
        protected override void Setup()
        {
            //no setup needed.
        }

        [Fact]
        public async void ClientCanSendMessage()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var response = await client
                .SendMessageAsync(WRITE_TEST_SENDER_EMAIL_ADDRESS, WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                "Testing the postmark client: " + DateTime.Now, "Plain text body", "<b>This is only a test!</b>");

            // This should successfully send.
            Assert.Equal(0, response.ErrorCode);
            Assert.Equal("OK", response.Message);
        }

        [Fact]
        public async void ClientProducesDeliveryStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetDeliveryStatsAsync();
            Assert.True(stats.Bounces.Any());
        }

        [Fact]
        public async void ClientCanRetrieveBounces()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var bounces = await client.GetBouncesAsync();
            Assert.NotNull(bounces);
        }

        [Fact]
        public async void ClientCanRetrieveServerInformation()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var server = await client.GetServerAsync();

            Assert.NotNull(server);
            Assert.Contains(WRITE_TEST_SERVER_TOKEN, server.ApiTokens);
        }


        [Fact]
        public async void ClientCanGetClientUsageStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetOutboundClientUsageCountsAsync();
            Assert.NotNull(stats);
        }

        [Fact]
        public async void ClientCanGetReadtimeStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetOutboundReadtimeStatsAsync();
            Assert.NotNull(stats);
        }


    }
}
