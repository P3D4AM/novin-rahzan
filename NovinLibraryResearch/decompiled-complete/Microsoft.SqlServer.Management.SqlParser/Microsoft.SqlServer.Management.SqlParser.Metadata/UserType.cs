namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public enum UserType : sbyte
{
	AsymmetricKey,
	Certificate,
	NoLogin,
	SqlLogin,
	Password,
	External
}
