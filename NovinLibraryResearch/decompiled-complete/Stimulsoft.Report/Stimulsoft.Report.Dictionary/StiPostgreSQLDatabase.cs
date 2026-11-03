using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiPostgreSQLDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiPostgreSQLDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiPostgreSQLDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiPostgreSqlConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiPostgreSQLSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiPostgreSQLAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "User Id = " + userName + "; Password = " + password;
	}

	public StiPostgreSQLDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiPostgreSQLDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiPostgreSQLDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiPostgreSQLDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiPostgreSQLDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
