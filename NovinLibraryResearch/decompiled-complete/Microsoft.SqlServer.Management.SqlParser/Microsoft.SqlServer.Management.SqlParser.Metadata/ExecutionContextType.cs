namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public enum ExecutionContextType
{
	Caller,
	ExecuteAsLogin,
	ExecuteAsUser,
	Owner,
	Self
}
