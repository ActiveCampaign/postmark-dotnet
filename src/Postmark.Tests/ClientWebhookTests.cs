using Xunit;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostmarkDotNet.Exceptions;
using PostmarkDotNet.Model;
using PostmarkDotNet.Model.Webhooks;

namespace Postmark.Tests
{
    public class ClientWebhookTests : ClientBaseFixture, IAsyncLifetime
    {
        private PostmarkAdminClient _adminClient;
        private PostmarkServer _server;
        
        public async Task InitializeAsync()
        {
            _adminClient = new PostmarkAdminClient(WriteAccountToken, BaseUrl);
            _server = await _adminClient.CreateServerAsync($"integration-test-webhooks-{Guid.NewGuid()}");
            Client = new PostmarkClient(_server.ApiTokens.First(), BaseUrl);
        }

        public async Task DisposeAsync()
        {
            await _adminClient.DeleteServerAsync(_server.ID);
        }

        [Fact]
        public async Task ClientCanCreateConfiguration()
        {
            var url = "http://www.test.com/webhook";
            var messageStream = "outbound";
            var httpAuth = new HttpAuth { Username = "testUser", Password = "testPassword" };
            var httpHeaders = new List<HttpHeader> { new HttpHeader { Name = "testName", Value = "testValue" } };
            var triggers = new WebhookConfigurationTriggers
            {
                Bounce = new WebhookConfigurationBounceTrigger { Enabled = true, IncludeContent = true },
                Click = new WebhookConfigurationClickTrigger { Enabled = true },
                Open = new WebhookConfigurationOpenTrigger { Enabled = true, PostFirstOpenOnly = true },
                Delivery = new WebhookConfigurationDeliveryTrigger { Enabled = true },
                SpamComplaint = new WebhookConfigurationSpamComplaintTrigger { Enabled = true, IncludeContent = true }
            };
            var newConfiguration = await Client.CreateWebhookConfigurationAsync(url, messageStream, httpAuth, httpHeaders, triggers);

            Assert.NotNull(newConfiguration.ID);
            Assert.Equal(url, newConfiguration.Url);
            Assert.Equal(messageStream, newConfiguration.MessageStream);
            Assert.Equal(httpAuth.Username, newConfiguration.HttpAuth.Username);
            Assert.Equal(httpAuth.Password, newConfiguration.HttpAuth.Password);
            Assert.Equal(httpHeaders.First().Name, newConfiguration.HttpHeaders.First().Name);
            Assert.Equal(httpHeaders.First().Value, newConfiguration.HttpHeaders.First().Value);
            Assert.Equal(triggers.Bounce.Enabled, newConfiguration.Triggers.Bounce.Enabled);
            Assert.Equal(triggers.Bounce.IncludeContent, newConfiguration.Triggers.Bounce.IncludeContent);
            Assert.Equal(triggers.Open.Enabled, newConfiguration.Triggers.Open.Enabled);
            Assert.Equal(triggers.Open.PostFirstOpenOnly, newConfiguration.Triggers.Open.PostFirstOpenOnly);
            Assert.Equal(triggers.Click.Enabled, newConfiguration.Triggers.Click.Enabled);
            Assert.Equal(triggers.Delivery.Enabled, newConfiguration.Triggers.Delivery.Enabled);
            Assert.Equal(triggers.SpamComplaint.Enabled, newConfiguration.Triggers.SpamComplaint.Enabled);
            Assert.Equal(triggers.SpamComplaint.IncludeContent, newConfiguration.Triggers.SpamComplaint.IncludeContent);
        }

        [Fact]
        public async Task ClientCanGetWebhookConfiguration()
        {
            var url = "http://www.test123.com/webhook";

            var expectedConfiguration = await Client.CreateWebhookConfigurationAsync(url);

            var actualConfiguration = await Client.GetWebhookConfigurationAsync(expectedConfiguration.ID.Value);

            Assert.Equal(expectedConfiguration.ID, actualConfiguration.ID);
            Assert.Equal(expectedConfiguration.Url, actualConfiguration.Url);
            Assert.Equal("outbound", actualConfiguration.MessageStream);
        }

        [Fact]
        public async Task ClientCanListWebhookConfigurations()
        {
            var url1 = "http://www.test1.com/hook" + Guid.NewGuid();
            var url2 = "http://www.test2.com/hook" + Guid.NewGuid();
            await Client.CreateWebhookConfigurationAsync(url1);
            await Client.CreateWebhookConfigurationAsync(url2);

            var configurations = await Client.GetWebhookConfigurationsAsync();

            Assert.Equal(2, configurations.Webhooks.Count());
            Assert.Contains(configurations.Webhooks, k => k.Url == url1);
            Assert.Contains(configurations.Webhooks, k => k.Url == url2);
        }

        [Fact]
        public async Task ClientCanDeleteWebhookConfigurations()
        {
            var createdResponse = await Client.CreateWebhookConfigurationAsync("http://www.test.com/delete-hook");
            var configuration = await Client.GetWebhookConfigurationAsync(createdResponse.ID.Value);

            var response = await Client.DeleteWebhookConfigurationAsync(configuration.ID.Value);

            Assert.Equal(PostmarkStatus.Success, response.Status);

            await Assert.ThrowsAsync<PostmarkValidationException>(async () =>
                await Client.GetWebhookConfigurationAsync(configuration.ID.Value));
        }

        [Fact]
        public async Task ClientCanEditConfiguration()
        {
            var url = "http://www.test.com/webhook";
            var messageStream = "outbound";
            var httpAuth = new HttpAuth { Username = "testUser", Password = "testPassword" };
            var httpHeaders = new List<HttpHeader> { new HttpHeader { Name = "testName", Value = "testValue" } };
            var triggers = new WebhookConfigurationTriggers
            {
                Bounce = new WebhookConfigurationBounceTrigger { Enabled = true, IncludeContent = true },
                Click = new WebhookConfigurationClickTrigger { Enabled = true },
            };
            var oldConfig = await Client.CreateWebhookConfigurationAsync(url, messageStream, httpAuth, httpHeaders, triggers);

            var newUrl = "http://www.test.com/new-webhook";
            var newHttpAuth = new HttpAuth { Username = "updatedUser", Password = "updatedPassword" };
            var newHeaders = new List<HttpHeader>();
            var triggersUpdate = new WebhookConfigurationTriggers
            {
                Click = new WebhookConfigurationClickTrigger { Enabled = false }
            };
            var updatedConfig = await Client.EditWebhookConfigurationAsync(oldConfig.ID.Value, newUrl, newHttpAuth,
                newHeaders, triggersUpdate);

            Assert.Equal(oldConfig.ID, updatedConfig.ID);
            Assert.Equal(oldConfig.MessageStream, updatedConfig.MessageStream);
            Assert.Equal(newHttpAuth.Username, updatedConfig.HttpAuth.Username);
            Assert.Equal(newHttpAuth.Password, updatedConfig.HttpAuth.Password);
            Assert.Equal(newUrl, updatedConfig.Url);
            Assert.Equal(newHeaders, updatedConfig.HttpHeaders);
            Assert.Equal(triggersUpdate.Click.Enabled, updatedConfig.Triggers.Click.Enabled);
            Assert.Equal(triggers.Bounce.Enabled, updatedConfig.Triggers.Bounce.Enabled);
        }
    }
}
