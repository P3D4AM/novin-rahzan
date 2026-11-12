namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlVariableAssignment : SqlAssignment
{
	public SqlVariableAssignment(SqlAssignmentOperatorType operatorType)
		: base(operatorType)
	{
	}
}
