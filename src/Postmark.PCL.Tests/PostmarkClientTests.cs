
using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Configuration;
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
    }
}
