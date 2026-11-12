using System;

namespace Microsoft.Identity.Client.AuthScheme;

internal class AuthSchemeHelper
{
	public static bool StoreTokenTypeInCacheKey(string tokenType)
	{
		if (string.Equals(tokenType, "bearer", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		if (string.Equals(tokenType, "ssh-cert", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return true;
	}
}
