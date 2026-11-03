using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlSourceConverter))]
public class StiInformixSource : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiInformixSource;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiInformixConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiInformixAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiInformixSource();
	}

	public StiInformixSource()
		: this("", "", "")
	{
	}

	public StiInformixSource(string dataName, string name)
		: this(dataName, name, name)
	{
	}

	public StiInformixSource(string dataName, string name, string alias)
		: this(dataName, name, alias, string.Empty)
	{
	}

	public StiInformixSource(string dataName, string name, string alias, string sqlCommand)
		: base(dataName, name, alias, sqlCommand)
	{
	}

	public StiInformixSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(dataName, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiInformixSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiInformixSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiInformixSource(string dataName, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(dataName, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
