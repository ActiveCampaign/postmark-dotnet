using Newtonsoft.Json;
using PostmarkDotNet.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    internal abstract class PostmarkClientBase
    {
        private static readonly string _agent = "Postmark.NET 2.x (" +
              typeof(PostmarkClient).AssemblyQualifiedName + ")";

        private static Uri API_BASE = new Uri("https://api.postmarkapp.com");

        protected abstract string AuthHeaderName { get; }

        protected string _authToken;

        /// <summary>
        /// The core delivery method for all other API methods.
        /// </summary>
        /// <typeparam name="TRequestBody"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="verb"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected async Task<TResponse> ProcessRequestAsync<TRequestBody, TResponse>(string apiPath, HttpMethod verb, TRequestBody message = null) where TRequestBody : class
        {
            TResponse retval;

            using (var client = new HttpClient())
            {
                client.BaseAddress = API_BASE;

                var request = new HttpRequestMessage(verb, apiPath);

                //if the message is not a string, or the message is a non-empty string,
                //set a body for this request.
                if (message != null)
                {
                    var content = new JsonContent<TRequestBody>(message);
                    request.Content = content;
                }

                request.Headers.Add("Accept", "application/json");
                request.Headers.Add(AuthHeaderName, _authToken);
                request.Headers.Add("User-Agent", _agent);

                var result = await client.SendAsync(request);
                var body = await result.Content.ReadAsStringAsync();

                retval = JsonConvert.DeserializeObject<TResponse>(body);
            }

            return retval;
        }

        /// <summary>
        /// For GET/DELETE requests (which should have no
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected async Task<TResponse> ProcessNoBodyRequestAsync<TResponse>(string apiPath, IDictionary<string, object> parameters = null, HttpMethod verb = null)
        {
            parameters = parameters ?? new Dictionary<string, object>();

            var query = "";
            foreach (var param in parameters)
            {
                if (param.Value != null)
                {
                    if (query != "")
                    {
                        query += "&";
                    }
                    query += String.Format("{0}={1}", WebUtility.UrlEncode(param.Key), WebUtility.UrlEncode(param.Value.ToString()));
                }
            }

            if (!String.IsNullOrWhiteSpace(query))
            {
                apiPath = apiPath + "?" + query;
            }

            return await ProcessRequestAsync<object, TResponse>(apiPath, verb ?? HttpMethod.Get);
        }

    }
}
