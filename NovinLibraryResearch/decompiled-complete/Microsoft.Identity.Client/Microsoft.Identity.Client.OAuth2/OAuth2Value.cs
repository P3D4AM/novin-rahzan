using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.OAuth2;

internal static class OAuth2Value
{
	public const string CodeChallengeMethodValue = "S256";

	public const string ScopeOpenId = "openid";

	public const string ScopeOfflineAccess = "offline_access";

	public const string ScopeProfile = "profile";

	public static readonly HashSet<string> ReservedScopes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "openid", "profile", "offline_access" };
}
