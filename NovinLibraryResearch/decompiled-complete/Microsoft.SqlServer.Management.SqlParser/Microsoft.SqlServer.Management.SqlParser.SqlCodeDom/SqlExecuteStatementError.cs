namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlExecuteStatementError : SqlStatementError, ISqlErrorObject
{
	internal SqlExecuteStatementError(SqlErrorCodeObject errorCodeObject)
		: base(errorCodeObject)
	{
	}
}
