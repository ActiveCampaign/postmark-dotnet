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
        public async void ClientCanSuppressRecipients()
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
        public async void InvalidRequest_HasFailedStatus()
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
        public async void ClientCanDeleteSuppressions()
        {
            var reactivationRequest = new PostmarkSuppressionChangeRequest { EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com" };
            var result = await _client.DeleteSuppressions(new List<PostmarkSuppressionChangeRequest> { reactivationRequest });

            Assert.Single(result.Suppressions);

            var suppressionResult = result.Suppressions.First();
            Assert.Equal(reactivationRequest.EmailAddress, suppressionResult.EmailAddress);
            Assert.Equal(PostmarkSuppressionRequestStatus.Deleted, suppressionResult.Status);
            Assert.Null(suppressionResult.Message);
        }

        [Fact]
        public async void ClientCanListSuppressions()
        {
            var suppressionRequests = new List<PostmarkSuppressionChangeRequest>();

            for (var i = 0; i < 5; i++)
            {
                var reactivationRequest = new PostmarkSuppressionChangeRequest { EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com" };
                suppressionRequests.Add(reactivationRequest);
            }

            var suppressionResult = await _client.CreateSuppressions(suppressionRequests);
            Assert.Equal(5, suppressionResult.Suppressions.Count());
            Assert.True(suppressionResult.Suppressions.All(k => k.Status == PostmarkSuppressionRequestStatus.Suppressed));

            // Suppressions are being processed asynchronously so we must give it some time to process those requests
            await Task.Delay(5000);

            var suppressionListing = await _client.ListSuppressions(new PostmarkSuppressionQuery());
            Assert.Equal(5, suppressionListing.Suppressions.Count());
        }


        [Fact]
        public async void ClientCanFilterSuppressionsByEmail()
        {
            var suppressionRequests = new List<PostmarkSuppressionChangeRequest>();

            for (var i = 0; i < 3; i++)
            {
                var reactivationRequest = new PostmarkSuppressionChangeRequest { EmailAddress = $"test-{Guid.NewGuid().ToString()}@gmail.com" };
                suppressionRequests.Add(reactivationRequest);
            }

            await _client.CreateSuppressions(suppressionRequests);

            // Suppressions are being processed asynchronously so we must give it some time to process those requests
            await Task.Delay(5000);

            // Testing the default message stream - should be empty
            var suppressionListing = await _client.ListSuppressions(new PostmarkSuppressionQuery
            {
                EmailAddress = suppressionRequests.First().EmailAddress
            });
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
