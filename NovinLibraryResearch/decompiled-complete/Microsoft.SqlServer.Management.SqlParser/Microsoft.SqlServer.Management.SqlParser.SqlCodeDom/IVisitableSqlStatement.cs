namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface IVisitableSqlStatement : IVisitableSqlCodeObject
{
	void Accept(ISqlStatementVisitor visitor);

	T Accept<T>(ISqlStatementVisitor<T> visitor);

	void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context);

	T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context);
}
