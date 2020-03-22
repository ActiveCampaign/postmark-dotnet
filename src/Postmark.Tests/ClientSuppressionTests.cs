using Xunit;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postmark.Model.Suppressions;
using PostmarkDotNet.Model;

namespace Postmark.Tests
{
    public class ClientSuppressionTests : ClientBaseFixture, IDisposable
    {
        private PostmarkAdminClient _adminClient;
        private PostmarkServer _server;

        protected override void Setup()
        {
            _adminClient = new PostmarkAdminClient(WRITE_ACCOUNT_TOKEN, BASE_URL);
            _server = _adminClient.CreateServerAsync($"integration-test-suppressions-{Guid.NewGuid()}").Result;
            _client = new PostmarkClient(_server.ApiTokens.First(), BASE_URL);
        }

        [Fact]
        public async Task ClientCanSuppressRecipients()
        {
            var suppressionRequest = new PostmarkSuppressionChangeRequest { EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com" };
            var result = await _client.CreateSuppressions(new List<PostmarkSuppressionChangeRequest> { suppressionRequest });

            Assert.Single(result.Suppressions);

            var suppressionResult = result.Suppressions.First();
            Assert.Equal(suppressionRequest.EmailAddress, suppressionResult.EmailAddress);
            Assert.Equal(PostmarkSuppressionRequestStatus.Suppressed, suppressionResult.Status);
            Assert.Null(suppressionResult.Message);
        }

        [Fact]
        public async Task InvalidRequest_HasFailedStatus()
        {
            var suppressionRequest = new PostmarkSuppressionChangeRequest { EmailAddress = "not-a-correct-email-address" };
            var result = await _client.CreateSuppressions(new List<PostmarkSuppressionChangeRequest> { suppressionRequest });

            Assert.Single(result.Suppressions);

            var suppressionResult = result.Suppressions.First();
            Assert.Equal(suppressionRequest.EmailAddress, suppressionResult.EmailAddress);
            Assert.Equal(PostmarkSuppressionRequestStatus.Failed, suppressionResult.Status);
            Assert.NotNull(suppressionResult.Message);
        }

        [Fact]
        public async Task ClientCanDeleteSuppressions()
        {
            var reactivationRequest = new PostmarkSuppressionChangeRequest { EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com" };
            var result = await _client.DeleteSuppressions(new List<PostmarkSuppressionChangeRequest> { reactivationRequest });

            Assert.Single(result.Suppressions);

            var suppressionResult = result.Suppressions.First();
            Assert.Equal(reactivationRequest.EmailAddress, suppressionResult.EmailAddress);
            Assert.Equal(PostmarkSuppressionRequestStatus.Deleted, suppressionResult.Status);
            Assert.Null(suppressionResult.Message);
        }

        private Task Cleanup()
        {
            return Task.Run(async () =>
            {
                await _adminClient.DeleteServerAsync(_server.ID);
            });
        }

        public void Dispose()
        {
            Cleanup().Wait();
        }
    }
}
