using System;

namespace Microsoft.Data.ProviderBase;

internal sealed class DbConnectionPoolAuthenticationContextKey
{
	private readonly string _stsAuthority;

	private readonly string _servicePrincipalName;

	private readonly int _hashCode;

	internal string StsAuthority => _stsAuthority;

	internal string ServicePrincipalName => _servicePrincipalName;

	internal DbConnectionPoolAuthenticationContextKey(string stsAuthority, string servicePrincipalName)
	{
		_stsAuthority = stsAuthority;
		_servicePrincipalName = servicePrincipalName;
		_hashCode = ComputeHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (!(obj is DbConnectionPoolAuthenticationContextKey dbConnectionPoolAuthenticationContextKey))
		{
			return false;
		}
		if (string.Equals(StsAuthority, dbConnectionPoolAuthenticationContextKey.StsAuthority, StringComparison.InvariantCultureIgnoreCase))
		{
			return string.Equals(ServicePrincipalName, dbConnectionPoolAuthenticationContextKey.ServicePrincipalName, StringComparison.InvariantCultureIgnoreCase);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _hashCode;
	}

	private int ComputeHashCode()
	{
		int num = 33;
		num = num * 17 + StsAuthority.GetHashCode();
		return num * 17 + ServicePrincipalName.GetHashCode();
	}
}
