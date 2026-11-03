using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiOracleDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiOracleDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiOracleDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiOracleConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiOracleSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiOracleAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiOracleConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "User Id = " + userName + "; Password = " + password;
	}

	public StiOracleDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiOracleDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiOracleDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiOracleDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiOracleDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
