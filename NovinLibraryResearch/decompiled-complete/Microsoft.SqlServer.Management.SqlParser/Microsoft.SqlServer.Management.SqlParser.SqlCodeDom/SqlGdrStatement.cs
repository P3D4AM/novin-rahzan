namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlGdrStatement : SqlStatement
{
	internal static SqlGdrStatement Create(SqlGdrStatementType statementType)
	{
		return statementType switch
		{
			SqlGdrStatementType.SqlGrant => new SqlGrantStatement(), 
			SqlGdrStatementType.SqlDeny => new SqlDenyStatement(), 
			SqlGdrStatementType.SqlRevoke => new SqlRevokeStatement(), 
			_ => null, 
		};
	}
}
