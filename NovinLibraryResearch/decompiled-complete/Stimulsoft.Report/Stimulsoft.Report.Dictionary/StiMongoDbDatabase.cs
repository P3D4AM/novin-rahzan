using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiMongoDbDatabaseConverter))]
public class StiMongoDbDatabase : StiNoSqlDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiMongoDbDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiMongoDbDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiMongoDbConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiMongoDbAdapterService);
	}

	public override StiNoSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiMongoDbSource(nameInSource, name);
	}

	public StiMongoDbDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiMongoDbDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiMongoDbDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiMongoDbDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiMongoDbDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
