namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public enum LoginType : sbyte
{
	AsymmetricKey,
	Certificate,
	Sql,
	Windows,
	External
}
