using System.Collections.Generic;
using System.Text;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal static class ThrottleCommon
{
	public const string ThrottleRetryAfterHeaderName = "x-ms-lib-capability";

	public const string ThrottleRetryAfterHeaderValue = "retry-after, h429";

	internal const string KeyDelimiter = ".";

	public static string GetRequestStrictThumbprint(IReadOnlyDictionary<string, string> bodyParams, string authority, string homeAccountId)
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
		stringBuilder.Append(homeAccountId + ".");
		return stringBuilder.ToString();
	}

	public static void TryThrowServiceException(string thumbprint, ThrottlingCache cache, ICoreLogger logger, string providerName)
	{
		if (cache.TryGetOrRemoveExpired(thumbprint, logger, out var ex))
		{
			logger.WarningPii("[Throttling] Exception thrown because of throttling rule " + providerName + " - thumbprint: " + thumbprint, "[Throttling] Exception thrown because of throttling rule " + providerName);
			throw new MsalThrottledServiceException(ex);
		}
	}
}
