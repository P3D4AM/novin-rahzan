using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiSybaseDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiSybaseDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiSybaseDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiSybaseConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiSybaseSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiSybaseAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiSybaseConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Uid = " + userName + "; Pwd = " + password;
	}

	public StiSybaseDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiSybaseDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiSybaseDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiSybaseDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiSybaseDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
