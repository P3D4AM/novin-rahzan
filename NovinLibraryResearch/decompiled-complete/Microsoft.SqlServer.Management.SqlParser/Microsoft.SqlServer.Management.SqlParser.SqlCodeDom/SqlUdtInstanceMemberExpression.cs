namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlUdtInstanceMemberExpression : SqlUdtMemberExpression
{
	private readonly SqlScalarExpression udtInstance;

	public SqlScalarExpression UdtInstance => udtInstance;

	internal SqlUdtInstanceMemberExpression(SqlScalarExpression instance, SqlIdentifier memberName)
		: base(memberName)
	{
		udtInstance = instance;
	}
}
