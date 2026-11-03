using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlSourceConverter))]
public class StiOdbcSource : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiOdbcSource;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiOdbcConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiOdbcAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiOdbcSource();
	}

	public StiOdbcSource()
		: this("", "", "")
	{
	}

	public StiOdbcSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiOdbcSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiOdbcSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiOdbcSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiOdbcSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiOdbcSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiOdbcSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
