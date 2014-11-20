using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class PostmarkClientTests : ClientBaseFixture
    {
        public override async Task Setup()
        {
            //nothing needed.
        }

        [Test]
        public async Task ClientCanSendMessage()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var response = await client
                .SendMessageAsync(WRITE_TEST_SENDER_EMAIL_ADDRESS, WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                "Testing the postmark client: " + DateTime.Now, "<b>This is only a test!</b>");

            // This should successfully send.
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("OK", response.Message);
        }

        [Test]
        public async Task ClientProducesDeliveryStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetDeliveryStatsAsync();
            Assert.True(stats.Bounces.Any());
        }

        [Test]
        public async Task ClientCanRetrieveBounces()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var bounces = await client.GetBouncesAsync();
            Assert.NotNull(bounces);
        }

        [Test]
        public async Task ClientCanRetrieveServerInformation()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var server = await client.GetServer();

            Assert.NotNull(server);
            Assert.Contains(WRITE_TEST_SERVER_TOKEN, server.ApiTokens);
        }


        [Test]
        public async Task ClientCanUpdateServerName()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var serverName = "test-server " + DateTime.Now.ToString("o");
            var nonModifiedServer = await client.GetServer();
            var updatedServer = await client.EditServer(name: serverName);
            var modifiedServer = await client.GetServer();

            Assert.AreNotEqual(nonModifiedServer.Name, updatedServer.Name, "Updated server name should be different than current name");
            Assert.AreEqual(serverName, updatedServer.Name, "Updated server name returned from EditServer should be the same as the value passed in.");
            Assert.AreEqual(serverName, modifiedServer.Name, "Updated server name returned from subsequent call to GetServer should show new name.");
        }

        [Test]
        public async Task ClientCanGetClientUsageStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetOutboundClientUsageCountsAsync();
            Assert.NotNull(stats);
        }

        [Test]
        public async Task ClientCanGetReadtimeStats()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var stats = await client.GetOutboundReadtimeStatsAsync();
            Assert.NotNull(stats);
        }


    }
}
