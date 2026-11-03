using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiSqlCeDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiSqlCeDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiSqlCeDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiSqlCeConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiSqlCeSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiSqlCeAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Password = " + password;
	}

	public StiSqlCeDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiSqlCeDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiSqlCeDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiSqlCeDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiSqlCeDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
