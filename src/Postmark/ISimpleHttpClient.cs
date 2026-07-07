using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public interface ISimpleHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
    }
}
