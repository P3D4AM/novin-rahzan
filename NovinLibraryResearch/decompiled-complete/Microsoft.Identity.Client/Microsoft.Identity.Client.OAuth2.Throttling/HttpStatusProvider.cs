using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal class HttpStatusProvider : IThrottlingProvider
{
	internal static readonly TimeSpan s_throttleDuration = TimeSpan.FromSeconds(60.0);

	internal ThrottlingCache ThrottlingCache { get; }

	public HttpStatusProvider()
	{
		ThrottlingCache = new ThrottlingCache();
	}

	public void RecordException(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams, MsalServiceException ex)
	{
		ICoreLogger logger = requestParams.RequestContext.Logger;
		if (IsRequestSupported(requestParams) && (ex.StatusCode == 429 || (ex.StatusCode >= 500 && ex.StatusCode < 600)) && !RetryAfterProvider.TryGetRetryAfterValue(ex.Headers, out var retryAfterTimespan))
		{
			string text = $"[Throttling] Http status code {ex.StatusCode} encountered - ";
			retryAfterTimespan = s_throttleDuration;
			logger.Info(text + $"throttling for {retryAfterTimespan.TotalSeconds} seconds");
			string requestStrictThumbprint = ThrottleCommon.GetRequestStrictThumbprint(bodyParams, requestParams.AuthorityInfo.CanonicalAuthority, requestParams.Account?.HomeAccountId?.Identifier);
			ThrottlingCacheEntry entry = new ThrottlingCacheEntry(ex, s_throttleDuration);
			ThrottlingCache.AddAndCleanup(requestStrictThumbprint, entry, logger);
		}
	}

	public void ResetCache()
	{
		ThrottlingCache.Clear();
	}

	public void TryThrottle(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams)
	{
		if (!ThrottlingCache.IsEmpty() && IsRequestSupported(requestParams))
		{
			ICoreLogger logger = requestParams.RequestContext.Logger;
			ThrottleCommon.TryThrowServiceException(ThrottleCommon.GetRequestStrictThumbprint(bodyParams, requestParams.AuthorityInfo.CanonicalAuthority, requestParams.Account?.HomeAccountId?.Identifier), ThrottlingCache, logger, "HttpStatusProvider");
		}
	}

	private static bool IsRequestSupported(AuthenticationRequestParameters requestParameters)
	{
		return !requestParameters.IsConfidentialClient;
	}
}
