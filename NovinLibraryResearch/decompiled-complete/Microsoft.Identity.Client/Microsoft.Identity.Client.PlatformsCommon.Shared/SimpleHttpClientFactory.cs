using System;
using System.Net.Http;
using Microsoft.Identity.Client.Http;

namespace Microsoft.Identity.Client.PlatformsCommon.Shared;

internal class SimpleHttpClientFactory : IMsalHttpClientFactory
{
	private static readonly Lazy<HttpClient> s_httpClient = new Lazy<HttpClient>(() => InitializeClient());

	private static HttpClient InitializeClient()
	{
		HttpClient httpClient = new HttpClient(new HttpClientHandler
		{
			UseDefaultCredentials = true
		});
		HttpClientConfig.ConfigureRequestHeadersAndSize(httpClient);
		return httpClient;
	}

	public HttpClient GetHttpClient()
	{
		return s_httpClient.Value;
	}
}
