using System.Security;
using Microsoft.Data.Common;
using Microsoft.Data.ProviderBase;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlConnectionPoolGroupProviderInfo : DbConnectionPoolGroupProviderInfo
{
	private string _alias;

	private PermissionSet _failoverPermissionSet;

	private string _failoverPartner;

	private bool _useFailoverPartner;

	internal string FailoverPartner => _failoverPartner;

	internal bool UseFailoverPartner => _useFailoverPartner;

	internal SqlConnectionPoolGroupProviderInfo(SqlConnectionString connectionOptions)
	{
		_failoverPartner = connectionOptions.FailoverPartner;
		if (ADP.IsEmpty(_failoverPartner))
		{
			_failoverPartner = null;
		}
	}

	internal void AliasCheck(string server)
	{
		if (!(_alias != server))
		{
			return;
		}
		lock (this)
		{
			if (_alias == null)
			{
				_alias = server;
			}
			else if (_alias != server)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnectionPoolGroupProviderInfo|INFO> alias change detected. Clearing PoolGroup");
				base.PoolGroup.Clear();
				_alias = server;
			}
		}
	}

	private PermissionSet CreateFailoverPermission(SqlConnectionString userConnectionOptions, string actualFailoverPartner)
	{
		string keyword = ((userConnectionOptions["failover partner"] != null) ? "failover partner" : "data source");
		string connectionString = userConnectionOptions.ExpandKeyword(keyword, actualFailoverPartner);
		return new SqlConnectionString(connectionString).CreatePermissionSet();
	}

	internal void FailoverCheck(SqlInternalConnection connection, bool actualUseFailoverPartner, SqlConnectionString userConnectionOptions, string actualFailoverPartner)
	{
		if (UseFailoverPartner != actualUseFailoverPartner)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnectionPoolGroupProviderInfo|INFO> Failover detected. failover partner='{0}'. Clearing PoolGroup", actualFailoverPartner);
			base.PoolGroup.Clear();
			_useFailoverPartner = actualUseFailoverPartner;
		}
		if (_useFailoverPartner || !(_failoverPartner != actualFailoverPartner))
		{
			return;
		}
		PermissionSet failoverPermissionSet = CreateFailoverPermission(userConnectionOptions, actualFailoverPartner);
		lock (this)
		{
			if (_failoverPartner != actualFailoverPartner)
			{
				_failoverPartner = actualFailoverPartner;
				_failoverPermissionSet = failoverPermissionSet;
			}
		}
	}

	internal void FailoverPermissionDemand()
	{
		if (_useFailoverPartner)
		{
			_failoverPermissionSet?.Demand();
		}
	}
}
