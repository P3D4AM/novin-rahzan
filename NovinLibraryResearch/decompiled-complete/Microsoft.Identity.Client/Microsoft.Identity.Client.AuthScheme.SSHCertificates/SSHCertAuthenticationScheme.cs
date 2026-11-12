using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.Cache.Items;

namespace Microsoft.Identity.Client.AuthScheme.SSHCertificates;

internal class SSHCertAuthenticationScheme : IAuthenticationScheme
{
	internal const string SSHCertTokenType = "ssh-cert";

	private readonly string _jwk;

	public string AuthorizationHeaderPrefix
	{
		get
		{
			throw new MsalClientException("ssh_cert_used_as_http_header", "MSAL was configured to request SSH certificates from AAD, and these cannot be used as an HTTP authentication header. Developers are responsible for transporting the SSH certificates to the target machines.");
		}
	}

	public string AccessTokenType => "ssh-cert";

	public string KeyId { get; }

	public SSHCertAuthenticationScheme(string keyId, string jwk)
	{
		if (string.IsNullOrEmpty(keyId))
		{
			throw new ArgumentNullException("keyId");
		}
		if (string.IsNullOrEmpty(jwk))
		{
			throw new ArgumentNullException("jwk");
		}
		KeyId = keyId;
		_jwk = jwk;
	}

	public string FormatAccessToken(MsalAccessTokenCacheItem msalAccessTokenCacheItem)
	{
		return msalAccessTokenCacheItem.Secret;
	}

	public IDictionary<string, string> GetTokenRequestParams()
	{
		return new Dictionary<string, string>
		{
			{ "token_type", "ssh-cert" },
			{ "req_cnf", _jwk }
		};
	}
}
