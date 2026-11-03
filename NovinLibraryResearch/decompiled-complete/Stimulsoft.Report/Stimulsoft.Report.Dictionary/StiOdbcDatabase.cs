using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiOdbcDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiOdbcDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiOdbcDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiOdbcConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiOdbcSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiOdbcAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiOdbcConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Uid = " + userName + "; Pwd = " + password;
	}

	public StiOdbcDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiOdbcDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiOdbcDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiOdbcDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiOdbcDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
