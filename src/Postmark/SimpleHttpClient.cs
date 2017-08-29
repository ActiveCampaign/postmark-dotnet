using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    internal class SimpleHttpClient : ISimpleHttpClient
    {
        private HttpClient _client = new HttpClient();

        public void Dispose()
        {
            _client.Dispose();
        }

        public TimeSpan Timeout
        {
            get
            {
                return _client.Timeout;
            }
            set
            {
                _client.Timeout = value;
            }
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return _client.SendAsync(request);
        }
    }
}