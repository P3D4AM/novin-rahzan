using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Adapters.Azure;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.DataSources.Azure;

[TypeConverter(typeof(StiCosmosDbSourceConverter))]
public class StiCosmosDbSource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiCosmosDbSource;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		throw new NotImplementedException();
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiCosmosDbAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiCosmosDbSource();
	}

	public StiCosmosDbSource()
		: this("", "", "")
	{
	}

	public StiCosmosDbSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiCosmosDbSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiCosmosDbSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiCosmosDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiCosmosDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiCosmosDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiCosmosDbSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
