using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Threading.Tasks;
using System.Linq;
using PostmarkDotNet.Model;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientServerInformationTests : ClientBaseFixture
    {
        private string _serverPrefix;
        private string _name;
        private string _color;
        private bool _rawEmailEnabled;
        private bool _smtpActivated;
        private string _inboundHookUrl;
        private string _bounceHookUrl;
        private string _openHookUrl;
        private bool _postFirstOpenOpenOnly;
        private bool _trackOpens;
        private int _inboundSpamThreshold;
        private string _inboundDomain;
        public override async Task Setup()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var id = Guid.NewGuid().ToString("n");
            _serverPrefix = "integration-test-server-";

            _name = _serverPrefix + id;
            _color = ServerColors.Purple;
            _rawEmailEnabled = true;
            _smtpActivated = true;
            _inboundHookUrl = "http://www.example.com/inbound/" + id;
            _bounceHookUrl = "http://www.example.com/bounce/" + id;
            _openHookUrl = "http://www.example.com/opened/" + id;
            _postFirstOpenOpenOnly = true;
            _trackOpens = true;
            //_inboundDomain = "inbound-" + id + ".exmaple.com";
            _inboundSpamThreshold = 30;
        }

        [TestCase]
        public async void Client_CanGetServerInformation()
        {
            var server = await _client.GetServerAsync();

            Assert.IsTrue(server.ApiTokens.Any());
            Assert.NotNull(server.Name);
            Assert.Greater(server.ID, 0);
            Assert.NotNull(server.Color);
        }

        [TestCase]
        public async void Client_CanGetEditAServerInformation()
        {
            var existingServer = await _client.GetServerAsync();
            var updatedAffix = "updated";

            var updatedServer = await _client.EditServerAsync(
                _name + updatedAffix, ServerColors.Purple,
                !existingServer.RawEmailEnabled, !existingServer.SmtpApiActivated,
                _inboundHookUrl + updatedAffix, _bounceHookUrl + updatedAffix,
                _openHookUrl + updatedAffix, !existingServer.PostFirstOpenOnly,
                !existingServer.TrackOpens, _inboundDomain, 5);

            //go get a fresh copy from the API.
            var retrievedServer = await _client.GetServerAsync();

            //reset this server
            await _client.EditServerAsync(
                existingServer.Name, ServerColors.Yellow,
                existingServer.RawEmailEnabled, existingServer.SmtpApiActivated,
                existingServer.InboundHookUrl, existingServer.BounceHookUrl,
                existingServer.OpenHookUrl, existingServer.PostFirstOpenOnly,
                existingServer.TrackOpens, _inboundDomain,
                existingServer.InboundSpamThreshold);

            Assert.AreEqual(_name + updatedAffix, retrievedServer.Name);
            Assert.AreEqual(ServerColors.Purple, retrievedServer.Color);
            Assert.AreNotEqual(existingServer.Color, retrievedServer.Color);
            Assert.AreNotEqual(existingServer.RawEmailEnabled, retrievedServer.RawEmailEnabled);
            Assert.AreNotEqual(existingServer.SmtpApiActivated, retrievedServer.SmtpApiActivated);
            Assert.AreEqual(_inboundHookUrl + updatedAffix, retrievedServer.InboundHookUrl);
            Assert.AreEqual(_bounceHookUrl + updatedAffix, retrievedServer.BounceHookUrl);
            Assert.AreEqual(_openHookUrl + updatedAffix, retrievedServer.OpenHookUrl);
            Assert.AreNotEqual(existingServer.PostFirstOpenOnly, retrievedServer.PostFirstOpenOnly);
            Assert.AreNotEqual(existingServer.TrackOpens, retrievedServer.TrackOpens);
            Assert.AreEqual(5, retrievedServer.InboundSpamThreshold);
            Assert.AreNotEqual(existingServer.InboundSpamThreshold, retrievedServer.InboundSpamThreshold);
        }


    }
}
