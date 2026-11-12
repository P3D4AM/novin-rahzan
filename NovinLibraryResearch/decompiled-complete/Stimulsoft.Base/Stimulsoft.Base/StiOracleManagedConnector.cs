namespace Stimulsoft.Base;

public class StiOracleManagedConnector : StiOracleConnector
{
	public override string[] NuGetPackages => new string[1] { "Oracle.ManagedDataAccess" };

	public override string NuGetVersion => "12.1.24160719";

	public StiOracleManagedConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Oracle.ManagedDataAccess.dll";
		base.TypeConnection = "Oracle.ManagedDataAccess.Client.OracleConnection";
		base.TypeDataAdapter = "Oracle.ManagedDataAccess.Client.OracleDataAdapter";
		base.TypeCommand = "Oracle.ManagedDataAccess.Client.OracleCommand";
		base.TypeParameter = "Oracle.ManagedDataAccess.Client.OracleParameter";
		base.TypeDbType = "Oracle.ManagedDataAccess.Client.OracleDbType";
		base.TypeCommandBuilder = "Oracle.ManagedDataAccess.Client.OracleCommandBuilder";
		base.TypeConnectionStringBuilder = "Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder";
		base.TypeDataSourceEnumerator = "Oracle.ManagedDataAccess.Client.OracleDataSourceEnumerator";
	}
}
