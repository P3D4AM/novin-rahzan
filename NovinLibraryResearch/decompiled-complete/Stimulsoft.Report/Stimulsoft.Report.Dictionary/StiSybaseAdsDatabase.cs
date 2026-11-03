using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiSybaseAdsDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiSybaseAdsDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiSybaseAdsDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiSybaseAdsConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiSybaseAdsSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiSybaseAdsAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiSybaseAdsConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "Uid = " + userName + "; Pwd = " + password;
	}

	public StiSybaseAdsDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiSybaseAdsDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiSybaseAdsDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiSybaseAdsDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiSybaseAdsDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
