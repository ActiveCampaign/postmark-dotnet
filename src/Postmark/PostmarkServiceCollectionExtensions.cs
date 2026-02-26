using System;
using Microsoft.Extensions.DependencyInjection;

namespace PostmarkDotNet
{
	public static class PostmarkServiceCollectionExtensions
	{
		private const string BaseUri = "https://api.postmarkapp.com";
		private static readonly string UserAgent = $"Postmark.NET 2.x ({typeof(PostmarkClient).AssemblyQualifiedName})";
		
		public static IHttpClientBuilder AddPostmarkClient(
			this IServiceCollection services,
			string serverToken)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (string.IsNullOrWhiteSpace(serverToken))
				throw new ArgumentException("serverToken is required.", nameof(serverToken));
			
			return services
			       .AddHttpClient<IPostmarkClient, PostmarkClient>()
			       .ConfigureHttpClient((_, client) =>
			       {
				       client.BaseAddress = new Uri(BaseUri);
				       client.Timeout = TimeSpan.FromSeconds(60);
				       
				       client.DefaultRequestHeaders.Add("Accept", "application/json");
				       client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
				       
				       client.DefaultRequestHeaders.Add("X-Postmark-Server-Token", serverToken);
			       });
		}

		public static IHttpClientBuilder AddPostmarkAdminClient(
			this IServiceCollection services,
			string accountToken)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (string.IsNullOrWhiteSpace(accountToken))
				throw new ArgumentException("accountToken is required.", nameof(accountToken));

			return services
			       .AddHttpClient<IPostmarkAdminClient, PostmarkAdminClient>()
			       .ConfigureHttpClient((_, client) =>
			       {
				       client.BaseAddress = new Uri(BaseUri);
				       client.Timeout = TimeSpan.FromSeconds(60);
				       
				       client.DefaultRequestHeaders.Add("Accept", "application/json");
				       client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
				       
				       client.DefaultRequestHeaders.Add("X-Postmark-Account-Token", accountToken);
			       });
		}
	}
}