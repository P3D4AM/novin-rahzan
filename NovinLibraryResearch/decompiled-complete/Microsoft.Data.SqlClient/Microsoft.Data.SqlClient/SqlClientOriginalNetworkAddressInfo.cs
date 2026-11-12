using System;
using System.Net;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlClientOriginalNetworkAddressInfo
{
	private IPAddress _address;

	private bool _isFromDataSecurityProxy;

	public IPAddress Address => _address;

	public bool IsFromDataSecurityProxy => _isFromDataSecurityProxy;

	public SqlClientOriginalNetworkAddressInfo(IPAddress address, bool isFromDataSecurityProxy = false)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		_address = address;
		_isFromDataSecurityProxy = isFromDataSecurityProxy;
	}

	public override int GetHashCode()
	{
		if (_address == null)
		{
			return 0;
		}
		return _address.GetHashCode();
	}

	public override bool Equals(object other)
	{
		if (!(other is SqlClientOriginalNetworkAddressInfo sqlClientOriginalNetworkAddressInfo))
		{
			return false;
		}
		if (sqlClientOriginalNetworkAddressInfo._address != _address)
		{
			return false;
		}
		if (_isFromDataSecurityProxy != sqlClientOriginalNetworkAddressInfo._isFromDataSecurityProxy)
		{
			return false;
		}
		return true;
	}
}
