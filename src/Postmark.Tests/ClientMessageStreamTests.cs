using Xunit;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;
using Postmark.Model.MessageStreams;
using PostmarkDotNet.Model;

namespace Postmark.Tests
{
    public class ClientMessageStreamTests : ClientBaseFixture, IDisposable
    {
        private PostmarkAdminClient _adminClient;
        private PostmarkServer _server;

        protected override void Setup()
        {
            _adminClient = new PostmarkAdminClient(WRITE_ACCOUNT_TOKEN, BASE_URL);
            _server = TestUtils.MakeSynchronous(() => _adminClient.CreateServerAsync($"integration-test-message-stream-{Guid.NewGuid()}"));
            _client = new PostmarkClient(_server.ApiTokens.First(), BASE_URL);
        }

        [Fact]
        public async Task ClientCanCreateMessageStream()
        {
            var id = "test-id";
            var streamType = MessageStreamType.Broadcasts;
            var streamName = "Test Stream";
            var description = "This is a description.";

            var messageStream = await _client.CreateMessageStream(id, streamType, streamName, description);

            Assert.Equal(id, messageStream.ID);
            Assert.Equal(_server.ID, messageStream.ServerID);
            Assert.Equal(streamType, messageStream.MessageStreamType);
            Assert.Equal(streamName, messageStream.Name);
            Assert.Equal(description, messageStream.Description);
            Assert.Null(messageStream.UpdatedAt);
            Assert.Null(messageStream.ArchivedAt);
        }

        [Fact]
        public async Task ClientCanEditMessageStream()
        {
            var messageStream = await CreateDummyMessageStream(MessageStreamType.Broadcasts);

            var newName = "Updated Stream Name";
            var newDescription = "Updated Stream Description";
            var updatedMessageStream = await _client.EditMessageStream(messageStream.ID, newName, newDescription);

            Assert.Equal(newName, updatedMessageStream.Name);
            Assert.Equal(newDescription, updatedMessageStream.Description);
            Assert.NotNull(updatedMessageStream.UpdatedAt);
        }

        [Fact]
        public async Task ClientCanGetMessageStream()
        {
            var expectedMessageStream = await CreateDummyMessageStream(MessageStreamType.Broadcasts);
            var actualMessageStream = await _client.GetMessageStream(expectedMessageStream.ID);

            Assert.Equal(expectedMessageStream.ID, actualMessageStream.ID);
            Assert.Equal(expectedMessageStream.ServerID, actualMessageStream.ServerID);
            Assert.Equal(expectedMessageStream.MessageStreamType, actualMessageStream.MessageStreamType);
            Assert.Equal(expectedMessageStream.Name, actualMessageStream.Name);
            Assert.Equal(expectedMessageStream.Description, actualMessageStream.Description);
        }

        [Fact]
        public async Task ClientCanListMessageStreams()
        {
            var transactionalStream = await CreateDummyMessageStream(MessageStreamType.Transactional);
            var broadcastsStream = await CreateDummyMessageStream(MessageStreamType.Broadcasts);

            // Listing All stream types
            var listing = await _client.ListMessageStreams(MessageStreamTypeFilter.All);
            Assert.Equal(4, listing.TotalCount); // includes the default streams
            Assert.Equal(4, listing.MessageStreams.Count());
            Assert.Contains(transactionalStream.ID, listing.MessageStreams.Select(k => k.ID));
            Assert.Contains(broadcastsStream.ID, listing.MessageStreams.Select(k => k.ID));

            // Filtering by stream type
            var filteredListing = await _client.ListMessageStreams(MessageStreamTypeFilter.Transactional);
            Assert.Equal(2, filteredListing.TotalCount); // includes default stream
            Assert.Equal(2, filteredListing.MessageStreams.Count());
            Assert.Contains(transactionalStream.ID, listing.MessageStreams.Select(k => k.ID));
        }

        [Fact]
        public async Task ClientCanListArchivedStreams()
        {
            var transactionalStream = await CreateDummyMessageStream(MessageStreamType.Broadcasts);

            await _client.ArchiveMessageStream(transactionalStream.ID);

            // By default we are not including archived streams
            var filteredListing = await _client.ListMessageStreams(MessageStreamTypeFilter.Broadcasts, includeArchivedStreams: false);
            Assert.Equal(0, filteredListing.TotalCount);
            Assert.Empty(filteredListing.MessageStreams);

            // Including archived streams
            var completeListing = await _client.ListMessageStreams(MessageStreamTypeFilter.Broadcasts, includeArchivedStreams: true);
            Assert.Equal(1, completeListing.TotalCount);
            Assert.Single(completeListing.MessageStreams);
            Assert.Equal(transactionalStream.ID, completeListing.MessageStreams.First().ID);
            Assert.NotNull(completeListing.MessageStreams.First().ArchivedAt);
        }

        [Fact]
        public async Task ClientCanArchiveStreams()
        {
            var transactionalStream = await CreateDummyMessageStream(MessageStreamType.Transactional);

            var confirmation = await _client.ArchiveMessageStream(transactionalStream.ID);

            Assert.Equal(transactionalStream.ID, confirmation.ID);
            Assert.Equal(transactionalStream.ServerID, confirmation.ServerID);
            Assert.True(confirmation.ExpectedPurgeDate > DateTime.UtcNow);

            var fetchedMessageStream = await _client.GetMessageStream(transactionalStream.ID);
            Assert.NotNull(fetchedMessageStream.ArchivedAt);
        }

        [Fact]
        public async Task ClientCanUnArchiveStreams()
        {
            var transactionalStream = await CreateDummyMessageStream(MessageStreamType.Transactional);

            await _client.ArchiveMessageStream(transactionalStream.ID);

            var unarchivedStream = await _client.UnArchiveMessageStream(transactionalStream.ID);

            Assert.Equal(transactionalStream.ID, unarchivedStream.ID);
            Assert.Equal(transactionalStream.ServerID, unarchivedStream.ServerID);
            Assert.Null(unarchivedStream.ArchivedAt);
        }

        private async Task<PostmarkMessageStream> CreateDummyMessageStream(MessageStreamType streamType)
        {
            var id = $"test-{Guid.NewGuid().ToString().Substring(0, 25)}"; // IDs are only 30 characters long.
            var streamName = "Dummy Test Stream";
            var description = "This is a dummy description.";

            return await _client.CreateMessageStream(id, streamType, streamName, description);
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
