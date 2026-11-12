using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class GroupJoinFlatteningExpressionVisitor : ExpressionVisitor
{
	private class SelectManyVerifyingExpressionVisitor : ExpressionVisitor
	{
		private readonly List<ParameterExpression> _allowedParameters = new List<ParameterExpression>();

		private readonly ISet<string> _allowedMethods = new HashSet<string> { "Where", "AsQueryable" };

		private ParameterExpression _rootParameter;

		private int _rootParameterCount;

		private bool _correlated;

		public bool VerifyCollectionSelector(Expression body, ParameterExpression rootParameter)
		{
			_correlated = false;
			_rootParameterCount = 0;
			_rootParameter = rootParameter;
			Visit(body);
			if (_rootParameterCount == 1)
			{
				Expression expression = body;
				while (expression != null)
				{
					if (expression is MemberExpression memberExpression)
					{
						expression = memberExpression.Expression;
						continue;
					}
					if (expression is MethodCallExpression methodCallExpression && methodCallExpression.Method.DeclaringType == typeof(Queryable))
					{
						expression = methodCallExpression.Arguments[0];
						continue;
					}
					if (expression is ParameterExpression)
					{
						if (expression != _rootParameter)
						{
							_correlated = true;
						}
					}
					else
					{
						_correlated = true;
					}
					break;
				}
			}
			_rootParameter = null;
			return _correlated;
		}

		protected override Expression VisitLambda<T>(Expression<T> lambdaExpression)
		{
			try
			{
				_allowedParameters.AddRange(lambdaExpression.Parameters);
				return base.VisitLambda(lambdaExpression);
			}
			finally
			{
				foreach (ParameterExpression parameter in lambdaExpression.Parameters)
				{
					_allowedParameters.Remove(parameter);
				}
			}
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (_correlated)
			{
				return methodCallExpression;
			}
			if (methodCallExpression.Method.DeclaringType == typeof(Queryable) && !_allowedMethods.Contains(methodCallExpression.Method.Name))
			{
				if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericMethodDefinition() == QueryableMethods.Select)
				{
					LambdaExpression lambdaExpression = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();
					if (lambdaExpression.Body == lambdaExpression.Parameters[0])
					{
						return methodCallExpression;
					}
				}
				_correlated = true;
				return methodCallExpression;
			}
			return base.VisitMethodCall(methodCallExpression);
		}

		protected override Expression VisitParameter(ParameterExpression parameterExpression)
		{
			if (_allowedParameters.Contains(parameterExpression))
			{
				return parameterExpression;
			}
			if (parameterExpression == _rootParameter)
			{
				_rootParameterCount++;
				return parameterExpression;
			}
			_correlated = true;
			return base.VisitParameter(parameterExpression);
		}
	}

	private static readonly EnumerableToQueryableMethodConvertingExpressionVisitor _enumerableToQueryableReMappingExpressionVisitor = new EnumerableToQueryableMethodConvertingExpressionVisitor();

	private readonly SelectManyVerifyingExpressionVisitor _selectManyVerifyingExpressionVisitor = new SelectManyVerifyingExpressionVisitor();

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.Method.DeclaringType == typeof(Queryable) && methodCallExpression.Method.IsGenericMethod)
		{
			MethodInfo genericMethodDefinition = methodCallExpression.Method.GetGenericMethodDefinition();
			if (genericMethodDefinition == QueryableMethods.SelectManyWithCollectionSelector)
			{
				if (methodCallExpression.Arguments[0] is MethodCallExpression methodCallExpression2 && methodCallExpression2.Method.IsGenericMethod && methodCallExpression2.Method.GetGenericMethodDefinition() == QueryableMethods.GroupJoin)
				{
					Expression expression = Visit(methodCallExpression2.Arguments[0]);
					Expression expression2 = Visit(methodCallExpression2.Arguments[1]);
					LambdaExpression lambdaExpression = methodCallExpression2.Arguments[2].UnwrapLambdaFromQuote();
					LambdaExpression lambdaExpression2 = methodCallExpression2.Arguments[3].UnwrapLambdaFromQuote();
					LambdaExpression lambdaExpression3 = methodCallExpression2.Arguments[4].UnwrapLambdaFromQuote();
					LambdaExpression lambdaExpression4 = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();
					LambdaExpression lambdaExpression5 = methodCallExpression.Arguments[2].UnwrapLambdaFromQuote();
					Expression expression3 = lambdaExpression4.Body;
					bool flag = false;
					if (expression3 is MethodCallExpression methodCallExpression3 && methodCallExpression3.Method.IsGenericMethod && methodCallExpression3.Method.GetGenericMethodDefinition() == QueryableMethods.DefaultIfEmptyWithoutArgument)
					{
						flag = true;
						expression3 = methodCallExpression3.Arguments[0];
					}
					expression3 = ReplacingExpressionVisitor.Replace(lambdaExpression4.Parameters[0], lambdaExpression3.Body, expression3);
					bool flag2 = _selectManyVerifyingExpressionVisitor.VerifyCollectionSelector(expression3, lambdaExpression3.Parameters[1]);
					if (flag2)
					{
						ParameterExpression parameterExpression = lambdaExpression.Parameters[0];
						ParameterExpression parameterExpression2 = lambdaExpression2.Parameters[0];
						BinaryExpression body = Expression.Equal(lambdaExpression.Body, lambdaExpression2.Body);
						expression2 = Expression.Call(QueryableMethods.Where.MakeGenericMethod(expression2.Type.TryGetSequenceType()), expression2, Expression.Quote(Expression.Lambda(body, parameterExpression2)));
						expression2 = ReplacingExpressionVisitor.Replace(lambdaExpression3.Parameters[1], expression2, expression3);
						expression2 = Expression.Quote(Expression.Lambda(expression2, parameterExpression));
					}
					else
					{
						expression2 = _enumerableToQueryableReMappingExpressionVisitor.Visit(ReplacingExpressionVisitor.Replace(lambdaExpression3.Parameters[1], expression2, expression3));
						if (expression2 is MethodCallExpression methodCallExpression4 && methodCallExpression4.Method.IsGenericMethod && methodCallExpression4.Method.GetGenericMethodDefinition() == QueryableMethods.AsQueryable && methodCallExpression4.Type == methodCallExpression4.Arguments[0].Type)
						{
							expression2 = methodCallExpression4.Arguments[0];
						}
					}
					LambdaExpression lambdaExpression6 = Expression.Lambda(ReplacingExpressionVisitor.Replace(lambdaExpression5.Parameters[0], lambdaExpression3.Body, lambdaExpression5.Body), lambdaExpression3.Parameters[0], lambdaExpression5.Parameters[1]);
					if (!flag2)
					{
						if (flag)
						{
							return Expression.Call(QueryableExtensions.LeftJoinMethodInfo.MakeGenericMethod(expression.Type.TryGetSequenceType(), expression2.Type.TryGetSequenceType(), lambdaExpression.ReturnType, lambdaExpression6.ReturnType), expression, expression2, lambdaExpression, lambdaExpression2, lambdaExpression6);
						}
						return Expression.Call(QueryableMethods.Join.MakeGenericMethod(expression.Type.TryGetSequenceType(), expression2.Type.TryGetSequenceType(), lambdaExpression.ReturnType, lambdaExpression6.ReturnType), expression, expression2, lambdaExpression, lambdaExpression2, lambdaExpression6);
					}
				}
			}
			else if (genericMethodDefinition == QueryableMethods.SelectManyWithoutCollectionSelector && methodCallExpression.Arguments[0] is MethodCallExpression methodCallExpression5 && methodCallExpression5.Method.IsGenericMethod && methodCallExpression5.Method.GetGenericMethodDefinition() == QueryableMethods.GroupJoin)
			{
				Expression expression4 = Visit(methodCallExpression5.Arguments[0]);
				Expression replacement = Visit(methodCallExpression5.Arguments[1]);
				LambdaExpression lambdaExpression7 = methodCallExpression5.Arguments[2].UnwrapLambdaFromQuote();
				LambdaExpression lambdaExpression8 = methodCallExpression5.Arguments[3].UnwrapLambdaFromQuote();
				LambdaExpression lambdaExpression9 = methodCallExpression5.Arguments[4].UnwrapLambdaFromQuote();
				LambdaExpression lambdaExpression10 = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();
				Expression expression5 = lambdaExpression9.Body;
				bool flag3 = false;
				if (expression5 is MethodCallExpression methodCallExpression6 && methodCallExpression6.Method.IsGenericMethod && methodCallExpression6.Method.GetGenericMethodDefinition() == QueryableMethods.DefaultIfEmptyWithoutArgument)
				{
					flag3 = true;
					expression5 = methodCallExpression6.Arguments[0];
				}
				if (!_selectManyVerifyingExpressionVisitor.VerifyCollectionSelector(expression5, lambdaExpression9.Parameters[1]))
				{
					replacement = ReplacingExpressionVisitor.Replace(lambdaExpression9.Parameters[1], replacement, expression5);
					replacement = ReplacingExpressionVisitor.Replace(lambdaExpression10.Parameters[0], replacement, lambdaExpression10.Body);
					replacement = _enumerableToQueryableReMappingExpressionVisitor.Visit(replacement);
					LambdaExpression lambdaExpression11 = Expression.Lambda(lambdaExpression8.Parameters[0], lambdaExpression9.Parameters[0], lambdaExpression8.Parameters[0]);
					if (flag3)
					{
						return Expression.Call(QueryableExtensions.LeftJoinMethodInfo.MakeGenericMethod(expression4.Type.TryGetSequenceType(), replacement.Type.TryGetSequenceType(), lambdaExpression7.ReturnType, lambdaExpression11.ReturnType), expression4, replacement, lambdaExpression7, lambdaExpression8, lambdaExpression11);
					}
					return Expression.Call(QueryableMethods.Join.MakeGenericMethod(expression4.Type.TryGetSequenceType(), replacement.Type.TryGetSequenceType(), lambdaExpression7.ReturnType, lambdaExpression11.ReturnType), expression4, replacement, lambdaExpression7, lambdaExpression8, lambdaExpression11);
				}
			}
		}
		return base.VisitMethodCall(methodCallExpression);
	}
}
