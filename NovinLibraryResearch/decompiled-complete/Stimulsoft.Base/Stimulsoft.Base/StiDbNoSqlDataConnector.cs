using System;
using System.Data;

namespace Stimulsoft.Base;

public abstract class StiDbNoSqlDataConnector : StiNoSqlDataConnector, IDbConnection, IDisposable
{
	public int ConnectionTimeout => 30;

	public string Database
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ConnectionState State => ConnectionState.Closed;

	public IDbTransaction BeginTransaction()
	{
		throw new NotImplementedException();
	}

	public IDbTransaction BeginTransaction(IsolationLevel il)
	{
		throw new NotImplementedException();
	}

	public void ChangeDatabase(string databaseName)
	{
		throw new NotImplementedException();
	}

	public void Close()
	{
		throw new NotImplementedException();
	}

	public IDbCommand CreateCommand()
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Open()
	{
		throw new NotImplementedException();
	}

	public StiDbNoSqlDataConnector(string connectionString = "")
		: base(connectionString)
	{
	}
}
