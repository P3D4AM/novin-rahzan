namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public enum StatementEventType
{
	NotReusable,
	Reused,
	RejectedByPosition,
	RejectedByState
}
