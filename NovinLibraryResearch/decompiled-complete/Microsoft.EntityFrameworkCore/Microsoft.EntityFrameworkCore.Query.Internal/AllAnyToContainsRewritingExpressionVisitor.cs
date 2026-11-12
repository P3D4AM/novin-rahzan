using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class AllAnyToContainsRewritingExpressionVisitor : ExpressionVisitor
{
	private static bool IsExpressionOfFunc(Type type, int funcGenericArgs = 2)
	{
		if (type.IsGenericType)
		{
			return type.GetGenericArguments().Length == funcGenericArgs;
		}
		return false;
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.Method.IsGenericMethod)
		{
			MethodInfo genericMethodDefinition = methodCallExpression.Method.GetGenericMethodDefinition();
			if ((object)genericMethodDefinition != null && (genericMethodDefinition.Equals(EnumerableMethods.AnyWithPredicate) || genericMethodDefinition.Equals(EnumerableMethods.All)))
			{
				ExpressionType nodeType = methodCallExpression.Arguments[0].NodeType;
				if ((nodeType == ExpressionType.Parameter || nodeType == ExpressionType.Constant) && methodCallExpression.Arguments[1] is LambdaExpression lambdaExpression && TryExtractEqualityOperands(lambdaExpression.Body, out var left, out var right, out var negated) && (left is ParameterExpression || right is ParameterExpression))
				{
					Expression arg = ((left is ParameterExpression) ? right : left);
					if (genericMethodDefinition.Equals(EnumerableMethods.AnyWithPredicate) && !negated)
					{
						MethodInfo method = EnumerableMethods.Contains.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments()[0]);
						return Expression.Call(null, method, methodCallExpression.Arguments[0], arg);
					}
					if (genericMethodDefinition.Equals(EnumerableMethods.All) && negated)
					{
						MethodInfo method2 = EnumerableMethods.Contains.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments()[0]);
						return Expression.Not(Expression.Call(null, method2, methodCallExpression.Arguments[0], arg));
					}
				}
			}
		}
		return base.VisitMethodCall(methodCallExpression);
	}

	private bool TryExtractEqualityOperands(Expression expression, out Expression left, out Expression right, out bool negated)
	{
		left = null;
		right = null;
		negated = false;
		Expression expression2;
		Expression @object;
		if (!(expression is BinaryExpression { NodeType: var nodeType } binaryExpression))
		{
			if (!(expression is MethodCallExpression methodCallExpression))
			{
				if (expression is UnaryExpression { NodeType: ExpressionType.Not } unaryExpression)
				{
					bool result = TryExtractEqualityOperands(unaryExpression.Operand, out left, out right, out negated);
					negated = !negated;
					return result;
				}
			}
			else if (methodCallExpression.Method.Name == "Equals")
			{
				negated = false;
				if (methodCallExpression.Arguments.Count == 1 && methodCallExpression.Object.Type == methodCallExpression.Arguments[0].Type)
				{
					@object = methodCallExpression.Object;
					expression2 = methodCallExpression.Arguments[0];
					left = @object;
					right = expression2;
				}
				else if (methodCallExpression.Arguments.Count == 2 && methodCallExpression.Arguments[0].Type == methodCallExpression.Arguments[1].Type)
				{
					expression2 = methodCallExpression.Arguments[0];
					@object = methodCallExpression.Arguments[1];
					left = expression2;
					right = @object;
				}
				return true;
			}
			return false;
		}
		switch (nodeType)
		{
		case ExpressionType.Equal:
			negated = false;
			break;
		case ExpressionType.NotEqual:
			negated = true;
			break;
		default:
			return false;
		}
		expression2 = binaryExpression.Left;
		@object = binaryExpression.Right;
		left = expression2;
		right = @object;
		return true;
	}
}
