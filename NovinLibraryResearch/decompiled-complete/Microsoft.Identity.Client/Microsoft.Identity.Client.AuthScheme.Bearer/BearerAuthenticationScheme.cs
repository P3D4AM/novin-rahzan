using System.Collections.Generic;
using Microsoft.Identity.Client.Cache.Items;

namespace Microsoft.Identity.Client.AuthScheme.Bearer;

internal class BearerAuthenticationScheme : IAuthenticationScheme
{
	internal const string BearerTokenType = "bearer";

	public string AuthorizationHeaderPrefix => "Bearer";

	public string AccessTokenType => "bearer";

	public string KeyId => null;

	public string FormatAccessToken(MsalAccessTokenCacheItem msalAccessTokenCacheItem)
	{
		return msalAccessTokenCacheItem.Secret;
	}

	public IDictionary<string, string> GetTokenRequestParams()
	{
		return new Dictionary<string, string>();
	}
}
