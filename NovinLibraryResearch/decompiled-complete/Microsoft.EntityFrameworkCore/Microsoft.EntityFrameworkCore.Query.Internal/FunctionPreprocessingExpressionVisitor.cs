using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class FunctionPreprocessingExpressionVisitor : ExpressionVisitor
{
	private static readonly MethodInfo _startsWithMethodInfo = typeof(string).GetRuntimeMethod("StartsWith", new Type[1] { typeof(string) });

	private static readonly MethodInfo _endsWithMethodInfo = typeof(string).GetRuntimeMethod("EndsWith", new Type[1] { typeof(string) });

	private static readonly Expression _constantNullString = Expression.Constant(null, typeof(string));

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (_startsWithMethodInfo.Equals(methodCallExpression.Method) || _endsWithMethodInfo.Equals(methodCallExpression.Method))
		{
			if (methodCallExpression.Arguments[0] is ConstantExpression constantExpression && (string)constantExpression.Value == string.Empty)
			{
				return Expression.Constant(true);
			}
			Expression expression = Visit(methodCallExpression.Object);
			Expression expression2 = Visit(methodCallExpression.Arguments[0]);
			BinaryExpression binaryExpression = Expression.AndAlso(Expression.NotEqual(expression, _constantNullString), Expression.AndAlso(Expression.NotEqual(expression2, _constantNullString), methodCallExpression.Update(expression, new Expression[1] { expression2 })));
			if (!(expression2 is ConstantExpression))
			{
				return Expression.OrElse(Expression.Equal(expression2, Expression.Constant(string.Empty)), binaryExpression);
			}
			return binaryExpression;
		}
		return base.VisitMethodCall(methodCallExpression);
	}

	protected override Expression VisitUnary(UnaryExpression unaryExpression)
	{
		if (unaryExpression.NodeType == ExpressionType.Not && unaryExpression.Operand is MethodCallExpression methodCallExpression && (_startsWithMethodInfo.Equals(methodCallExpression.Method) || _endsWithMethodInfo.Equals(methodCallExpression.Method)))
		{
			if (methodCallExpression.Arguments[0] is ConstantExpression constantExpression && (string)constantExpression.Value == string.Empty)
			{
				return Expression.Constant(false);
			}
			Expression expression = Visit(methodCallExpression.Object);
			Expression expression2 = Visit(methodCallExpression.Arguments[0]);
			BinaryExpression binaryExpression = Expression.AndAlso(Expression.NotEqual(expression, _constantNullString), Expression.AndAlso(Expression.NotEqual(expression2, _constantNullString), Expression.Not(methodCallExpression.Update(expression, new Expression[1] { expression2 }))));
			if (!(expression2 is ConstantExpression))
			{
				return Expression.AndAlso(Expression.NotEqual(expression2, Expression.Constant(string.Empty)), binaryExpression);
			}
			return binaryExpression;
		}
		return base.VisitUnary(unaryExpression);
	}
}
