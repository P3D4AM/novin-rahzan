using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiAzureTableStorageDatabaseConverter))]
public class StiAzureTableStorageDatabase : StiNoSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.Azure;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiAzureTableStorageDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiAzureTableStorageDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiAzureTableStorageConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiAzureTableStorageAdapterService);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiAzureTableStorageSource(nameInSource, name);
	}

	public StiAzureTableStorageDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiAzureTableStorageDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiAzureTableStorageDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiAzureTableStorageDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiAzureTableStorageDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
