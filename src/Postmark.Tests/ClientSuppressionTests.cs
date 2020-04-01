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
            _server = TestUtils.MakeSynchronous(() => _adminClient.CreateServerAsync($"integration-test-suppressions-{Guid.NewGuid()}"));
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

            var reactivationResult = result.Suppressions.First();
            Assert.Equal(reactivationRequest.EmailAddress, reactivationResult.EmailAddress);
            Assert.Equal(PostmarkReactivationRequestStatus.Deleted, reactivationResult.Status);
            Assert.Null(reactivationResult.Message);
        }

        [Fact]
        public async Task ClientCanListSuppressions()
        {
            var suppressionRequests = new List<PostmarkSuppressionChangeRequest>();

            for (var i = 0; i < 5; i++)
            {
                var suppressionRequest = new PostmarkSuppressionChangeRequest { EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com" };
                suppressionRequests.Add(suppressionRequest);
            }

            var suppressionResult = await _client.CreateSuppressions(suppressionRequests);
            Assert.Equal(5, suppressionResult.Suppressions.Count());
            Assert.True(suppressionResult.Suppressions.All(k => k.Status == PostmarkSuppressionRequestStatus.Suppressed));

            // Suppressions are being processed asynchronously so we must give it some time to process those requests
            var suppressionListing = await TestUtils.PollUntil(() => _client.ListSuppressions(new PostmarkSuppressionQuery()),
                k => k.Suppressions.Count() == 5);

            Assert.Equal(5, suppressionListing.Suppressions.Count());
        }

        [Fact]
        public async Task ClientCanFilterSuppressionsByEmail()
        {
            var suppressionRequests = new List<PostmarkSuppressionChangeRequest>();

            for (var i = 0; i < 3; i++)
            {
                var suppressionRequest = new PostmarkSuppressionChangeRequest { EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com" };
                suppressionRequests.Add(suppressionRequest);
            }

            await _client.CreateSuppressions(suppressionRequests);

            var query = new PostmarkSuppressionQuery
            {
                EmailAddress = suppressionRequests.First().EmailAddress
            };

            // Suppressions are being processed asynchronously so we must give it some time to process those requests
            var suppressionListing = await TestUtils.PollUntil(() => _client.ListSuppressions(query), k => k.Suppressions.Count() == 1);

            Assert.Single(suppressionListing.Suppressions);

            var actualSuppression = suppressionListing.Suppressions.First();

            Assert.Equal(suppressionRequests.First().EmailAddress, actualSuppression.EmailAddress);
            Assert.Equal("Customer", actualSuppression.Origin);
            Assert.Equal("ManualSuppression", actualSuppression.SuppressionReason);
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
