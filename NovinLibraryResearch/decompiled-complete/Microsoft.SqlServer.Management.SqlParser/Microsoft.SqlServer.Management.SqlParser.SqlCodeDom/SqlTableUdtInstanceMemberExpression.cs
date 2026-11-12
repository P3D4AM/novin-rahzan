namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlTableUdtInstanceMemberExpression : SqlTableUdtMemberExpression
{
	private readonly SqlScalarExpression udtInstance;

	public SqlScalarExpression UdtInstance => udtInstance;

	internal SqlTableUdtInstanceMemberExpression(SqlScalarExpression instance, SqlIdentifier memberName)
		: base(memberName)
	{
		udtInstance = instance;
	}
}
