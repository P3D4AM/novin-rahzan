using System;

namespace Microsoft.Identity.Client.Cache.Keys;

internal class MsalRefreshTokenCacheKey : IiOSKey
{
	private readonly string _environment;

	private readonly string _homeAccountId;

	private readonly string _clientId;

	private readonly string _familyId;

	public string iOSAccount => MsalCacheKeys.GetiOSAccountKey(_homeAccountId, _environment);

	public string iOSGeneric
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(_familyId))
			{
				return ("RefreshToken-" + _familyId + "-").ToLowerInvariant();
			}
			return MsalCacheKeys.GetiOSGenericKey("RefreshToken", _clientId, null);
		}
	}

	public string iOSService
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(_familyId))
			{
				return ("RefreshToken-" + _familyId + "--").ToLowerInvariant();
			}
			return MsalCacheKeys.GetiOSServiceKey("RefreshToken", _clientId, null, null);
		}
	}

	public int iOSType => 2002;

	internal MsalRefreshTokenCacheKey(string environment, string clientId, string userIdentifier, string familyId)
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
		_familyId = familyId;
	}

	public override string ToString()
	{
		if (!string.IsNullOrWhiteSpace(_familyId))
		{
			string text = "-";
			return (_homeAccountId + text + _environment + text + "RefreshToken" + text + _familyId + text + text).ToLowerInvariant();
		}
		return MsalCacheKeys.GetCredentialKey(_homeAccountId, _environment, "RefreshToken", _clientId, null, null);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		MsalRefreshTokenCacheKey msalRefreshTokenCacheKey = obj as MsalRefreshTokenCacheKey;
		return string.Equals(ToString(), msalRefreshTokenCacheKey.ToString(), StringComparison.OrdinalIgnoreCase);
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}
}
