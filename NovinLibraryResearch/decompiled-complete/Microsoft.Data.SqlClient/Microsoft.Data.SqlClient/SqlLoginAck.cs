namespace Microsoft.Data.SqlClient;

internal sealed class SqlLoginAck
{
	internal string programName;

	internal byte majorVersion;

	internal byte minorVersion;

	internal short buildNum;

	internal bool isVersion8;

	internal uint tdsVersion;
}
