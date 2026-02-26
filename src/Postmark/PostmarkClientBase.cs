using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Postmark.Exceptions;
using PostmarkDotNet.Converters;
using PostmarkDotNet.Exceptions;
using PostmarkDotNet.Utility;

namespace PostmarkDotNet;

/// <summary>
///     The core postmark client code.
/// </summary>
public abstract class PostmarkClientBase
{
	protected const string DateFormat = "yyyy-MM-dd";

    /// <summary>
    ///     Default transactional message stream id.
    /// </summary>
    protected const string DefaultTransactionalStream = "outbound";

	private readonly HttpClient _client;

	public PostmarkClientBase(HttpClient client)
	{
		_client = client;
	}

    /// <summary>
    ///     The core delivery method for all other API methods.
    /// </summary>
    /// <typeparam name="TRequestBody"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="apiPath"></param>
    /// <param name="verb"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    protected async Task<TResponse> ProcessRequestAsync<TRequestBody, TResponse>(string apiPath, HttpMethod verb, TRequestBody message = null)
		where TRequestBody : class
	{
		var request = new HttpRequestMessage(verb, apiPath.TrimStart('/'));

		//if the message is not a string, or the message is a non-empty string,
		//set a body for this request.
		if (message != null)
		{
			var content = new JsonContent<TRequestBody>(message);
			request.Content = content;
		}

		var result = await _client.SendAsync(request);

		var body = await result.Content.ReadAsStringAsync();

		if (JsonExtensions.TryDeserializeObject(body, out TResponse parsedResponse)
		    && result.StatusCode == HttpStatusCode.OK)
			return parsedResponse;

		if (!JsonExtensions.TryDeserializeObject(body, out PostmarkResponse error)) 
			throw new PostmarkResponseException("The response from the API could not be parsed.", body);

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

    /// <summary>
    ///     Core implementation of HTTP interaction for "no body" requests (i.e. GET/DELETE)
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="apiPath"></param>
    /// <param name="parameters"></param>
    /// <param name="verb">The http verb to use for the request.</param>
    /// <returns></returns>
    protected async Task<TResponse> ProcessNoBodyRequestAsync<TResponse>(
		string apiPath,
		IDictionary<string, object> parameters = null,
		HttpMethod verb = null)
	{
		parameters ??= new Dictionary<string, object>();

		var query = "";
		foreach (var param in parameters)
			if (param.Value != null)
			{
				if (query != "") query += "&";
				query += $"{WebUtility.UrlEncode(param.Key)}={WebUtility.UrlEncode(param.Value.ToString())}";
			}

		if (!string.IsNullOrWhiteSpace(query)) 
			apiPath = $"{apiPath}?{query}";

		return await ProcessRequestAsync<object, TResponse>(apiPath, verb ?? HttpMethod.Get);
	}
}