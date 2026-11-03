using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiSQLiteDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiSQLiteDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiSQLiteDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiSqLiteConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiSQLiteSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiSQLiteAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Password = " + password;
	}

	public StiSQLiteDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiSQLiteDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiSQLiteDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiSQLiteDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiSQLiteDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
