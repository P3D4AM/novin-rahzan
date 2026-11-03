using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Azure;
using Stimulsoft.Report.Dictionary.Adapters.Azure;
using Stimulsoft.Report.Dictionary.DataSources.Azure;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.Databases.Azure;

[TypeConverter(typeof(StiCosmosDbDatabaseConverter))]
public class StiCosmosDbDatabase : StiNoSqlDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCosmosDbDatabase;

	public override StiConnectionType ConnectionType => StiConnectionType.Azure;

	public override StiDatabase CreateNew()
	{
		return new StiCosmosDbDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiCosmosDbConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiCosmosDbAdapterService);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiCosmosDbSource(nameInSource, name);
	}

	public StiCosmosDbDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiCosmosDbDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiCosmosDbDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiCosmosDbDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiCosmosDbDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
