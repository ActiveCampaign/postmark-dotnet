using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using Xunit;

namespace Postmark.Tests
{
    /// <summary>
    /// Verifies that the CancellationToken accepted by the async client methods is actually
    /// threaded down to the underlying HttpClient.SendAsync call.
    ///
    /// These tests pass an already-canceled token: HttpClient.SendAsync short-circuits and
    /// throws an OperationCanceledException before any network I/O occurs. If the token were
    /// not threaded through, the call would instead attempt a real request and fail with a
    /// different error, so this reliably distinguishes "threaded" from "dropped" without a
    /// live server, network access, or mutating the global ClientFactory (which would break
    /// the parallel integration tests).
    /// </summary>
    public class ClientCancellationTests
    {
        // Non-routable base URL: if the token were ever dropped, the call would fail here
        // rather than silently contacting the real Postmark API.
        private const string BaseUrl = "http://localhost:9";

        private static PostmarkClient CanceledClient() => new PostmarkClient("test-server-token", BaseUrl);
        private static PostmarkAdminClient CanceledAdminClient() => new PostmarkAdminClient("test-account-token", BaseUrl);

        private static CancellationToken Canceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            return cts.Token;
        }

        [Fact]
        public async Task GetRequest_HonorsCancellationToken()
        {
            var client = CanceledClient();
            await Assert.ThrowsAnyAsync<System.OperationCanceledException>(
                () => client.GetServerAsync(Canceled()));
        }

        [Fact]
        public async Task GetRequestWithQueryParams_HonorsCancellationToken()
        {
            var client = CanceledClient();
            await Assert.ThrowsAnyAsync<System.OperationCanceledException>(
                () => client.GetBouncesAsync(cancellationToken: Canceled()));
        }

        [Fact]
        public async Task PostRequestWithBody_HonorsCancellationToken()
        {
            var client = CanceledClient();
            await Assert.ThrowsAnyAsync<System.OperationCanceledException>(
                () => client.SendMessageAsync(new PostmarkMessage(), Canceled()));
        }

        [Fact]
        public async Task ParamsBatchOverload_HonorsCancellationToken()
        {
            var client = CanceledClient();
            await Assert.ThrowsAnyAsync<System.OperationCanceledException>(
                () => client.SendMessagesAsync(Canceled(), new PostmarkMessage()));
        }

        [Fact]
        public async Task TemplatedParamsOverload_HonorsCancellationToken()
        {
            var client = CanceledClient();
            await Assert.ThrowsAnyAsync<System.OperationCanceledException>(
                () => client.SendEmailWithTemplateAsync(Canceled(), "template-alias", new { name = "x" },
                    "to@example.com", "from@example.com"));
        }

        [Fact]
        public async Task AdminClient_HonorsCancellationToken()
        {
            var admin = CanceledAdminClient();
            await Assert.ThrowsAnyAsync<System.OperationCanceledException>(
                () => admin.GetServersAsync(cancellationToken: Canceled()));
        }

        [Fact]
        public async Task ExtensionMethod_HonorsCancellationToken()
        {
            var client = CanceledClient();
            var messages = new List<PostmarkMessage> { new PostmarkMessage() };
            await Assert.ThrowsAnyAsync<System.OperationCanceledException>(
                () => client.SendMessagesAsync(messages, Canceled()));
        }
    }
}
