using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public abstract class ClientBaseFixture
    {

        /// <summary>
        /// This is just a hook to avoid await compiler warnings.
        /// </summary>
        protected static Task CompletionSource { get; set; }

        static ClientBaseFixture()
        {
            var t = new TaskCompletionSource<int>();
            t.SetResult(1);
            CompletionSource = t.Task;
        }

        private static void AssertSettingsAvailable()
        {
            Assert.NotNull(READ_INBOUND_TEST_SERVER_TOKEN, "READ_INBOUND_TEST_SERVER_TOKEN must be defined as an environment variable or app setting.");
            Assert.NotNull(READ_SELENIUM_TEST_SERVER_TOKEN, "READ_SELENIUM_TEST_SERVER_TOKEN must be defined as an environment variable or app setting.");
            Assert.NotNull(WRITE_ACCOUNT_TOKEN, "WRITE_ACCOUNT_TOKEN must be defined as an environment variable or app setting.");
            Assert.NotNull(WRITE_TEST_SERVER_TOKEN, "WRITE_TEST_SERVER_TOKEN must be defined as an environment variable or app setting.");
            Assert.NotNull(WRITE_TEST_SENDER_EMAIL_ADDRESS, "WRITE_TEST_SENDER_EMAIL_ADDRESS must be defined as an environment variable or app setting.");
            Assert.NotNull(WRITE_TEST_EMAIL_RECIPIENT_ADDRESS, "WRITE_TEST_EMAIL_RECIPIENT_ADDRESS must be defined as an environment variable or app setting.");
            Assert.NotNull(WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE, "WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE must be defined as an environment variable or app setting.");
            Assert.NotNull(API_BASE_URL, "API_BASE_URL must be defined as an environment variable or app setting.");
        }

        /// <summary>
        /// Retrieve the config variable from the environment, 
        /// or app.config if the environment doesn't specify it.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get.</param>
        /// <returns></returns>
        private static string ConfigVariable(string variableName)
        {
            var retval = ConfigurationManager.AppSettings[variableName];
            //this is here to allow us to have a config that isn't committed to source control, but still allows the project to build
            try
            {
                var masterConfig = XDocument.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/../../../../testconfig.config"));
                retval = masterConfig.Root.Element("appSettings").Elements("add")
                       .First(k => k.Attribute("key").Value == variableName).Attribute("value").Value;
            }
            catch
            {
                //This is OK, it just doesn't exist.. no big deal.
            }
            return String.IsNullOrWhiteSpace(retval) ? Environment.GetEnvironmentVariable(variableName) : retval;
        }

        public static readonly DateTime TESTING_DATE = DateTime.Now;

        public static readonly string READ_INBOUND_TEST_SERVER_TOKEN = ConfigVariable("READ_INBOUND_TEST_SERVER_TOKEN");
        public static readonly string READ_SELENIUM_TEST_SERVER_TOKEN = ConfigVariable("READ_SELENIUM_TEST_SERVER_TOKEN");

        public static readonly string WRITE_ACCOUNT_TOKEN = ConfigVariable("WRITE_ACCOUNT_TOKEN");
        public static readonly string WRITE_TEST_SERVER_TOKEN = ConfigVariable("WRITE_TEST_SERVER_TOKEN");
        public static readonly string WRITE_TEST_SENDER_EMAIL_ADDRESS = ConfigVariable("WRITE_TEST_SENDER_EMAIL_ADDRESS");
        public static readonly string WRITE_TEST_EMAIL_RECIPIENT_ADDRESS = ConfigVariable("WRITE_TEST_EMAIL_RECIPIENT_ADDRESS");
        public static readonly string WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE = ConfigVariable("WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE");

        public static readonly string API_BASE_URL = ConfigVariable("API_BASE_URL");

        protected PostmarkClient _client;

        [SetUp]
        public void RunSetupSynchronously()
        {
            AssertSettingsAvailable();
            SetupAsync().Wait();

        }

        protected abstract Task SetupAsync();
    }
}
