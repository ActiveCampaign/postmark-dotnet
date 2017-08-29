using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public interface ISimpleHttpClient : IDisposable
    {
        TimeSpan Timeout { get; set; }

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}