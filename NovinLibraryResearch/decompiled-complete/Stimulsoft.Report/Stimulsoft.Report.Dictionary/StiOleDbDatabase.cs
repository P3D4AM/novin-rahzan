using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiOleDbDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiOleDbDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiOleDbDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiOleDbConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiOleDbSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiOleDbAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiOleDbConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "UID = " + userName + "; Password = " + password;
	}

	public StiOleDbDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiOleDbDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiOleDbDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiOleDbDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiOleDbDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
