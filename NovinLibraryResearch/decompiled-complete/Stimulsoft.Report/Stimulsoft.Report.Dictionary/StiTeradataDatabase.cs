using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiTeradataDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiTeradataDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiTeradataDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiTeradataConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiTeradataAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "User Id = " + userName + "; Password = " + password;
	}

	public StiTeradataDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiTeradataDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiTeradataDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiTeradataDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiTeradataDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
