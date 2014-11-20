using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    internal class TestConfigurationParameters
    {
        public static string TEST_SERVER_TOKEN = ConfigurationManager.AppSettings["TEST_SERVER_TOKEN"];
        public static string TEST_SENDER_EMAIL_ADDRESS = ConfigurationManager.AppSettings["TEST_SENDER_EMAIL_ADDRESS"];
        public static string TEST_EMAIL_RECIPIENT_ADDRESS = ConfigurationManager.AppSettings["TEST_EMAIL_RECIPIENT_ADDRESS"];

    }
}
