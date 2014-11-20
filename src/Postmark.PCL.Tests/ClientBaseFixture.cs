using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public abstract class ClientBaseFixture
    {
        public static readonly DateTime TESTING_DATE = DateTime.Now;

        public static string READ_TEST_SERVER_TOKEN = ConfigurationManager.AppSettings["READ_TEST_SERVER_TOKEN"];

        public static string WRITE_ACCOUNT_TOKEN = ConfigurationManager.AppSettings["WRITE_ACCOUNT_TOKEN"];
        public static string WRITE_TEST_SERVER_TOKEN = ConfigurationManager.AppSettings["WRITE_TEST_SERVER_TOKEN"];
        public static string WRITE_TEST_SENDER_EMAIL_ADDRESS = ConfigurationManager.AppSettings["WRITE_TEST_SENDER_EMAIL_ADDRESS"];
        public static string WRITE_TEST_EMAIL_RECIPIENT_ADDRESS = ConfigurationManager.AppSettings["WRITE_TEST_EMAIL_RECIPIENT_ADDRESS"];

        protected PostmarkClient _client;

        [SetUp]
        public void RunSetupSynchronously()
        {
            Setup();
        }

        public abstract Task Setup();
    }
}
