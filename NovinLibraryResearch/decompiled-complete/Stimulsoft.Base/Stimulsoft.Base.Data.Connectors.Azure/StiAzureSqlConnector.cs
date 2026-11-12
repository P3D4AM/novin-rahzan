namespace Stimulsoft.Base.Data.Connectors.Azure;

public class StiAzureSqlConnector : StiMsSqlConnector
{
	public override string Name => "Azure SQL";

	public override string GetSampleConnectionString()
	{
		return "Server =<server address>; User ID =<username>; Password =<password>; Initial Catalog =<database name>;";
	}

	public new static StiAzureSqlConnector Get(string connectionString = null)
	{
		return new StiAzureSqlConnector(connectionString);
	}

	public StiAzureSqlConnector(string connectionString = null)
		: base(connectionString)
	{
	}
}
