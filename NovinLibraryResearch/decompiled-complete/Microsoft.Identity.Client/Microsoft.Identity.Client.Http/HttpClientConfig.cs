using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Identity.Client.Http;

internal static class HttpClientConfig
{
	public const long MaxResponseContentBufferSizeInBytes = 1048576L;

	public const int MaxConnections = 50;

	public static readonly TimeSpan ConnectionLifeTime = TimeSpan.FromMinutes(1.0);

	public static void ConfigureRequestHeadersAndSize(HttpClient httpClient)
	{
		httpClient.MaxResponseContentBufferSize = 1048576L;
		httpClient.DefaultRequestHeaders.Accept.Clear();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}
}
