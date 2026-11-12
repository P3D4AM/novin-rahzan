using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalSqlTranslatingExpressionVisitorFactory : IRelationalSqlTranslatingExpressionVisitorFactory
{
	private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

	public RelationalSqlTranslatingExpressionVisitorFactory([JetBrains.Annotations.NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies)
	{
		_dependencies = dependencies;
	}

	public virtual RelationalSqlTranslatingExpressionVisitor Create(IModel model, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
	{
		return new RelationalSqlTranslatingExpressionVisitor(_dependencies, model, queryableMethodTranslatingExpressionVisitor);
	}
}
