using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RelationalQueryFilterDefiningQueryRewritingConvention : QueryFilterDefiningQueryRewritingConvention
{
	protected class RelationalDbSetAccessRewritingExpressionVisitor : DbSetAccessRewritingExpressionVisitor
	{
		public RelationalDbSetAccessRewritingExpressionVisitor(Type contextType)
			: base(contextType)
		{
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.Method.DeclaringType == typeof(RelationalQueryableExtensions) && (methodCallExpression.Method.Name == "FromSql" || methodCallExpression.Method.Name == "FromSqlRaw" || methodCallExpression.Method.Name == "FromSqlInterpolated"))
			{
				Expression expression = Visit(methodCallExpression.Arguments[0]);
				MethodInfo method = RelationalQueryableExtensions.FromSqlOnQueryableMethodInfo.MakeGenericMethod(expression.Type.GetGenericArguments()[0]);
				switch (methodCallExpression.Method.Name)
				{
				case "FromSqlRaw":
					return Expression.Call(null, method, expression, methodCallExpression.Arguments[1], methodCallExpression.Arguments[2]);
				case "FromSql":
					if (methodCallExpression.Arguments.Count != 2)
					{
						if (methodCallExpression.Arguments.Count == 3)
						{
							return Expression.Call(null, method, expression, Expression.Constant(Expression.Lambda<Func<RawSqlString>>(Expression.Convert(methodCallExpression.Arguments[1], typeof(RawSqlString)), Array.Empty<ParameterExpression>()).Compile()().Format), methodCallExpression.Arguments[2]);
						}
						break;
					}
					goto case "FromSqlInterpolated";
				case "FromSqlInterpolated":
				{
					FormattableString formattableString = Expression.Lambda<Func<FormattableString>>(Expression.Convert(methodCallExpression.Arguments[1], typeof(FormattableString)), Array.Empty<ParameterExpression>()).Compile()();
					return Expression.Call(null, method, expression, Expression.Constant(formattableString.Format), Expression.Constant(formattableString.GetArguments()));
				}
				}
			}
			return base.VisitMethodCall(methodCallExpression);
		}
	}

	public RelationalQueryFilterDefiningQueryRewritingConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		: base(dependencies)
	{
		DbSetAccessRewriter = new RelationalDbSetAccessRewritingExpressionVisitor(Dependencies.ContextType);
	}
}
