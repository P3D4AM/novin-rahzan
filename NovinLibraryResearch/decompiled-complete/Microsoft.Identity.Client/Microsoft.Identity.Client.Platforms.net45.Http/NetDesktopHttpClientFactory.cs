using System;
using System.Net.Http;
using Microsoft.Identity.Client.Http;

namespace Microsoft.Identity.Client.Platforms.net45.Http;

internal class NetDesktopHttpClientFactory : IMsalHttpClientFactory
{
	private static readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() => InitializeClient());

	public HttpClient GetHttpClient()
	{
		return _httpClient.Value;
	}

	private static HttpClient InitializeClient()
	{
		HttpClient httpClient = new HttpClient(new DnsSensitiveClientHandler());
		HttpClientConfig.ConfigureRequestHeadersAndSize(httpClient);
		return httpClient;
	}
}
