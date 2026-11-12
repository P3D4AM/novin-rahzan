using System;

namespace Microsoft.Identity.Client.Cache.Keys;

internal class MsalIdTokenCacheKey : IiOSKey
{
	private readonly string _environment;

	private readonly string _homeAccountId;

	private readonly string _clientId;

	private readonly string _tenantId;

	public string iOSAccount => MsalCacheKeys.GetiOSAccountKey(_homeAccountId, _environment);

	public string iOSGeneric => MsalCacheKeys.GetiOSGenericKey("IdToken", _clientId, _tenantId);

	public string iOSService => MsalCacheKeys.GetiOSServiceKey("IdToken", _clientId, _tenantId, null);

	public int iOSType => 2003;

	public MsalIdTokenCacheKey(string environment, string tenantId, string userIdentifier, string clientId)
	{
		if (string.IsNullOrEmpty(environment))
		{
			throw new ArgumentNullException("environment");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentNullException("clientId");
		}
		_environment = environment;
		_homeAccountId = userIdentifier;
		_clientId = clientId;
		_tenantId = tenantId;
	}

	public override string ToString()
	{
		return MsalCacheKeys.GetCredentialKey(_homeAccountId, _environment, "IdToken", _clientId, _tenantId, null);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		MsalIdTokenCacheKey msalIdTokenCacheKey = obj as MsalIdTokenCacheKey;
		return string.Equals(ToString(), msalIdTokenCacheKey.ToString(), StringComparison.OrdinalIgnoreCase);
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}
}
