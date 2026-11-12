namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlTableUdtMemberExpression : SqlTableExpression
{
	private readonly SqlIdentifier memberName;

	public SqlIdentifier MemberName => memberName;

	internal SqlTableUdtMemberExpression(SqlIdentifier memberName)
	{
		this.memberName = memberName;
	}
}
