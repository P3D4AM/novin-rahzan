using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal class RetryAfterProvider : IThrottlingProvider
{
	internal static readonly TimeSpan MaxRetryAfter = TimeSpan.FromSeconds(3600.0);

	internal ThrottlingCache ThrottlingCache { get; }

	public RetryAfterProvider()
	{
		ThrottlingCache = new ThrottlingCache();
	}

	public void RecordException(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams, MsalServiceException ex)
	{
		if (TryGetRetryAfterValue(ex.Headers, out var retryAfterTimespan))
		{
			retryAfterTimespan = GetSafeValue(retryAfterTimespan);
			ICoreLogger logger = requestParams.RequestContext.Logger;
			logger.Info("[Throttling] Retry-After header detected, " + $"value: {retryAfterTimespan.TotalSeconds} seconds");
			string requestStrictThumbprint = ThrottleCommon.GetRequestStrictThumbprint(bodyParams, requestParams.AuthorityInfo.CanonicalAuthority, requestParams.Account?.HomeAccountId?.Identifier);
			ThrottlingCacheEntry entry = new ThrottlingCacheEntry(ex, retryAfterTimespan);
			ThrottlingCache.AddAndCleanup(requestStrictThumbprint, entry, logger);
		}
	}

	public void ResetCache()
	{
		ThrottlingCache.Clear();
	}

	public void TryThrottle(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams)
	{
		if (!ThrottlingCache.IsEmpty())
		{
			ICoreLogger logger = requestParams.RequestContext.Logger;
			ThrottleCommon.TryThrowServiceException(ThrottleCommon.GetRequestStrictThumbprint(bodyParams, requestParams.AuthorityInfo.CanonicalAuthority, requestParams.Account?.HomeAccountId?.Identifier), ThrottlingCache, logger, "RetryAfterProvider");
		}
	}

	public static bool TryGetRetryAfterValue(HttpResponseHeaders headers, out TimeSpan retryAfterTimespan)
	{
		retryAfterTimespan = TimeSpan.Zero;
		DateTimeOffset? dateTimeOffset = headers?.RetryAfter?.Date;
		if (dateTimeOffset.HasValue)
		{
			retryAfterTimespan = dateTimeOffset.Value - DateTimeOffset.Now;
			return true;
		}
		TimeSpan? timeSpan = headers?.RetryAfter?.Delta;
		if (timeSpan.HasValue)
		{
			retryAfterTimespan = timeSpan.Value;
			return true;
		}
		return false;
	}

	private static TimeSpan GetSafeValue(TimeSpan headerValue)
	{
		if (headerValue > MaxRetryAfter)
		{
			return MaxRetryAfter;
		}
		return headerValue;
	}
}
