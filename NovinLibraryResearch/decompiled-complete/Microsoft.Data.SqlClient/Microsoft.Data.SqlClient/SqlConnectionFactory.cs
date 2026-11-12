using System;
using System.Data.Common;
using System.IO;
using System.Reflection;
using Microsoft.Data.Common;
using Microsoft.Data.ProviderBase;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlConnectionFactory : DbConnectionFactory
{
	public static readonly SqlConnectionFactory SingletonInstance = new SqlConnectionFactory();

	private const string _metaDataXml = "MetaDataXml";

	public override DbProviderFactory ProviderFactory => SqlClientFactory.Instance;

	private SqlConnectionFactory()
		: base(SqlPerformanceCounters.SingletonInstance)
	{
	}

	protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection)
	{
		return CreateConnection(options, poolKey, poolGroupProviderInfo, pool, owningConnection, null);
	}

	protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, DbConnectionPoolKey poolKey, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection, DbConnectionOptions userOptions)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)options;
		SqlConnectionPoolKey sqlConnectionPoolKey = (SqlConnectionPoolKey)poolKey;
		SqlInternalConnection sqlInternalConnection = null;
		SessionData reconnectSessionData = null;
		SqlConnection sqlConnection = owningConnection as SqlConnection;
		bool applyTransientFaultHandling = sqlConnection?._applyTransientFaultHandling ?? false;
		SqlConnectionString userConnectionOptions = null;
		if (userOptions != null)
		{
			userConnectionOptions = (SqlConnectionString)userOptions;
		}
		else if (sqlConnection != null)
		{
			userConnectionOptions = (SqlConnectionString)sqlConnection.UserConnectionOptions;
		}
		if (sqlConnection != null)
		{
			reconnectSessionData = sqlConnection._recoverySessionData;
		}
		if (sqlConnectionString.ContextConnection)
		{
			return GetContextConnection(sqlConnectionString, poolGroupProviderInfo);
		}
		bool redirectedUserInstance = false;
		DbConnectionPoolIdentity identity = null;
		if (sqlConnectionString.IntegratedSecurity || sqlConnectionString.UsesCertificate || sqlConnectionString.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated)
		{
			identity = ((pool == null) ? DbConnectionPoolIdentity.GetCurrent() : pool.Identity);
		}
		if (sqlConnectionString.UserInstance)
		{
			redirectedUserInstance = true;
			string instanceName;
			if (pool == null || (pool != null && pool.Count <= 0))
			{
				SqlInternalConnectionTds sqlInternalConnectionTds = null;
				try
				{
					SqlConnectionString connectionOptions = new SqlConnectionString(sqlConnectionString, sqlConnectionString.DataSource, userInstance: true, false);
					sqlInternalConnectionTds = new SqlInternalConnectionTds(identity, connectionOptions, sqlConnectionPoolKey.Credential, null, "", null, redirectedUserInstance: false, null, null, null, null, null, null, null, applyTransientFaultHandling);
					instanceName = sqlInternalConnectionTds.InstanceName;
					if (!instanceName.StartsWith("\\\\.\\", StringComparison.Ordinal))
					{
						throw SQL.NonLocalSSEInstance();
					}
					if (pool != null)
					{
						SqlConnectionPoolProviderInfo sqlConnectionPoolProviderInfo = (SqlConnectionPoolProviderInfo)pool.ProviderInfo;
						sqlConnectionPoolProviderInfo.InstanceName = instanceName;
					}
				}
				finally
				{
					sqlInternalConnectionTds?.Dispose();
				}
			}
			else
			{
				SqlConnectionPoolProviderInfo sqlConnectionPoolProviderInfo2 = (SqlConnectionPoolProviderInfo)pool.ProviderInfo;
				instanceName = sqlConnectionPoolProviderInfo2.InstanceName;
			}
			sqlConnectionString = new SqlConnectionString(sqlConnectionString, instanceName, userInstance: false, null);
			poolGroupProviderInfo = null;
		}
		return new SqlInternalConnectionTds(identity, sqlConnectionString, sqlConnectionPoolKey.Credential, poolGroupProviderInfo, "", null, redirectedUserInstance, userConnectionOptions, reconnectSessionData, sqlConnectionPoolKey.ServerCertificateValidationCallback, sqlConnectionPoolKey.ClientCertificateRetrievalCallback, pool, sqlConnectionPoolKey.AccessToken, sqlConnectionPoolKey.OriginalNetworkAddressInfo, applyTransientFaultHandling);
	}

	protected override DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous)
	{
		return new SqlConnectionString(connectionString);
	}

	internal override DbConnectionPoolProviderInfo CreateConnectionPoolProviderInfo(DbConnectionOptions connectionOptions)
	{
		DbConnectionPoolProviderInfo result = null;
		if (((SqlConnectionString)connectionOptions).UserInstance)
		{
			result = new SqlConnectionPoolProviderInfo();
		}
		return result;
	}

	protected override DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions connectionOptions)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)connectionOptions;
		DbConnectionPoolGroupOptions result = null;
		if (!sqlConnectionString.ContextConnection && sqlConnectionString.Pooling)
		{
			int num = sqlConnectionString.ConnectTimeout;
			if (0 < num && num < 2147483)
			{
				num *= 1000;
			}
			else if (num >= 2147483)
			{
				num = int.MaxValue;
			}
			if (sqlConnectionString.Authentication == SqlAuthenticationMethod.ActiveDirectoryInteractive)
			{
				num = ((num < 214748364) ? (num * 10) : int.MaxValue);
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlConnectionFactory.CreateConnectionPoolGroupOptions>Set connection pool CreateTimeout={0} when AD Interactive is in use.", num);
			}
			result = new DbConnectionPoolGroupOptions(sqlConnectionString.IntegratedSecurity || sqlConnectionString.UsesCertificate || sqlConnectionString.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated, sqlConnectionString.MinPoolSize, sqlConnectionString.MaxPoolSize, num, sqlConnectionString.LoadBalanceTimeout, sqlConnectionString.Enlist);
		}
		return result;
	}

	protected override DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
	{
		Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Microsoft.Data.SqlClient.SqlMetaData.xml");
		cacheMetaDataFactory = true;
		return new SqlMetaDataFactory(manifestResourceStream, internalConnection.ServerVersion, internalConnection.ServerVersion);
	}

	internal override DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
	{
		return new SqlConnectionPoolGroupProviderInfo((SqlConnectionString)connectionOptions);
	}

	internal static SqlConnectionString FindSqlConnectionOptions(SqlConnectionPoolKey key)
	{
		SqlConnectionString sqlConnectionString = (SqlConnectionString)SingletonInstance.FindConnectionOptions(key);
		if (sqlConnectionString == null)
		{
			sqlConnectionString = new SqlConnectionString(key.ConnectionString);
		}
		if (sqlConnectionString.IsEmpty)
		{
			throw ADP.NoConnectionString();
		}
		return sqlConnectionString;
	}

	private SqlInternalConnectionSmi GetContextConnection(SqlConnectionString options, object providerInfo)
	{
		SmiContext currentContext = SmiContextFactory.Instance.GetCurrentContext();
		SqlInternalConnectionSmi sqlInternalConnectionSmi = (SqlInternalConnectionSmi)currentContext.GetContextValue(0);
		if (sqlInternalConnectionSmi == null || sqlInternalConnectionSmi.IsConnectionDoomed)
		{
			sqlInternalConnectionSmi?.Dispose();
			sqlInternalConnectionSmi = new SqlInternalConnectionSmi(options, currentContext);
			currentContext.SetContextValue(0, sqlInternalConnectionSmi);
		}
		sqlInternalConnectionSmi.Activate();
		return sqlInternalConnectionSmi;
	}

	internal override DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection)
	{
		if (connection is SqlConnection sqlConnection)
		{
			return sqlConnection.PoolGroup;
		}
		return null;
	}

	internal override DbConnectionInternal GetInnerConnection(DbConnection connection)
	{
		if (connection is SqlConnection sqlConnection)
		{
			return sqlConnection.InnerConnection;
		}
		return null;
	}

	protected override int GetObjectId(DbConnection connection)
	{
		if (connection is SqlConnection sqlConnection)
		{
			return sqlConnection.ObjectID;
		}
		return 0;
	}

	internal override void PermissionDemand(DbConnection outerConnection)
	{
		if (outerConnection is SqlConnection sqlConnection)
		{
			sqlConnection.PermissionDemand();
		}
	}

	internal override void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup)
	{
		if (outerConnection is SqlConnection sqlConnection)
		{
			sqlConnection.PoolGroup = poolGroup;
		}
	}

	internal override void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to)
	{
		if (owningObject is SqlConnection sqlConnection)
		{
			sqlConnection.SetInnerConnectionEvent(to);
		}
	}

	internal override bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from)
	{
		if (owningObject is SqlConnection sqlConnection)
		{
			return sqlConnection.SetInnerConnectionFrom(to, from);
		}
		return false;
	}

	internal override void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to)
	{
		if (owningObject is SqlConnection sqlConnection)
		{
			sqlConnection.SetInnerConnectionTo(to);
		}
	}
}
