using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal class UiRequiredProvider : IThrottlingProvider
{
	internal static readonly TimeSpan s_uiRequiredExpiration = TimeSpan.FromSeconds(120.0);

	internal ThrottlingCache ThrottlingCache { get; }

	public UiRequiredProvider()
	{
		ThrottlingCache = new ThrottlingCache();
	}

	public void RecordException(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams, MsalServiceException ex)
	{
		if (ex is MsalUiRequiredException && IsRequestSupported(requestParams))
		{
			ICoreLogger logger = requestParams.RequestContext.Logger;
			TimeSpan timeSpan = s_uiRequiredExpiration;
			logger.Info("[Throttling] MsalUiRequiredException encountered - " + $"throttling for {timeSpan.TotalSeconds} seconds");
			string requestStrictThumbprint = GetRequestStrictThumbprint(bodyParams, requestParams.AuthorityInfo.CanonicalAuthority, requestParams.RequestContext.ServiceBundle.PlatformProxy.CryptographyManager);
			ThrottlingCacheEntry entry = new ThrottlingCacheEntry(ex, s_uiRequiredExpiration);
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
			string requestStrictThumbprint = GetRequestStrictThumbprint(bodyParams, requestParams.AuthorityInfo.CanonicalAuthority, requestParams.RequestContext.ServiceBundle.PlatformProxy.CryptographyManager);
			TryThrowException(requestStrictThumbprint, logger);
		}
	}

	private void TryThrowException(string thumbprint, ICoreLogger logger)
	{
		if (ThrottlingCache.TryGetOrRemoveExpired(thumbprint, logger, out var ex) && ex is MsalUiRequiredException originalException)
		{
			logger.WarningPii("[Throttling] Exception thrown because of throttling rule UiRequired - thumbprint: " + thumbprint, "[Throttling] Exception thrown because of throttling rule UiRequired ");
			throw new MsalThrottledUiRequiredException(originalException);
		}
	}

	private static bool IsRequestSupported(AuthenticationRequestParameters requestParams)
	{
		if (!requestParams.IsConfidentialClient)
		{
			return requestParams.ApiId == ApiEvent.ApiIds.AcquireTokenSilent;
		}
		return false;
	}

	private static string GetRequestStrictThumbprint(IReadOnlyDictionary<string, string> bodyParams, string authority, ICryptographyManager crypto)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (bodyParams.TryGetValue("client_id", out var value))
		{
			stringBuilder.Append(value + ".");
		}
		stringBuilder.Append(authority + ".");
		if (bodyParams.TryGetValue("scope", out var value2))
		{
			stringBuilder.Append(value2 + ".");
		}
		if (bodyParams.TryGetValue("refresh_token", out var value3) && !string.IsNullOrEmpty(value3))
		{
			stringBuilder.Append(crypto.CreateSha256Hash(value3) + ".");
		}
		return stringBuilder.ToString();
	}
}
