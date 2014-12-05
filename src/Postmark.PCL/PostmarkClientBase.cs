using PostmarkDotNet.Converters;
using PostmarkDotNet.Exceptions;
using PostmarkDotNet.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkDotNet.PCL
{
    public abstract class PostmarkClientBase
    {

        private static Func<ISimpleHttpClient> _clientFactory = () => new SimpleHttpClient();

        /// <summary>
        /// Configure a global connection factory to to process Http interactions.
        /// 
        /// </summary>
        /// <remarks>
        /// In most cases, you should not need to modify this property, but it's useful
        /// in cases where you want to use another http client, or to mock the http processing
        /// (for tests).
        /// </remarks>
        public static Func<ISimpleHttpClient> ClientFactory
        {
            get
            {
                return _clientFactory;
            }
            set
            {
                _clientFactory = value ?? (() => new SimpleHttpClient());
            }
        }

        protected static readonly string DATE_FORMAT = "yyyy-MM-dd";
        private static readonly string _agent = "Postmark.NET 2.x (" +
              typeof(PostmarkClient).AssemblyQualifiedName + ")";

        private Uri baseUri;

        public PostmarkClientBase(string apiBaseUri = "https://api.postmarkapp.com", int requestTimeoutInSeconds = 30)
        {
            baseUri = new Uri(apiBaseUri);
            _requestTimeout = requestTimeoutInSeconds;
        }

        protected abstract string AuthHeaderName { get; }

        protected string _authToken;
        private int _requestTimeout;

        /// <summary>
        /// The core delivery method for all other API methods.
        /// </summary>
        /// <typeparam name="TRequestBody"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="verb"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="PostmarkDotNet.PCL.Exceptions.PostmarkValidationException"></exception>
        /// <exception cref="System.Exception"></exception>
        protected async Task<TResponse> ProcessRequestAsync<TRequestBody, TResponse>(string apiPath,
            HttpMethod verb, TRequestBody message = null) where TRequestBody : class
        {
            TResponse retval = default(TResponse);

            using (var client = ClientFactory())
            {
                client.BaseAddress = baseUri;

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

                client.Timeout = TimeSpan.FromSeconds(this._requestTimeout);

                var result = await client.SendAsync(request);
                var body = await result.Content.ReadAsStringAsync();

                if (!body.TryDeserializeObject<TResponse>(out retval))
                {
                    PostmarkResponse error;
                    if (body.TryDeserializeObject<PostmarkResponse>(out error))
                    {
                        throw new PostmarkValidationException(error);
                    }
                    else
                    {
                        throw new Exception("The response from the API could not be parsed.");
                    }
                }
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
        protected async Task<TResponse> ProcessNoBodyRequestAsync<TResponse>
            (string apiPath, IDictionary<string, object> parameters = null, HttpMethod verb = null)
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
