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
        private string _serverPrefix;
        private string _name;
        private string _color;

        private string _inboundHookUrl;
        private string _bounceHookUrl;
        private string _openHookUrl;

        protected override async Task SetupAsync()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
            var id = Guid.NewGuid().ToString("n");
            _serverPrefix = "integration-test-server-";

            _name = _serverPrefix + id;
            _color = ServerColors.Purple;

            _inboundHookUrl = "http://www.example.com/inbound/" + id;
            _bounceHookUrl = "http://www.example.com/bounce/" + id;
            _openHookUrl = "http://www.example.com/opened/" + id;
            //_inboundDomain = "inbound-" + id + ".exmaple.com";
            await Task.CompletedTask;
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
        public async void Client_CanGetEditAServerInformation()
        {
            var existingServer = await _client.GetServerAsync();
            var updatedAffix = "updated";

            var updatedServer = await _client.EditServerAsync(
                _name + updatedAffix, ServerColors.Purple,
                !existingServer.RawEmailEnabled, !existingServer.SmtpApiActivated,
                _inboundHookUrl + updatedAffix, _bounceHookUrl + updatedAffix,
                _openHookUrl + updatedAffix, !existingServer.PostFirstOpenOnly,
                !existingServer.TrackOpens, null, 10, LinkTrackingOptions.HtmlOnly);

            //go get a fresh copy from the API.
            var retrievedServer = await _client.GetServerAsync();

            //reset this server
            await _client.EditServerAsync(
                existingServer.Name, ServerColors.Yellow,
                existingServer.RawEmailEnabled, existingServer.SmtpApiActivated,
                existingServer.InboundHookUrl, existingServer.BounceHookUrl,
                existingServer.OpenHookUrl, existingServer.PostFirstOpenOnly,
                existingServer.TrackOpens, null,
                existingServer.InboundSpamThreshold,
                LinkTrackingOptions.None);

            Assert.Equal(_name + updatedAffix, retrievedServer.Name);
            Assert.Equal(ServerColors.Purple, retrievedServer.Color);
            Assert.NotEqual(existingServer.Color, retrievedServer.Color);
            Assert.NotEqual(existingServer.RawEmailEnabled, retrievedServer.RawEmailEnabled);
            Assert.NotEqual(existingServer.SmtpApiActivated, retrievedServer.SmtpApiActivated);
            Assert.Equal(_inboundHookUrl + updatedAffix, retrievedServer.InboundHookUrl);
            Assert.Equal(_bounceHookUrl + updatedAffix, retrievedServer.BounceHookUrl);
            Assert.Equal(_openHookUrl + updatedAffix, retrievedServer.OpenHookUrl);
            Assert.NotEqual(existingServer.PostFirstOpenOnly, retrievedServer.PostFirstOpenOnly);
            Assert.NotEqual(existingServer.TrackOpens, retrievedServer.TrackOpens);
            Assert.Equal(10, retrievedServer.InboundSpamThreshold);
            Assert.NotEqual(existingServer.InboundSpamThreshold, retrievedServer.InboundSpamThreshold);
            Assert.Equal(LinkTrackingOptions.HtmlOnly, retrievedServer.TrackLinks);
        }


    }
}
