namespace Microsoft.Data.SqlClient;

internal class EnclaveSessionParameters
{
	internal string ServerName { get; set; }

	internal string AttestationUrl { get; set; }

	internal string Database { get; set; }

	internal EnclaveSessionParameters(string serverName, string attestationUrl, string database)
	{
		ServerName = serverName;
		AttestationUrl = attestationUrl;
		Database = database;
	}
}
