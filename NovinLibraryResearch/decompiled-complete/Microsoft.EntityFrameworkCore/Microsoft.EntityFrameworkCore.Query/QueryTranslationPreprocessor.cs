using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Query;

public class QueryTranslationPreprocessor
{
	private class EnumerableVerifyingExpressionVisitor : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof(Enumerable) && node.Arguments[0].Type.IsGenericType && node.Arguments[0].Type.GetGenericTypeDefinition() == typeof(IQueryable<>) && !string.Equals(node.Method.Name, "ToList") && !string.Equals(node.Method.Name, "ToArray"))
			{
				throw new InvalidFilterCriteriaException();
			}
			return base.VisitMethodCall(node);
		}
	}

	private readonly QueryCompilationContext _queryCompilationContext;

	protected virtual QueryTranslationPreprocessorDependencies Dependencies { get; }

	public QueryTranslationPreprocessor(QueryTranslationPreprocessorDependencies dependencies, QueryCompilationContext queryCompilationContext)
	{
		Dependencies = dependencies;
		_queryCompilationContext = queryCompilationContext;
	}

	public virtual Expression Process(Expression query)
	{
		query = new EnumerableToQueryableMethodConvertingExpressionVisitor().Visit(query);
		query = new QueryMetadataExtractingExpressionVisitor(_queryCompilationContext).Visit(query);
		query = new InvocationExpressionRemovingExpressionVisitor().Visit(query);
		query = new AllAnyToContainsRewritingExpressionVisitor().Visit(query);
		query = new GroupJoinFlatteningExpressionVisitor().Visit(query);
		query = new NullCheckRemovingExpressionVisitor().Visit(query);
		query = new EntityEqualityRewritingExpressionVisitor(_queryCompilationContext).Rewrite(query);
		query = new SubqueryMemberPushdownExpressionVisitor().Visit(query);
		query = new NavigationExpandingExpressionVisitor(_queryCompilationContext, Dependencies.EvaluatableExpressionFilter).Expand(query);
		query = new FunctionPreprocessingExpressionVisitor().Visit(query);
		new EnumerableVerifyingExpressionVisitor().Visit(query);
		return query;
	}
}
