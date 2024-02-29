using Xunit;
using PostmarkDotNet;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.Json;

namespace Postmark.Tests
{
    public abstract class ClientBaseFixture
    {
        private static string _assemblyLocation = typeof(ClientBaseFixture).GetTypeInfo().Assembly.Location;

        private static void AssertSettingsAvailable()
        {
            Assert.NotNull(ReadSeleniumTestServerToken);
            Assert.NotNull(ReadSeleniumOpenTrackingToken);
            Assert.NotNull(ReadLinkTrackingTestServerToken);

            Assert.NotNull(WriteAccountToken);
            Assert.NotNull(WriteTestServerToken);
            Assert.NotNull(WriteTestSenderEmailAddress);
            Assert.NotNull(WriteTestEmailRecipientAddress);
            Assert.NotNull(WriteTestSenderSignaturePrototype);
            Assert.NotNull(BaseUrl);
        }

        /// <summary>
        /// Retrieve the config variable from the environment, 
        /// or app.config if the environment doesn't specify it.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get.</param>
        /// <returns></returns>
        private static string ConfigVariable(string variableName)
        {
            string retval = null;
            //this is here to allow us to have a config that isn't committed to source control, but still allows the project to build
            var configFile = Environment.GetEnvironmentVariable("POSTMARK_SDK_CONFIG_FILE_NAME") ?? "testing_keys.json";
            try
            {
                var location = Path.GetFullPath(_assemblyLocation);
                var pathComponents = location.Split(Path.DirectorySeparatorChar).ToList();
                var componentsCount = pathComponents.Count;
                var keyPath = "";
                while (componentsCount > 0)
                {
                    keyPath = Path.Combine(new[] {Path.DirectorySeparatorChar.ToString()}
                        .Concat(pathComponents.Take(componentsCount)
                            .Concat(new[] {configFile}))
                        .ToArray());
                    if (File.Exists(keyPath))
                    {
                        break;
                    }

                    componentsCount--;
                }

                var values = JsonSerializer.Deserialize<Dictionary<String, String>>(File.ReadAllText(keyPath));
                retval = values[variableName];
            }
            catch
            {
                //This is OK, it just doesn't exist.. no big deal.
            }

            return string.IsNullOrWhiteSpace(retval) ? Environment.GetEnvironmentVariable(variableName) : retval;
        }

        protected static readonly DateTime TestingDate = DateTime.Now;

        protected static readonly string ReadSeleniumTestServerToken = ConfigVariable("READ_SELENIUM_TEST_SERVER_TOKEN");
        protected static readonly string ReadLinkTrackingTestServerToken = ConfigVariable("READ_LINK_TRACKING_TEST_SERVER_TOKEN");
        protected static readonly string ReadSeleniumOpenTrackingToken = ConfigVariable("READ_SELENIUM_OPEN_TRACKING_TOKEN");

        protected static readonly string WriteAccountToken = ConfigVariable("WRITE_ACCOUNT_TOKEN");
        protected static readonly string WriteTestServerToken = ConfigVariable("WRITE_TEST_SERVER_TOKEN");
        protected static readonly string WriteTestSenderEmailAddress = ConfigVariable("WRITE_TEST_SENDER_EMAIL_ADDRESS");
        protected static readonly string WriteTestEmailRecipientAddress = ConfigVariable("WRITE_TEST_EMAIL_RECIPIENT_ADDRESS");
        protected static readonly string WriteTestSenderSignaturePrototype = ConfigVariable("WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE");
        protected static readonly string BaseUrl = ConfigVariable("BASE_URL");

        protected PostmarkClient Client;

        protected ClientBaseFixture()
        {
            AssertSettingsAvailable();
        }
    }
}