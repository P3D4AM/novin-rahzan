using System;
using System.ComponentModel;
using Stimulsoft.Report.Dictionary.Adapters.Google;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.DataSources.Google;

[TypeConverter(typeof(StiBigQuerySourceConverter))]
public class StiBigQuerySource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiBigQueryDatabase;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiBigQueryAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiBigQuerySource();
	}

	public StiBigQuerySource()
		: base("", "", "", null)
	{
	}

	public StiBigQuerySource(string nameInSource, string name)
		: base(nameInSource, name)
	{
	}

	public StiBigQuerySource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiBigQuerySource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiBigQuerySource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiBigQuerySource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiBigQuerySource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiBigQuerySource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
