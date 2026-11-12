namespace Microsoft.Data.SqlClient;

internal sealed class SqlFedAuthToken
{
	internal uint dataLen;

	internal byte[] accessToken;

	internal long expirationFileTime;
}
