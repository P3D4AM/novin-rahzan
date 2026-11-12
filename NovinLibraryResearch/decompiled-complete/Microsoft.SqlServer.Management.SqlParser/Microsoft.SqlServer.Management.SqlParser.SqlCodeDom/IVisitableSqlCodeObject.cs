namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface IVisitableSqlCodeObject
{
	void Accept(ISqlCodeObjectVisitor visitor);

	T Accept<T>(ISqlCodeObjectVisitor<T> visitor);

	void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context);

	T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context);
}
