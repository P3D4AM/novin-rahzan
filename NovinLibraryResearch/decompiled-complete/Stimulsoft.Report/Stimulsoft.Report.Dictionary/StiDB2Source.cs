using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlSourceConverter))]
public class StiDB2Source : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiDB2Source;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiDb2Connector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDB2AdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiDB2Source();
	}

	public StiDB2Source()
		: this("", "", "")
	{
	}

	public StiDB2Source(string dataName, string name)
		: this(dataName, name, name)
	{
	}

	public StiDB2Source(string dataName, string name, string alias)
		: this(dataName, name, alias, string.Empty)
	{
	}

	public StiDB2Source(string dataName, string name, string alias, string sqlCommand)
		: base(dataName, name, alias, sqlCommand)
	{
	}

	public StiDB2Source(string dataName, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(dataName, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiDB2Source(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiDB2Source(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiDB2Source(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
