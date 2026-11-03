using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiMSAccessDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiMSAccessDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiMSAccessDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiMsAccessConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiMSAccessSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiMSAccessAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiMSAccessConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Password = " + password;
	}

	public StiMSAccessDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiMSAccessDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiMSAccessDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiMSAccessDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiMSAccessDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
