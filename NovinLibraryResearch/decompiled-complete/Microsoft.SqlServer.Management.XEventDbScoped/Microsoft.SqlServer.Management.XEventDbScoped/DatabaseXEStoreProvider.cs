using System.Collections.Generic;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.XEvent;

namespace Microsoft.SqlServer.Management.XEventDbScoped;

internal class DatabaseXEStoreProvider : IXEStoreProvider
{
	private DatabaseXEStore store;

	private ServerConnection serverConnection;

	private IComparer<string> comparer;

	public string DomainInstanceName => ServerConnection.TrueName;

	private ServerConnection ServerConnection
	{
		get
		{
			if (serverConnection != null)
			{
				return serverConnection;
			}
			if (store.OriginalConnection is SqlStoreConnection sqlStoreConnection)
			{
				serverConnection = sqlStoreConnection.ServerConnection;
			}
			else
			{
				serverConnection = (ServerConnection)store.OriginalConnection;
			}
			comparer = new ServerComparer(serverConnection);
			if (serverConnection.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase && (string.IsNullOrEmpty(serverConnection.DatabaseName) || comparer.Compare(serverConnection.DatabaseName, store.Name) != 0))
			{
				serverConnection = serverConnection.GetDatabaseConnection(store.Name);
			}
			return serverConnection;
		}
	}

	public DatabaseXEStoreProvider(DatabaseXEStore store)
	{
		this.store = store;
	}

	public ISfcExecutionEngine GetExecutionEngine()
	{
		return new SfcTSqlExecutionEngine(ServerConnection);
	}

	public ISfcConnection GetConnection(SfcObjectQueryMode mode)
	{
		switch (mode)
		{
		case SfcObjectQueryMode.SingleActiveQuery:
			return ServerConnection;
		case SfcObjectQueryMode.MultipleActiveQueries:
			if (ServerConnection.MultipleActiveResultSets)
			{
				return ServerConnection;
			}
			return null;
		default:
			return null;
		}
	}

	public IComparer<string> GetComparer()
	{
		if (comparer == null)
		{
			_ = ServerConnection.ConnectTimeout;
		}
		return comparer;
	}
}
