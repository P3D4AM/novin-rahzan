using Microsoft.Data.ProviderBase;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlConnectionPoolProviderInfo : DbConnectionPoolProviderInfo
{
	private string _instanceName;

	internal string InstanceName
	{
		get
		{
			return _instanceName;
		}
		set
		{
			_instanceName = value;
		}
	}
}
