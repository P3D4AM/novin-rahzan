using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Http;

namespace Microsoft.Identity.Client.Platforms.net45.Http;

internal class DnsSensitiveClientHandler : DelegatingHandler
{
	private readonly ConcurrentDictionary<EndpointCacheKey, bool> _endpoinsWithTcp = new ConcurrentDictionary<EndpointCacheKey, bool>();

	public DnsSensitiveClientHandler()
	{
		base.InnerHandler = new HttpClientHandler
		{
			UseDefaultCredentials = true
		};
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		AddConnectionLeaseTimeout(request.RequestUri);
		return await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void AddConnectionLeaseTimeout(Uri endpoint)
	{
		if (endpoint.IsAbsoluteUri)
		{
			EndpointCacheKey key = new EndpointCacheKey(endpoint);
			if (!_endpoinsWithTcp.TryGetValue(key, out var _))
			{
				ServicePoint servicePoint = ServicePointManager.FindServicePoint(endpoint);
				TimeSpan connectionLifeTime = HttpClientConfig.ConnectionLifeTime;
				servicePoint.ConnectionLeaseTimeout = (int)connectionLifeTime.TotalMilliseconds;
				_endpoinsWithTcp[key] = true;
			}
		}
	}
}
