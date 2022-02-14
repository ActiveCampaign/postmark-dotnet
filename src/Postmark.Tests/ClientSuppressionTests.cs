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
    public class ClientSuppressionTests : ClientBaseFixture, IAsyncLifetime
    {
        private PostmarkAdminClient _adminClient;
        private PostmarkServer _server;

        public async Task InitializeAsync()
        {
            _adminClient = new PostmarkAdminClient(WriteAccountToken, BaseUrl);
            _server = await _adminClient.CreateServerAsync($"integration-test-suppressions-{Guid.NewGuid()}");
            Client = new PostmarkClient(_server.ApiTokens.First(), BaseUrl);
        }

        public async Task DisposeAsync()
        {
            await _adminClient.DeleteServerAsync(_server.ID);
        }

        [Fact]
        public async Task ClientCanSuppressRecipients()
        {
            var suppressionRequest = new PostmarkSuppressionChangeRequest {EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com"};
            var result = await Client.CreateSuppressions(new List<PostmarkSuppressionChangeRequest> {suppressionRequest});

            Assert.Single(result.Suppressions);

            var suppressionResult = result.Suppressions.First();
            Assert.Equal(suppressionRequest.EmailAddress, suppressionResult.EmailAddress);
            Assert.Equal(PostmarkSuppressionRequestStatus.Suppressed, suppressionResult.Status);
            Assert.Null(suppressionResult.Message);
        }

        [Fact]
        public async Task InvalidRequest_HasFailedStatus()
        {
            var suppressionRequest = new PostmarkSuppressionChangeRequest {EmailAddress = "not-a-correct-email-address"};
            var result = await Client.CreateSuppressions(new List<PostmarkSuppressionChangeRequest> {suppressionRequest});

            Assert.Single(result.Suppressions);

            var suppressionResult = result.Suppressions.First();
            Assert.Equal(suppressionRequest.EmailAddress, suppressionResult.EmailAddress);
            Assert.Equal(PostmarkSuppressionRequestStatus.Failed, suppressionResult.Status);
            Assert.NotNull(suppressionResult.Message);
        }

        [Fact]
        public async Task ClientCanDeleteSuppressions()
        {
            var reactivationRequest = new PostmarkSuppressionChangeRequest {EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com"};
            var result = await Client.DeleteSuppressions(new List<PostmarkSuppressionChangeRequest> {reactivationRequest});

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
                var suppressionRequest = new PostmarkSuppressionChangeRequest {EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com"};
                suppressionRequests.Add(suppressionRequest);
            }

            var suppressionResult = await Client.CreateSuppressions(suppressionRequests);
            Assert.Equal(5, suppressionResult.Suppressions.Count());
            Assert.True(suppressionResult.Suppressions.All(k => k.Status == PostmarkSuppressionRequestStatus.Suppressed));

            // Suppressions are being processed asynchronously so we must give it some time to process those requests
            var suppressionListing = await TestUtils.PollUntil(() => Client.ListSuppressions(new PostmarkSuppressionQuery()),
                k => k.Suppressions.Count() == 5);

            Assert.Equal(5, suppressionListing.Suppressions.Count());
        }

        [Fact]
        public async Task ClientCanFilterSuppressionsByEmail()
        {
            var suppressionRequests = new List<PostmarkSuppressionChangeRequest>();

            for (var i = 0; i < 3; i++)
            {
                var suppressionRequest = new PostmarkSuppressionChangeRequest {EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com"};
                suppressionRequests.Add(suppressionRequest);
            }

            await Client.CreateSuppressions(suppressionRequests);

            var query = new PostmarkSuppressionQuery
            {
                EmailAddress = suppressionRequests.First().EmailAddress
            };

            // Suppressions are being processed asynchronously so we must give it some time to process those requests
            var suppressionListing = await TestUtils.PollUntil(() => Client.ListSuppressions(query), k => k.Suppressions.Count() == 1);

            Assert.Single(suppressionListing.Suppressions);

            var actualSuppression = suppressionListing.Suppressions.First();

            Assert.Equal(suppressionRequests.First().EmailAddress, actualSuppression.EmailAddress);
            Assert.Equal("Customer", actualSuppression.Origin);
            Assert.Equal("ManualSuppression", actualSuppression.SuppressionReason);
        }
    }
}