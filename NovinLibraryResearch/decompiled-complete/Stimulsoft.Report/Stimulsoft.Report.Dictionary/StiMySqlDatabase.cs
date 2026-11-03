using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiMySqlDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiMySqlDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiMySqlDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiMySqlConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiMySqlSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiMySqlAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Uid = " + userName + "; Pwd = " + password;
	}

	public StiMySqlDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiMySqlDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiMySqlDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiMySqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiMySqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
