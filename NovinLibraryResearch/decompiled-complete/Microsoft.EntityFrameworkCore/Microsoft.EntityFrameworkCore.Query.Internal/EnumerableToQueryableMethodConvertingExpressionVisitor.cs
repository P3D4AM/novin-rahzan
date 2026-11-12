using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class EnumerableToQueryableMethodConvertingExpressionVisitor : ExpressionVisitor
{
	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.Method.DeclaringType == typeof(Enumerable))
		{
			if (methodCallExpression.Method.Name == "SequenceEqual")
			{
				return base.VisitMethodCall(methodCallExpression);
			}
			if (methodCallExpression.Arguments.Count > 0 && ClientSource(methodCallExpression.Arguments[0]))
			{
				return base.VisitMethodCall(methodCallExpression);
			}
			Expression[] array = VisitAndConvert(methodCallExpression.Arguments, "VisitMethodCall").ToArray();
			MethodInfo method = methodCallExpression.Method;
			ParameterInfo[] parameters = method.GetParameters();
			Type[] array2 = null;
			if (method.Name == "Min" || method.Name == "Max")
			{
				array2 = new Type[methodCallExpression.Arguments.Count];
				if (!method.IsGenericMethod)
				{
					array2[0] = method.ReturnType;
				}
				else
				{
					Type[] genericArguments = method.GetGenericArguments();
					if (genericArguments.Length == array2.Length)
					{
						array2 = genericArguments;
					}
					else
					{
						array2[0] = genericArguments[0];
						array2[1] = method.ReturnType;
					}
				}
			}
			else if (method.IsGenericMethod)
			{
				array2 = method.GetGenericArguments();
			}
			foreach (MethodInfo declaredMethod in typeof(Queryable).GetTypeInfo().GetDeclaredMethods(methodCallExpression.Method.Name))
			{
				MethodInfo methodInfo = declaredMethod;
				if (methodInfo.IsGenericMethod)
				{
					if (array2 == null || methodInfo.GetGenericArguments().Length != array2.Length)
					{
						continue;
					}
					methodInfo = methodInfo.MakeGenericMethod(array2);
				}
				ParameterInfo[] parameters2 = methodInfo.GetParameters();
				if (parameters.Length != parameters2.Length)
				{
					continue;
				}
				bool flag = true;
				for (int i = 0; i < parameters.Length; i++)
				{
					Type parameterType = parameters[i].ParameterType;
					Type parameterType2 = parameters2[i].ParameterType;
					if (parameterType == parameterType2)
					{
						continue;
					}
					if (CanConvertEnumerableToQueryable(parameterType, parameterType2))
					{
						Expression expression = array[i];
						Type type = expression.Type.TryGetSequenceType();
						if (array[i].Type.TryGetElementType(typeof(List<>)) != null && array[i] is MethodCallExpression methodCallExpression2 && methodCallExpression2.Method.IsGenericMethod && methodCallExpression2.Method.GetGenericMethodDefinition() == EnumerableMethods.ToList)
						{
							type = methodCallExpression2.Method.GetGenericArguments()[0];
							expression = methodCallExpression2.Arguments[0];
						}
						Type type2 = expression.Type.TryGetElementType(typeof(IQueryable<>));
						if (type2 == null || type2 != type)
						{
							array[i] = Expression.Call(QueryableMethods.AsQueryable.MakeGenericMethod(type), expression);
						}
					}
					else if (!parameterType2.IsGenericType || !(parameterType2.GetGenericTypeDefinition() == typeof(Expression<>)) || !(parameterType2.GetGenericArguments()[0] == parameterType))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return Expression.Call(methodInfo, array.Select((Expression arg) => (!(arg is LambdaExpression expression2)) ? arg : Expression.Quote(expression2)));
				}
			}
			return methodCallExpression.Update(Visit(methodCallExpression.Object), array);
		}
		if (methodCallExpression.Method.DeclaringType.IsGenericType && methodCallExpression.Method.DeclaringType.GetGenericTypeDefinition() == typeof(List<>) && string.Equals("Contains", methodCallExpression.Method.Name))
		{
			if (ClientSource(methodCallExpression.Object))
			{
				return base.VisitMethodCall(methodCallExpression);
			}
			Type type3 = methodCallExpression.Method.DeclaringType.GetGenericArguments()[0];
			return Expression.Call(QueryableMethods.Contains.MakeGenericMethod(type3), Expression.Call(QueryableMethods.AsQueryable.MakeGenericMethod(type3), methodCallExpression.Object), methodCallExpression.Arguments[0]);
		}
		return base.VisitMethodCall(methodCallExpression);
	}

	private static bool ClientSource(Expression expression)
	{
		if (!(expression is ConstantExpression) && !(expression is MemberInitExpression) && !(expression is NewExpression))
		{
			return expression is ParameterExpression;
		}
		return true;
	}

	private static bool CanConvertEnumerableToQueryable(Type enumerableType, Type queryableType)
	{
		if (enumerableType == typeof(IEnumerable) && queryableType == typeof(IQueryable))
		{
			return true;
		}
		if (!enumerableType.IsGenericType || !queryableType.IsGenericType || !enumerableType.GetGenericArguments().SequenceEqual(queryableType.GetGenericArguments()))
		{
			return false;
		}
		enumerableType = enumerableType.GetGenericTypeDefinition();
		queryableType = queryableType.GetGenericTypeDefinition();
		if (enumerableType == typeof(IEnumerable<>) && queryableType == typeof(IQueryable<>))
		{
			return true;
		}
		if (enumerableType == typeof(IOrderedEnumerable<>) && queryableType == typeof(IOrderedQueryable<>))
		{
			return true;
		}
		return false;
	}
}
