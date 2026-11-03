using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiFirebirdDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiFirebirdDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiFirebirdDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiFirebirdConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiFirebirdSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiFirebirdAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "User = " + userName + "; Password = " + password;
	}

	public StiFirebirdDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiFirebirdDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiFirebirdDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiFirebirdDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiFirebirdDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
