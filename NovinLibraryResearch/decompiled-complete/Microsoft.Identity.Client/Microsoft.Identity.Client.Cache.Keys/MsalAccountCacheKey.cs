using System;
using System.Text;

namespace Microsoft.Identity.Client.Cache.Keys;

internal class MsalAccountCacheKey : IiOSKey
{
	private readonly string _environment;

	private readonly string _homeAccountId;

	private readonly string _tenantId;

	private readonly string _username;

	private readonly string _authorityType;

	public string iOSAccount
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(_homeAccountId ?? "");
			stringBuilder.Append("-");
			stringBuilder.Append(_environment);
			return stringBuilder.ToString().ToLowerInvariant();
		}
	}

	public string iOSGeneric => _username.ToLowerInvariant();

	public string iOSService => (_tenantId ?? "").ToLowerInvariant();

	public int iOSType => MsalCacheKeys.iOSAuthorityTypeToAttrType[_authorityType];

	public MsalAccountCacheKey(string environment, string tenantId, string userIdentifier, string username, string authorityType)
	{
		if (string.IsNullOrEmpty(environment))
		{
			throw new ArgumentNullException("environment");
		}
		_tenantId = tenantId;
		_environment = environment;
		_homeAccountId = userIdentifier;
		_username = username;
		_authorityType = authorityType;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_homeAccountId + "-");
		stringBuilder.Append(_environment + "-");
		stringBuilder.Append(_tenantId);
		return stringBuilder.ToString();
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		MsalAccountCacheKey msalAccountCacheKey = obj as MsalAccountCacheKey;
		return string.Equals(ToString(), msalAccountCacheKey.ToString(), StringComparison.OrdinalIgnoreCase);
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}
}
