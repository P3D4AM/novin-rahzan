using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Internal;

[DebuggerStepThrough]
public static class ExpressionExtensions
{
	public static bool IsNullConstantExpression([JetBrains.Annotations.NotNull] this Expression expression)
	{
		if (RemoveConvert(expression) is ConstantExpression constantExpression)
		{
			return constantExpression.Value == null;
		}
		return false;
	}

	public static IReadOnlyList<PropertyInfo> MatchPropertyAccessList([JetBrains.Annotations.NotNull] this LambdaExpression lambdaExpression, [JetBrains.Annotations.NotNull] Func<Expression, Expression, PropertyInfo> propertyMatcher)
	{
		ParameterExpression parameterExpression = lambdaExpression.Parameters.Single();
		if (RemoveConvert(lambdaExpression.Body) is NewExpression newExpression)
		{
			List<PropertyInfo> list = (from a in newExpression.Arguments
				select propertyMatcher(a, parameterExpression) into p
				where p != null
				select p).ToList();
			if (list.Count == newExpression.Arguments.Count)
			{
				return list;
			}
			return null;
		}
		PropertyInfo propertyInfo = propertyMatcher(lambdaExpression.Body, parameterExpression);
		if (!(propertyInfo != null))
		{
			return null;
		}
		return new PropertyInfo[1] { propertyInfo };
	}

	public static PropertyInfo MatchSimplePropertyAccess([JetBrains.Annotations.NotNull] this Expression parameterExpression, [JetBrains.Annotations.NotNull] Expression propertyAccessExpression)
	{
		IReadOnlyList<PropertyInfo> readOnlyList = parameterExpression.MatchPropertyAccess(propertyAccessExpression);
		if (readOnlyList == null || readOnlyList.Count != 1)
		{
			return null;
		}
		return readOnlyList[0];
	}

	private static IReadOnlyList<PropertyInfo> MatchPropertyAccess(this Expression parameterExpression, Expression propertyAccessExpression)
	{
		List<PropertyInfo> list = new List<PropertyInfo>();
		MemberExpression memberExpression;
		do
		{
			memberExpression = RemoveConvert(propertyAccessExpression).RemoveTypeAs() as MemberExpression;
			if (!(memberExpression?.Member is PropertyInfo item))
			{
				return null;
			}
			list.Insert(0, item);
			propertyAccessExpression = memberExpression.Expression;
		}
		while (RemoveConvert(memberExpression.Expression).RemoveTypeAs() != parameterExpression);
		return list;
	}

	public static Expression RemoveTypeAs([JetBrains.Annotations.CanBeNull] this Expression expression)
	{
		while (expression != null && expression.NodeType == ExpressionType.TypeAs)
		{
			expression = ((UnaryExpression)RemoveConvert(expression)).Operand;
		}
		return expression;
	}

	public static bool IsLogicalOperation([JetBrains.Annotations.NotNull] this Expression expression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(expression, "expression");
		if (expression.NodeType != ExpressionType.AndAlso)
		{
			return expression.NodeType == ExpressionType.OrElse;
		}
		return true;
	}

	public static bool IsEntityQueryable([JetBrains.Annotations.NotNull] this ConstantExpression constantExpression)
	{
		if (constantExpression.Type.GetTypeInfo().IsGenericType)
		{
			return constantExpression.Type.GetGenericTypeDefinition() == typeof(EntityQueryable<>);
		}
		return false;
	}

	public static LambdaExpression GetLambdaOrNull(this Expression expression)
	{
		if (!(expression is LambdaExpression result))
		{
			if (!(expression is UnaryExpression unaryExpression) || expression.NodeType != ExpressionType.Quote)
			{
				return null;
			}
			return (LambdaExpression)unaryExpression.Operand;
		}
		return result;
	}

	private static Expression RemoveConvert(Expression expression)
	{
		if (expression is UnaryExpression unaryExpression && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
		{
			return RemoveConvert(unaryExpression.Operand);
		}
		return expression;
	}
}
