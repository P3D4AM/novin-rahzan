using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class QueryMetadataExtractingExpressionVisitor : ExpressionVisitor
{
	private readonly QueryCompilationContext _queryCompilationContext;

	public QueryMetadataExtractingExpressionVisitor(QueryCompilationContext queryCompilationContext)
	{
		_queryCompilationContext = queryCompilationContext;
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		MethodInfo method = methodCallExpression.Method;
		if (method.DeclaringType == typeof(EntityFrameworkQueryableExtensions) && method.IsGenericMethod)
		{
			MethodInfo genericMethodDefinition = method.GetGenericMethodDefinition();
			if (genericMethodDefinition == EntityFrameworkQueryableExtensions.AsTrackingMethodInfo || genericMethodDefinition == EntityFrameworkQueryableExtensions.AsNoTrackingMethodInfo)
			{
				Expression? result = Visit(methodCallExpression.Arguments[0]);
				_queryCompilationContext.IsTracking = genericMethodDefinition == EntityFrameworkQueryableExtensions.AsTrackingMethodInfo;
				return result;
			}
			if (genericMethodDefinition == EntityFrameworkQueryableExtensions.TagWithMethodInfo)
			{
				Expression? result2 = Visit(methodCallExpression.Arguments[0]);
				_queryCompilationContext.AddTag((string)((ConstantExpression)methodCallExpression.Arguments[1]).Value);
				return result2;
			}
			if (genericMethodDefinition == EntityFrameworkQueryableExtensions.IgnoreQueryFiltersMethodInfo)
			{
				Expression? result3 = Visit(methodCallExpression.Arguments[0]);
				_queryCompilationContext.IgnoreQueryFilters = true;
				return result3;
			}
		}
		return base.VisitMethodCall(methodCallExpression);
	}
}
