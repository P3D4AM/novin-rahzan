using System;
using System.Data;

namespace Stimulsoft.Base;

public abstract class StiRestDataConnector : StiSqlDataConnector
{
	public override Type SqlType
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override int DefaultSqlType
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		return null;
	}

	public abstract void FillDataTable(DataTable table, string query);

	public override int GetSqlType(Type type)
	{
		throw new NotSupportedException();
	}

	public override Type GetNetType(string dbType)
	{
		throw new NotSupportedException();
	}

	public override Type GetNetType(int dbType)
	{
		throw new NotSupportedException();
	}

	protected StiRestDataConnector(string connectionString)
		: base(connectionString)
	{
		base.ConnectionString = connectionString;
	}
}
