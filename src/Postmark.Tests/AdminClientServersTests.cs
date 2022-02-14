using Xunit;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Postmark.Tests
{
    public class AdminClientServersTests : ClientBaseFixture, IAsyncDisposable
    {
        private PostmarkAdminClient _adminClient;
        private string _serverPrefix;
        private bool? _smtpActivated;
        private string _inboundHookUrl;
        private string _openHookUrl;
        private string _clickHookUrl;
        private bool? _postFirstOpenOpenOnly;
        private bool? _trackOpens;
        private int? _inboundSpamThreshold;
        private string _name;
        private string _color;
        private bool? _rawEmailEnabled;
        private string _bounceHookUrl;
        private string _deliveryHookUrl;
        private bool? _enableSmtpApiErrorHooks;

        protected override void Setup()
        {
            _adminClient = new PostmarkAdminClient(WriteAccountToken, BaseUrl);
            var id = Guid.NewGuid().ToString("n");
            _serverPrefix = "admin-client-integration-test-server-";

            _name = _serverPrefix + id;
            _color = ServerColors.Purple;
            _rawEmailEnabled = true;
            _smtpActivated = true;
            _inboundHookUrl = "http://www.example.com/inbound/" + id;
            _bounceHookUrl = "http://www.example.com/bounce/" + id;
            _openHookUrl = "http://www.example.com/opened/" + id;
            _clickHookUrl = "http://www.example.com/clicked/" + id;
            _deliveryHookUrl = "http://www.example.com/delivery/" + id;
            _postFirstOpenOpenOnly = true;
            _trackOpens = true;
            _inboundSpamThreshold = 30;
            _enableSmtpApiErrorHooks = true;
        }

        [Fact]
        public async void AdminClient_CanGetServer()
        {
            var allservers = await _adminClient.GetServersAsync();
            var server = await _adminClient.GetServerAsync(allservers.Servers.First().ID);
            Assert.NotNull(server);
            Assert.NotNull(server.Name);
        }

        [Fact]
        public async void AdminClient_CanListServers()
        {
            var allservers = await _adminClient.GetServersAsync();
            Assert.True(allservers.Servers.Count() > 0);
        }

        [Fact]
        public async void AdminClient_CanEditServer()
        {
            var newServer = await _adminClient.CreateServerAsync(_name, _color, _rawEmailEnabled, _smtpActivated,
                _inboundHookUrl, _bounceHookUrl, _openHookUrl, _postFirstOpenOpenOnly, _trackOpens,
                null, _inboundSpamThreshold);

            var updatedSuffix = "updated";

            var updatedServer = await _adminClient.EditServerAsync(newServer.ID, _name + updatedSuffix, ServerColors.Yellow,
                !newServer.RawEmailEnabled, !newServer.SmtpApiActivated,
                _inboundHookUrl + updatedSuffix, _bounceHookUrl + updatedSuffix,
                _openHookUrl + updatedSuffix, !newServer.PostFirstOpenOnly,
                !newServer.TrackOpens, null, 5, null, _clickHookUrl + updatedSuffix,
                _deliveryHookUrl + updatedSuffix, _enableSmtpApiErrorHooks);

            var retrievedServer = await _adminClient.GetServerAsync(newServer.ID);

            Assert.Equal(_name + updatedSuffix, retrievedServer.Name);
            Assert.Equal(ServerColors.Yellow, retrievedServer.Color);
            Assert.NotEqual(newServer.Color, retrievedServer.Color);
            Assert.Equal(!_rawEmailEnabled, retrievedServer.RawEmailEnabled);
            Assert.Equal(!_smtpActivated, retrievedServer.SmtpApiActivated);
            Assert.Equal(_inboundHookUrl + updatedSuffix, retrievedServer.InboundHookUrl);
            Assert.Equal(_bounceHookUrl + updatedSuffix, retrievedServer.BounceHookUrl);
            Assert.Equal(_clickHookUrl + updatedSuffix, retrievedServer.ClickHookUrl);
            Assert.Equal(_deliveryHookUrl + updatedSuffix, retrievedServer.DeliveryHookUrl);
            Assert.Equal(_openHookUrl + updatedSuffix, retrievedServer.OpenHookUrl);
            Assert.Equal(!_postFirstOpenOpenOnly, retrievedServer.PostFirstOpenOnly);
            Assert.Equal(!_trackOpens, retrievedServer.TrackOpens);
            //Assert.Equal(updatedAffix + _inboundDomain, retrievedServer.InboundDomain);
            Assert.Equal(5, retrievedServer.InboundSpamThreshold);
            Assert.NotEqual(newServer.InboundSpamThreshold, retrievedServer.InboundSpamThreshold);
            Assert.False(newServer.EnableSmtpApiErrorHooks);
            Assert.Equal(_enableSmtpApiErrorHooks, updatedServer.EnableSmtpApiErrorHooks);
        }

        [Fact]
        public async void AdminClient_CanCreateServer()
        {
            var newServer = await _adminClient.CreateServerAsync(_name, _color, _rawEmailEnabled, _smtpActivated,
                _inboundHookUrl, _bounceHookUrl, _openHookUrl, _postFirstOpenOpenOnly, _trackOpens,
                null, _inboundSpamThreshold, null, _clickHookUrl, _deliveryHookUrl, _enableSmtpApiErrorHooks);

            var retrievedServer = await _adminClient.GetServerAsync(newServer.ID);

            Assert.Equal(_name, retrievedServer.Name);
            Assert.Equal(_color, retrievedServer.Color);
            Assert.Equal(_rawEmailEnabled, retrievedServer.RawEmailEnabled);
            Assert.Equal(_smtpActivated, retrievedServer.SmtpApiActivated);
            Assert.Equal(_inboundHookUrl, retrievedServer.InboundHookUrl);
            Assert.Equal(_bounceHookUrl, retrievedServer.BounceHookUrl);
            Assert.Equal(_openHookUrl, retrievedServer.OpenHookUrl);
            Assert.Equal(_deliveryHookUrl, retrievedServer.DeliveryHookUrl);
            Assert.Equal(_clickHookUrl, retrievedServer.ClickHookUrl);
            Assert.Equal(_postFirstOpenOpenOnly, retrievedServer.PostFirstOpenOnly);
            Assert.Equal(_trackOpens, retrievedServer.TrackOpens);
            Assert.True(String.IsNullOrEmpty(retrievedServer.InboundDomain));
            Assert.Equal(_inboundSpamThreshold, retrievedServer.InboundSpamThreshold);
            Assert.Equal(_enableSmtpApiErrorHooks, retrievedServer.EnableSmtpApiErrorHooks);
        }

        [Fact]
        public async void AdminClient_CanDeleteServer()
        {
            var server = await _adminClient.CreateServerAsync(_serverPrefix + Guid.NewGuid().ToString("n"));
            var response = await _adminClient.DeleteServerAsync(server.ID);
            Assert.Equal(PostmarkStatus.Success, response.Status);
            Assert.Equal(0, response.ErrorCode);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                var servers = await _adminClient.GetServersAsync();
                var pendingDeletes = new List<Task>();
                foreach (var server in servers.Servers)
                {
                    if (Regex.IsMatch(server.Name, _serverPrefix))
                    {
                        var deleteTask = _adminClient.DeleteServerAsync(server.ID);
                        pendingDeletes.Add(deleteTask);
                    }
                }
                Task.WaitAll(pendingDeletes.ToArray());
            }catch{}
        }
    }
}
