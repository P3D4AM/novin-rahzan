using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerSqlTranslatingExpressionVisitorFactory : IRelationalSqlTranslatingExpressionVisitorFactory
{
	private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

	public SqlServerSqlTranslatingExpressionVisitorFactory([JetBrains.Annotations.NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies)
	{
		_dependencies = dependencies;
	}

	public virtual RelationalSqlTranslatingExpressionVisitor Create(IModel model, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
	{
		return new SqlServerSqlTranslatingExpressionVisitor(_dependencies, model, queryableMethodTranslatingExpressionVisitor);
	}
}
