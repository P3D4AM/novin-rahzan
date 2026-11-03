using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Azure;

namespace Stimulsoft.Report.Dictionary.Databases.Azure;

public class StiAzureSqlDatabase : StiSqlDatabase
{
	public override StiConnectionType ConnectionType => StiConnectionType.Azure;

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiAzureSqlConnector.Get(connectionString);
	}

	public override StiDatabase CreateNew()
	{
		return new StiAzureSqlDatabase();
	}

	public override string GetConnectionStringHelper()
	{
		return "StiAzureSqlConnectionHelper";
	}

	public StiAzureSqlDatabase()
	{
	}

	public StiAzureSqlDatabase(string name, string connectionString)
		: base(name, connectionString)
	{
	}

	public StiAzureSqlDatabase(string name, string alias, string connectionString)
		: base(name, alias, connectionString)
	{
	}

	public StiAzureSqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword)
		: base(name, alias, connectionString, promptUserNameAndpassword)
	{
	}

	public StiAzureSqlDatabase(string name, string alias, string connectionString, bool promptUserNameAndpassword, string key)
		: base(name, alias, connectionString, promptUserNameAndpassword, key)
	{
	}
}
