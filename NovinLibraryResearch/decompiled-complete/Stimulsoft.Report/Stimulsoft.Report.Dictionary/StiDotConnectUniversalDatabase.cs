using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiSqlDatabaseConverter))]
public class StiDotConnectUniversalDatabase : StiSqlDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiDotConnectUniversalDatabase;

	public override StiDatabase CreateNew()
	{
		return new StiDotConnectUniversalDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiUniversalDevartConnector.Get(connectionString);
	}

	public override StiSqlSource CreateDataSource(string nameInSource, string name)
	{
		return new StiDotConnectUniversalSource(nameInSource, name);
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDotConnectUniversalAdapterService);
	}

	public override string GetConnectionStringHelper()
	{
		return "StiUniversalDevartConnectionHelper";
	}

	public override string MapUserNameAndPassword(string userName, string password)
	{
		return "User ID = " + userName + "; Password = " + password;
	}

	public StiDotConnectUniversalDatabase()
		: this(string.Empty, string.Empty)
	{
	}

	public StiDotConnectUniversalDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiDotConnectUniversalDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiDotConnectUniversalDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiDotConnectUniversalDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
