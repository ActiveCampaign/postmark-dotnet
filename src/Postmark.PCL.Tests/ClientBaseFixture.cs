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
        /// <summary>
        /// Retrieve the config variable from the environment, 
        /// or app.config if the environment doesn't specify it.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get.</param>
        /// <returns></returns>
        private static string ConfigVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? ConfigurationManager.AppSettings[variableName];
        }

        public static readonly DateTime TESTING_DATE = DateTime.Now;

        public static string READ_INBOUND_TEST_SERVER_TOKEN = ConfigVariable("READ_INBOUND_TEST_SERVER_TOKEN");
        public static string READ_SELENIUM_TEST_SERVER_TOKEN = ConfigVariable("READ_SELENIUM_TEST_SERVER_TOKEN");

        public static string WRITE_ACCOUNT_TOKEN = ConfigVariable("WRITE_ACCOUNT_TOKEN");
        public static string WRITE_TEST_SERVER_TOKEN = ConfigVariable("WRITE_TEST_SERVER_TOKEN");
        public static string WRITE_TEST_SENDER_EMAIL_ADDRESS = ConfigVariable("WRITE_TEST_SENDER_EMAIL_ADDRESS");
        public static string WRITE_TEST_EMAIL_RECIPIENT_ADDRESS = ConfigVariable("WRITE_TEST_EMAIL_RECIPIENT_ADDRESS");

        protected PostmarkClient _client;

        [SetUp]
        public void RunSetupSynchronously()
        {
            Setup();
        }

        public abstract Task Setup();
    }
}
