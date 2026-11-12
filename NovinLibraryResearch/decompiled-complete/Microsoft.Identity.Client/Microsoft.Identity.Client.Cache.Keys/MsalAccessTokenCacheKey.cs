using System;
using Microsoft.Identity.Client.AuthScheme;

namespace Microsoft.Identity.Client.Cache.Keys;

internal class MsalAccessTokenCacheKey : IiOSKey
{
	private readonly string _clientId;

	private readonly string _environment;

	private readonly string _homeAccountId;

	private readonly string _normalizedScopes;

	private readonly string _tenantId;

	private readonly string _tokenType;

	private readonly string _credentialDescriptor;

	private readonly string[] _extraKeyParts;

	public string iOSAccount => MsalCacheKeys.GetiOSAccountKey(_homeAccountId, _environment);

	public string iOSService => MsalCacheKeys.GetiOSServiceKey(_credentialDescriptor, _clientId, _tenantId, _normalizedScopes, _extraKeyParts);

	public string iOSGeneric => MsalCacheKeys.GetiOSGenericKey(_credentialDescriptor, _clientId, _tenantId);

	public int iOSType => 2001;

	internal MsalAccessTokenCacheKey(string environment, string tenantId, string userIdentifier, string clientId, string scopes, string tokenType)
	{
		if (string.IsNullOrEmpty(environment))
		{
			throw new ArgumentNullException("environment");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentNullException("clientId");
		}
		if (string.IsNullOrEmpty(tokenType))
		{
			throw new ArgumentNullException("tokenType");
		}
		_environment = environment;
		_homeAccountId = userIdentifier;
		_clientId = clientId;
		_normalizedScopes = scopes;
		_tokenType = tokenType;
		_tenantId = tenantId;
		_credentialDescriptor = "AccessToken";
		if (AuthSchemeHelper.StoreTokenTypeInCacheKey(tokenType))
		{
			_extraKeyParts = new string[1] { _tokenType };
			_credentialDescriptor = "AccessToken_With_AuthScheme";
		}
	}

	public override string ToString()
	{
		return MsalCacheKeys.GetCredentialKey(_homeAccountId, _environment, _credentialDescriptor, _clientId, _tenantId, _normalizedScopes, _extraKeyParts);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		MsalAccessTokenCacheKey msalAccessTokenCacheKey = obj as MsalAccessTokenCacheKey;
		return string.Equals(ToString(), msalAccessTokenCacheKey.ToString(), StringComparison.OrdinalIgnoreCase);
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}
}
