namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlHint : SqlCodeObject
{
	public virtual bool IsTableHint => false;

	public virtual bool IsIndexHint => false;

	internal virtual SqlTableHint AsSqlTableHint => null;

	internal virtual SqlIndexHint AsSqlIndexHint => null;

	internal static SqlHint Index(SqlLiteralExpression argument)
	{
		return new SqlIndexHint(argument);
	}

	internal static SqlHint Table(string name)
	{
		return new SqlTableHint(name);
	}

	internal static SqlHint Table(SqlIdentifier name)
	{
		return new SqlTableHint(name);
	}
}
