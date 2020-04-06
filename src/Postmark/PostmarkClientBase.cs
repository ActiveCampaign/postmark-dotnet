using PostmarkDotNet.Converters;
using PostmarkDotNet.Exceptions;
using PostmarkDotNet.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    /// <summary>
    /// The core postmark client code.
    /// </summary>
    public abstract class PostmarkClientBase
    {
        private static Lazy<ISimpleHttpClient> _staticClient = 
            new Lazy<ISimpleHttpClient>(()=>new SimpleHttpClient()) ;

        /// <summary>
        /// Configure a global connection factory to to process HTTP interactions.
        /// </summary>
        /// <remarks>
        /// In most cases, you should not need to modify this property, but it's useful
        /// in cases where you want to use another http client, or to mock the http processing
        /// (for tests).
        /// </remarks>
        public static Func<ISimpleHttpClient> ClientFactory {get;set;} = () => _staticClient.Value;

        protected static readonly string DATE_FORMAT = "yyyy-MM-dd";

        /// <summary>
        /// Default transactional message stream id.
        /// </summary>
        protected const string DefaultTransactionalStream = "outbound";

        private static readonly string _agent = "Postmark.NET 2.x (" +
              typeof(PostmarkClient).AssemblyQualifiedName + ")";

        private Uri baseUri;

        /// <summary>
        /// Provides a base implementation of core request/response interactions.
        /// </summary>
        /// <param name="apiBaseUri"></param>
        /// <param name="requestTimeoutInSeconds"></param>
        public PostmarkClientBase(string apiBaseUri = "https://api.postmarkapp.com")
        {
            baseUri = new Uri(apiBaseUri);
        }

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
        /// <exception cref="System.Exception"></exception>
        protected async Task<TResponse> ProcessRequestAsync<TRequestBody, TResponse>(string apiPath,
            HttpMethod verb, TRequestBody message = null) where TRequestBody : class
        {
            TResponse retval = default(TResponse);

            var client = ClientFactory();
            
            var request = new HttpRequestMessage(verb, baseUri + apiPath.TrimStart('/'));

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

            if (!JsonNetExtensions.TryDeserializeObject<TResponse>(body, out retval) || result.StatusCode != HttpStatusCode.OK)
            {
                PostmarkResponse error;
                if (JsonNetExtensions.TryDeserializeObject<PostmarkResponse>(body, out error))
                {
                    switch ((int)result.StatusCode)
                    {
                        case 422:
                            error.Status = PostmarkStatus.UserError;
                            break;
                        case 500:
                            error.Status = PostmarkStatus.ServerError;
                            break;
                        default:
                            error.Status = PostmarkStatus.Unknown;
                            break;
                    }
                    throw new PostmarkValidationException(error);
                }
                else
                {
                    throw new Exception("The response from the API could not be parsed.");
                }
            }
            
            return retval;
        }

        /// <summary>
        /// Core implementation of HTTP interaction for "no body" requests (i.e. GET/DELETE)
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="parameters"></param>
        /// <param name="verb">The http verb to use for the request.</param>
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