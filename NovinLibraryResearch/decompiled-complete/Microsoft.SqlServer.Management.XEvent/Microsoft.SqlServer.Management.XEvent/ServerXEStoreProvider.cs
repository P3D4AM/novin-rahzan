using System.Collections.Generic;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

internal class ServerXEStoreProvider : IXEStoreProvider
{
	private BaseXEStore store;

	private ServerConnection serverConnection;

	public string DomainInstanceName => ServerConnection.TrueName;

	private ServerConnection ServerConnection
	{
		get
		{
			if (serverConnection == null)
			{
				if (store.OriginalConnection is SqlStoreConnection sqlStoreConnection)
				{
					serverConnection = sqlStoreConnection.ServerConnection;
				}
				else
				{
					serverConnection = (ServerConnection)store.OriginalConnection;
				}
			}
			return serverConnection;
		}
	}

	public ServerXEStoreProvider(BaseXEStore store)
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
		return new ServerComparer(ServerConnection);
	}
}
