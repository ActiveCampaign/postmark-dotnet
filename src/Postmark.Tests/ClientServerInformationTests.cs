using Xunit;
using PostmarkDotNet;
using System;
using System.Threading.Tasks;
using System.Linq;
using PostmarkDotNet.Model;

namespace Postmark.Tests
{
    public class ClientServerInformationTests : ClientBaseFixture
    {
        private string _serverBaseName;
        private string _name;
        private string _color;

        private string _inboundHookUrl;
        private string _bounceHookUrl;
        private string _openHookUrl;
        private string _clickHookUrl;
        private string _deliveryHookUrl;

        protected override void Setup()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var id = Guid.NewGuid().ToString("n");
            _serverBaseName = "dotnet-integration-test-server";

            _name = $"{_serverBaseName}-{id}";
            _color = ServerColors.Purple;

            _inboundHookUrl = "http://www.example.com/inbound/" + id;
            _bounceHookUrl = "http://www.example.com/bounce/" + id;
            _openHookUrl = "http://www.example.com/opened/" + id;
            _clickHookUrl = "http://www.example.com/click/" + id;
            _deliveryHookUrl = "http://www.example.com/delivery/" + id;
            //_inboundDomain = "inbound-" + id + ".exmaple.com";
        }

        [Fact]
        public async void Client_CanGetServerInformation()
        {
            var server = await _client.GetServerAsync();

            Assert.True(server.ApiTokens.Any());
            Assert.NotNull(server.Name);
            Assert.True(server.ID > 0);
            Assert.NotNull(server.Color);
        }

        [Fact]
        public async void ClientCanUpdateServerName()
        {
            var client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var serverName = _serverBaseName + DateTime.Now.ToString("o");
            var nonModifiedServer = await client.GetServerAsync();
            var updatedServer = await client.EditServerAsync(name: serverName);
            var modifiedServer = await client.GetServerAsync();

            Assert.False(nonModifiedServer.Name == updatedServer.Name, "Updated server name should be different than current name");
            Assert.True(serverName == updatedServer.Name, "Updated server name returned from EditServer should be the same as the value passed in.");
            Assert.True(serverName == modifiedServer.Name, "Updated server name returned from subsequent call to GetServer should show new name.");
        }

        [Fact]
        public async void Client_CanGetEditAServerInformation()
        {
            var existingServer = await _client.GetServerAsync();
            var updatedAffix = "updated";

            var inboundThreshold = 9;

            var updatedServer = await _client.EditServerAsync(
                _name + updatedAffix, ServerColors.Purple,
                !existingServer.RawEmailEnabled, !existingServer.SmtpApiActivated,
                _inboundHookUrl + updatedAffix, _bounceHookUrl + updatedAffix,
                _openHookUrl + updatedAffix, !existingServer.PostFirstOpenOnly,
                !existingServer.TrackOpens, null, inboundThreshold, LinkTrackingOptions.HtmlOnly,
                _clickHookUrl + updatedAffix, _deliveryHookUrl + updatedAffix);

            //go get a fresh copy from the API.
            var retrievedServer = await _client.GetServerAsync();

            //reset this server
            await _client.EditServerAsync(
                existingServer.Name, ServerColors.Yellow,
                existingServer.RawEmailEnabled, existingServer.SmtpApiActivated,
                existingServer.InboundHookUrl, existingServer.BounceHookUrl,
                existingServer.OpenHookUrl, existingServer.PostFirstOpenOnly,
                existingServer.TrackOpens, null,
                inboundThreshold + 1,
                LinkTrackingOptions.None, existingServer.ClickHookUrl, existingServer.DeliveryHookUrl);

            Assert.Equal(_name + updatedAffix, retrievedServer.Name);
            Assert.Equal(ServerColors.Purple, retrievedServer.Color);
            Assert.NotEqual(existingServer.Color, retrievedServer.Color);
            Assert.NotEqual(existingServer.RawEmailEnabled, retrievedServer.RawEmailEnabled);
            Assert.NotEqual(existingServer.SmtpApiActivated, retrievedServer.SmtpApiActivated);
            Assert.Equal(_inboundHookUrl + updatedAffix, retrievedServer.InboundHookUrl);
            Assert.Equal(_bounceHookUrl + updatedAffix, retrievedServer.BounceHookUrl);
            Assert.Equal(_openHookUrl + updatedAffix, retrievedServer.OpenHookUrl);
            Assert.Equal(_clickHookUrl + updatedAffix, retrievedServer.ClickHookUrl);
            Assert.Equal(_deliveryHookUrl + updatedAffix, retrievedServer.DeliveryHookUrl);
            Assert.NotEqual(existingServer.PostFirstOpenOnly, retrievedServer.PostFirstOpenOnly);
            Assert.NotEqual(existingServer.TrackOpens, retrievedServer.TrackOpens);
            Assert.Equal(10, retrievedServer.InboundSpamThreshold);
            Assert.NotEqual(existingServer.InboundSpamThreshold, retrievedServer.InboundSpamThreshold);
            Assert.Equal(LinkTrackingOptions.HtmlOnly, retrievedServer.TrackLinks);
        }

        void Dispose(){
            Task.Run(async () =>
            {
                try
                {
                    await _client.EditServerAsync(name: _serverBaseName);
                }catch{}
            }).Wait();
        }

    }
}
