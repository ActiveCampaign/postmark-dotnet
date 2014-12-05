using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public interface ISimpleHttpClient : IDisposable
    {
        Uri BaseAddress { get; set; }

        TimeSpan Timeout { get; set; }

        Task<HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request);
    }
}
