namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlAssignment : SqlCodeObject
{
	private readonly SqlAssignmentOperatorType operatorType;

	public SqlAssignmentOperatorType Operator => operatorType;

	public SqlAssignment(SqlAssignmentOperatorType operatorType)
	{
		this.operatorType = operatorType;
	}
}
