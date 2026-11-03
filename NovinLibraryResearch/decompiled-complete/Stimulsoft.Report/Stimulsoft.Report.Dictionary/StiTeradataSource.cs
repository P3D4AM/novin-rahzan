using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlSourceConverter))]
public class StiTeradataSource : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiTeradataSource;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiTeradataConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiTeradataAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiTeradataSource();
	}

	public StiTeradataSource()
		: this("", "", "")
	{
	}

	public StiTeradataSource(string dataName, string name)
		: this(dataName, name, name)
	{
	}

	public StiTeradataSource(string dataName, string name, string alias)
		: this(dataName, name, alias, string.Empty)
	{
	}

	public StiTeradataSource(string dataName, string name, string alias, string sqlCommand)
		: base(dataName, name, alias, sqlCommand)
	{
	}

	public StiTeradataSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(dataName, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiTeradataSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiTeradataSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiTeradataSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
