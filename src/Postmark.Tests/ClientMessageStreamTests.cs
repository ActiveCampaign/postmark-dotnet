using Xunit;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;
using Postmark.Model.MessageStreams;
using PostmarkDotNet.Model;

namespace Postmark.Tests
{
    public class ClientMessageStreamTests : ClientBaseFixture, IAsyncLifetime
    {
        private PostmarkAdminClient _adminClient;
        private PostmarkServer _server;

        protected override void Setup()
        {
            _adminClient = new PostmarkAdminClient(WriteAccountToken, BaseUrl);
            _server = TestUtils.MakeSynchronous(() => _adminClient.CreateServerAsync($"integration-test-message-stream-{Guid.NewGuid()}"));
            Client = new PostmarkClient(_server.ApiTokens.First(), BaseUrl);
        }

        [Fact]
        public async Task ClientCanCreateMessageStream()
        {
            var id = "test-id";
            var streamType = MessageStreamType.Broadcasts;
            var streamName = "Test Stream";
            var description = "This is a description.";

            var messageStream = await Client.CreateMessageStream(id, streamType, streamName, description);

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
            var updatedMessageStream = await Client.EditMessageStream(messageStream.ID, newName, newDescription);

            Assert.Equal(newName, updatedMessageStream.Name);
            Assert.Equal(newDescription, updatedMessageStream.Description);
            Assert.NotNull(updatedMessageStream.UpdatedAt);
        }

        [Fact]
        public async Task ClientCanGetMessageStream()
        {
            var expectedMessageStream = await CreateDummyMessageStream(MessageStreamType.Broadcasts);
            var actualMessageStream = await Client.GetMessageStream(expectedMessageStream.ID);

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
            var listing = await Client.ListMessageStreams(MessageStreamTypeFilter.All);
            Assert.Equal(5, listing.TotalCount); // includes the default streams
            Assert.Equal(5, listing.MessageStreams.Count());
            Assert.Contains(transactionalStream.ID, listing.MessageStreams.Select(k => k.ID));
            Assert.Contains(broadcastsStream.ID, listing.MessageStreams.Select(k => k.ID));

            // Filtering by stream type
            var filteredListing = await Client.ListMessageStreams(MessageStreamTypeFilter.Transactional);
            Assert.Equal(2, filteredListing.TotalCount); // includes default stream
            Assert.Equal(2, filteredListing.MessageStreams.Count());
            Assert.Contains(transactionalStream.ID, listing.MessageStreams.Select(k => k.ID));
        }

        [Fact]
        public async Task ClientCanListArchivedStreams()
        {
            var broadcastStream = await CreateDummyMessageStream(MessageStreamType.Broadcasts);

            await Client.ArchiveMessageStream(broadcastStream.ID);

            // By default we are not including archived streams
            var filteredListing = await Client.ListMessageStreams(MessageStreamTypeFilter.Broadcasts, includeArchivedStreams: false);
            Assert.Equal(1, filteredListing.TotalCount);
            Assert.NotEmpty(filteredListing.MessageStreams);

            // Including archived streams
            var completeListing = await Client.ListMessageStreams(MessageStreamTypeFilter.Broadcasts, includeArchivedStreams: true);
            Assert.Equal(2, completeListing.TotalCount);
            Assert.Equal(2, completeListing.MessageStreams.Count());
            Assert.Equal(broadcastStream.ID, completeListing.MessageStreams.First(s => s.ID.Contains("test")).ID);
            Assert.NotNull(completeListing.MessageStreams.First(s => s.ID.Contains("test")).ArchivedAt);
        }

        [Fact]
        public async Task ClientCanArchiveStreams()
        {
            var transactionalStream = await CreateDummyMessageStream(MessageStreamType.Transactional);

            var confirmation = await Client.ArchiveMessageStream(transactionalStream.ID);

            Assert.Equal(transactionalStream.ID, confirmation.ID);
            Assert.Equal(transactionalStream.ServerID, confirmation.ServerID);
            Assert.True(confirmation.ExpectedPurgeDate > DateTime.UtcNow);

            var fetchedMessageStream = await Client.GetMessageStream(transactionalStream.ID);
            Assert.NotNull(fetchedMessageStream.ArchivedAt);
        }

        [Fact]
        public async Task ClientCanUnArchiveStreams()
        {
            var transactionalStream = await CreateDummyMessageStream(MessageStreamType.Transactional);

            await Client.ArchiveMessageStream(transactionalStream.ID);

            var unarchivedStream = await Client.UnArchiveMessageStream(transactionalStream.ID);

            Assert.Equal(transactionalStream.ID, unarchivedStream.ID);
            Assert.Equal(transactionalStream.ServerID, unarchivedStream.ServerID);
            Assert.Null(unarchivedStream.ArchivedAt);
        }

        private async Task<PostmarkMessageStream> CreateDummyMessageStream(MessageStreamType streamType)
        {
            var id = $"test-{Guid.NewGuid().ToString().Substring(0, 25)}"; // IDs are only 30 characters long.
            var streamName = "Dummy Test Stream";
            var description = "This is a dummy description.";

            return await Client.CreateMessageStream(id, streamType, streamName, description);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _adminClient.DeleteServerAsync(_server.ID);
        }
    }
}
