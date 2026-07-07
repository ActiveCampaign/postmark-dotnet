using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    internal class SimpleHttpClient : ISimpleHttpClient
    {
        private HttpClient _client = new HttpClient();

        public SimpleHttpClient(TimeSpan? timeoutLength = null){
            _client.Timeout = timeoutLength ?? TimeSpan.FromSeconds(60);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            return _client.SendAsync(request, cancellationToken);
        }

        ~SimpleHttpClient(){
            _client.Dispose();
        }
    }
}
