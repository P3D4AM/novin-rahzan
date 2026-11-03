using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiInformixDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiInformixDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiInformixDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiInformixConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiInformixSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiInformixAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "UID = " + userName + "; Password = " + password;
	}

	public StiInformixDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiInformixDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiInformixDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiInformixDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiInformixDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
