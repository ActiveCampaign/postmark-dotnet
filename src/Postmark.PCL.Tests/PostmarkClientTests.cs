using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;


namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class PostmarkClientTests
    {
        private static string TEST_SERVER_TOKEN = ConfigurationManager.AppSettings["TEST_SERVER_TOKEN"];
        private static string TEST_SENDER_EMAIL_ADDRESS = ConfigurationManager.AppSettings["TEST_SENDER_EMAIL_ADDRESS"];
        private static string TEST_EMAIL_RECIPIENT_ADDRESS = ConfigurationManager.AppSettings["TEST_EMAIL_RECIPIENT_ADDRESS"];

        [Test]
        public async Task ClientCanSendMessage()
        {
            var client = new PostmarkClient(TEST_SERVER_TOKEN);
            var response = await client
                .SendMessageAsync(TEST_SENDER_EMAIL_ADDRESS, TEST_SENDER_EMAIL_ADDRESS,
                "Testing the postmark client: " + DateTime.Now, "This is only a test!");

            // This should successfully send.
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("OK", response.Message);
        }

        [Test]
        public async Task ClientProducesDeliveryStats()
        {
            var client = new PostmarkClient(TEST_SERVER_TOKEN);
            var stats = await client.GetDeliveryStatsAsync();
            Assert.True(stats.Bounces.Any());
        }

        [Test]
        public async Task ClientCanRetrieveBounces()
        {
            var client = new PostmarkClient(TEST_SERVER_TOKEN);
            var bounces = await client.GetBouncesAsync();
            Assert.NotNull(bounces);
        }

        [Test]
        public async Task ClientCanRetrieveServerInformation()
        {
            var client = new PostmarkClient(TEST_SERVER_TOKEN);
            var server = await client.GetServer();

            Assert.NotNull(server);
            Assert.Contains(TEST_SERVER_TOKEN, server.ApiTokens);
        }


        [Test]
        public async Task ClientCanUpdateServerName()
        {
            var client = new PostmarkClient(TEST_SERVER_TOKEN);
            var serverName = "test-server " + DateTime.Now.ToString("o");
            var nonModifiedServer = await client.GetServer();
            var updatedServer = await client.EditServer(name: serverName);
            var modifiedServer = await client.GetServer();

            Assert.AreNotEqual(nonModifiedServer.Name, updatedServer.Name, "Updated server name should be different than current name");
            Assert.AreEqual(serverName, updatedServer.Name, "Updated server name returned from EditServer should be the same as the value passed in.");
            Assert.AreEqual(serverName, modifiedServer.Name, "Updated server name returned from subsequent call to GetServer should show new name.");
        }
    }
}
