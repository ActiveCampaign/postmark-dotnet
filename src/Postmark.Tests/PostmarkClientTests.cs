using Xunit;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    public class PostmarkClientTests : ClientBaseFixture
    {
        protected override async Task SetupAsync()
        {
            //no setup needed.
            await CompletionSource;
        }

        [Fact]
        public async Task ClientCanSendMessage()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var response = await client
                .SendMessageAsync(WRITE_TEST_SENDER_EMAIL_ADDRESS, WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                "Testing the postmark client: " + DateTime.Now, "<b>This is only a test!</b>");

            // This should successfully send.
            Assert.Equal(0, response.ErrorCode);
            Assert.Equal("OK", response.Message);
        }

        [Fact]
        public async Task ClientProducesDeliveryStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetDeliveryStatsAsync();
            Assert.True(stats.Bounces.Any());
        }

        [Fact]
        public async Task ClientCanRetrieveBounces()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var bounces = await client.GetBouncesAsync();
            Assert.NotNull(bounces);
        }

        [Fact]
        public async Task ClientCanRetrieveServerInformation()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var server = await client.GetServerAsync();

            Assert.NotNull(server);
            Assert.Contains(WRITE_TEST_SERVER_TOKEN, server.ApiTokens);
        }


        [Fact]
        public async Task ClientCanUpdateServerName()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var serverName = "test-server " + DateTime.Now.ToString("o");
            var nonModifiedServer = await client.GetServerAsync();
            var updatedServer = await client.EditServerAsync(name: serverName);
            var modifiedServer = await client.GetServerAsync();

            Assert.False(nonModifiedServer.Name == updatedServer.Name, "Updated server name should be different than current name");
            Assert.True(serverName == updatedServer.Name, "Updated server name returned from EditServer should be the same as the value passed in.");
            Assert.True(serverName == modifiedServer.Name, "Updated server name returned from subsequent call to GetServer should show new name.");
        }

        [Fact]
        public async Task ClientCanGetClientUsageStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetOutboundClientUsageCountsAsync();
            Assert.NotNull(stats);
        }

        [Fact]
        public async Task ClientCanGetReadtimeStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetOutboundReadtimeStatsAsync();
            Assert.NotNull(stats);
        }


    }
}
