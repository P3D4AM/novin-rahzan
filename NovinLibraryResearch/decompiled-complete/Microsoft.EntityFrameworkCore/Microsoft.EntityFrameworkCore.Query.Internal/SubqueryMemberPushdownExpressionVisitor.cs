using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class SubqueryMemberPushdownExpressionVisitor : ExpressionVisitor
{
	private static readonly List<MethodInfo> _supportedMethods = new List<MethodInfo>
	{
		QueryableMethods.FirstWithPredicate,
		QueryableMethods.FirstWithoutPredicate,
		QueryableMethods.FirstOrDefaultWithPredicate,
		QueryableMethods.FirstOrDefaultWithoutPredicate,
		QueryableMethods.SingleWithPredicate,
		QueryableMethods.SingleWithoutPredicate,
		QueryableMethods.SingleOrDefaultWithPredicate,
		QueryableMethods.SingleOrDefaultWithoutPredicate,
		QueryableMethods.LastWithPredicate,
		QueryableMethods.LastWithoutPredicate,
		QueryableMethods.LastOrDefaultWithPredicate,
		QueryableMethods.LastOrDefaultWithoutPredicate
	};

	private static readonly IDictionary<MethodInfo, MethodInfo> _predicateLessMethodInfo = new Dictionary<MethodInfo, MethodInfo>
	{
		{
			QueryableMethods.FirstWithPredicate,
			QueryableMethods.FirstWithoutPredicate
		},
		{
			QueryableMethods.FirstOrDefaultWithPredicate,
			QueryableMethods.FirstOrDefaultWithoutPredicate
		},
		{
			QueryableMethods.SingleWithPredicate,
			QueryableMethods.SingleWithoutPredicate
		},
		{
			QueryableMethods.SingleOrDefaultWithPredicate,
			QueryableMethods.SingleOrDefaultWithoutPredicate
		},
		{
			QueryableMethods.LastWithPredicate,
			QueryableMethods.LastWithoutPredicate
		},
		{
			QueryableMethods.LastOrDefaultWithPredicate,
			QueryableMethods.LastOrDefaultWithoutPredicate
		}
	};

	protected override Expression VisitMember(MemberExpression memberExpression)
	{
		Expression expression = Visit(memberExpression.Expression);
		if (expression is MethodCallExpression methodCallExpression && methodCallExpression.Method.IsGenericMethod && _supportedMethods.Contains(methodCallExpression.Method.GetGenericMethodDefinition()))
		{
			return PushdownMember(methodCallExpression, delegate(Expression target, bool nullable)
			{
				MemberExpression memberExpression2 = Expression.MakeMemberAccess(target, memberExpression.Member);
				return (!nullable || memberExpression2.Type.IsNullableType()) ? ((Expression)memberExpression2) : ((Expression)Expression.Convert(memberExpression2, memberExpression2.Type.MakeNullable()));
			}, memberExpression.Type);
		}
		return memberExpression.Update(expression);
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.TryGetEFPropertyArguments(out var entityExpression, out var _))
		{
			entityExpression = Visit(entityExpression);
			if (entityExpression is MethodCallExpression methodCallExpression2 && methodCallExpression2.Method.IsGenericMethod && _supportedMethods.Contains(methodCallExpression2.Method.GetGenericMethodDefinition()))
			{
				return PushdownMember(methodCallExpression2, delegate(Expression target, bool nullable)
				{
					Type type = methodCallExpression.Type;
					if (nullable && !type.IsNullableType())
					{
						type = type.MakeNullable();
					}
					return Expression.Call(EF.PropertyMethod.MakeGenericMethod(type), target, methodCallExpression.Arguments[1]);
				}, methodCallExpression.Type);
			}
		}
		if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericMethodDefinition() == QueryableMethods.AsQueryable && methodCallExpression.Arguments[0] is MemberExpression memberExpression)
		{
			MemberExpression memberExpression2 = memberExpression.Update(Visit(memberExpression.Expression));
			return Expression.Call(QueryableMethods.AsQueryable.MakeGenericMethod(memberExpression2.Type.TryGetSequenceType()), memberExpression2);
		}
		return base.VisitMethodCall(methodCallExpression);
	}

	private Expression PushdownMember(MethodCallExpression methodCallExpression, Func<Expression, bool, Expression> createSelector, Type returnType)
	{
		Expression expression = methodCallExpression.Arguments[0];
		Type type = expression.Type.TryGetSequenceType();
		MethodInfo methodInfo = methodCallExpression.Method.GetGenericMethodDefinition();
		if (methodCallExpression.Arguments.Count == 2)
		{
			expression = Expression.Call(QueryableMethods.Where.MakeGenericMethod(type), expression, methodCallExpression.Arguments[1]);
			methodInfo = _predicateLessMethodInfo[methodInfo];
		}
		if (expression is MethodCallExpression methodCallExpression2 && methodCallExpression2.Method.IsGenericMethod && methodCallExpression2.Method.GetGenericMethodDefinition() == QueryableMethods.Select)
		{
			LambdaExpression lambdaExpression = methodCallExpression2.Arguments[1].UnwrapLambdaFromQuote();
			Expression body = lambdaExpression.Body;
			Expression expression2 = createSelector(body, methodCallExpression.Method.Name.EndsWith("OrDefault"));
			expression = Expression.Call(QueryableMethods.Select.MakeGenericMethod(methodCallExpression2.Arguments[0].Type.TryGetSequenceType(), expression2.Type), methodCallExpression2.Arguments[0], Expression.Quote(Expression.Lambda(expression2, lambdaExpression.Parameters[0])));
			expression = Visit(expression);
		}
		else
		{
			ParameterExpression parameterExpression = Expression.Parameter(type, "s");
			Expression expression3 = createSelector(parameterExpression, methodCallExpression.Method.Name.EndsWith("OrDefault"));
			expression = Expression.Call(QueryableMethods.Select.MakeGenericMethod(type, expression3.Type), expression, Expression.Quote(Expression.Lambda(expression3, parameterExpression)));
		}
		expression = Expression.Call(methodInfo.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression);
		if (!(expression.Type != returnType))
		{
			return expression;
		}
		return Expression.Convert(expression, returnType);
	}
}
