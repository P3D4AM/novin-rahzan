using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerQueryTranslationPostprocessor : RelationalQueryTranslationPostprocessor
{
	public SqlServerQueryTranslationPostprocessor(QueryTranslationPostprocessorDependencies dependencies, RelationalQueryTranslationPostprocessorDependencies relationalDependencies, QueryCompilationContext queryCompilationContext)
		: base(dependencies, relationalDependencies, queryCompilationContext)
	{
	}

	public override Expression Process(Expression query)
	{
		query = base.Process(query);
		query = new SearchConditionConvertingExpressionVisitor(SqlExpressionFactory).Visit(query);
		return query;
	}
}
