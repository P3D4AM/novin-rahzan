using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiAzureTableStorageSourceConverter))]
public class StiAzureTableStorageSource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiAzureTableStorageSource;

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		throw new NotImplementedException();
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiAzureTableStorageAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiAzureTableStorageSource();
	}

	public StiAzureTableStorageSource()
		: this("", "", "")
	{
	}

	public StiAzureTableStorageSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiAzureTableStorageSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiAzureTableStorageSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiAzureTableStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiAzureTableStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiAzureTableStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiAzureTableStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
