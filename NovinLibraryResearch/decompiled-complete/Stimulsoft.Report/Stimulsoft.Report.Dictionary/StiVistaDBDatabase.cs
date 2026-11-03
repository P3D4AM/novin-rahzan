using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiVistaDBDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiVistaDBDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiVistaDBDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiVistaDbConnector.Get(connectionString);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiVistaDBAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return null;
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Password = " + password;
	}

	public StiVistaDBDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiVistaDBDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiVistaDBDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiVistaDBDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiVistaDBDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
