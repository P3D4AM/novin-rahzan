using System.Collections.Generic;
using Microsoft.Identity.Client.Cache.Items;

namespace Microsoft.Identity.Client.AuthScheme;

internal interface IAuthenticationScheme
{
	string AuthorizationHeaderPrefix { get; }

	string KeyId { get; }

	string AccessTokenType { get; }

	IDictionary<string, string> GetTokenRequestParams();

	string FormatAccessToken(MsalAccessTokenCacheItem msalAccessTokenCacheItem);
}
