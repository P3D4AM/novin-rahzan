using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Internal;

[DebuggerStepThrough]
public static class EFPropertyExtensions
{
	public static bool TryGetEFIndexerArguments([JetBrains.Annotations.NotNull] this MethodCallExpression methodCallExpression, out Expression entityExpression, out string propertyName)
	{
		if (methodCallExpression.IsEFIndexer() && methodCallExpression.Arguments[0] is ConstantExpression constantExpression)
		{
			entityExpression = methodCallExpression.Object;
			propertyName = (string)constantExpression.Value;
			return true;
		}
		entityExpression = null;
		propertyName = null;
		return false;
	}

	public static bool IsEFIndexer([JetBrains.Annotations.NotNull] this MethodCallExpression methodCallExpression)
	{
		return methodCallExpression.Method.IsEFIndexer();
	}

	public static bool IsEFIndexer(this MethodInfo methodInfo)
	{
		if (!methodInfo.IsStatic && "get_Item".Equals(methodInfo.Name, StringComparison.Ordinal) && typeof(object) == methodInfo.ReturnType)
		{
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters != null && parameters.Count() == 1)
			{
				return typeof(string) == methodInfo.GetParameters().First().ParameterType;
			}
		}
		return false;
	}

	public static Expression CreateKeyAccessExpression([JetBrains.Annotations.NotNull] this Expression target, [JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, bool makeNullable = false)
	{
		if (properties.Count != 1)
		{
			return Expression.New(AnonymousObject.AnonymousObjectCtor, Expression.NewArrayInit(typeof(object), properties.Select((IProperty p) => Expression.Convert(target.CreateEFPropertyExpression(p, makeNullable), typeof(object))).Cast<Expression>().ToArray()));
		}
		return target.CreateEFPropertyExpression(properties[0], makeNullable);
	}

	public static Expression CreateEFPropertyExpression([JetBrains.Annotations.NotNull] this Expression target, [JetBrains.Annotations.NotNull] IPropertyBase property, bool makeNullable = true)
	{
		return CreateEFPropertyExpression(target, property.DeclaringType.ClrType, property.ClrType, property.Name, makeNullable);
	}

	public static Expression CreateEFPropertyExpression([JetBrains.Annotations.NotNull] this Expression target, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		return CreateEFPropertyExpression(target, memberInfo.DeclaringType, memberInfo.GetMemberType(), memberInfo.GetSimpleMemberName(), makeNullable: false);
	}

	private static Expression CreateEFPropertyExpression(Expression target, Type propertyDeclaringType, Type propertyType, string propertyName, bool makeNullable)
	{
		if (propertyDeclaringType != target.Type && target.Type.GetTypeInfo().IsAssignableFrom(propertyDeclaringType.GetTypeInfo()))
		{
			target = Expression.Convert(target, propertyDeclaringType);
		}
		if (makeNullable)
		{
			propertyType = propertyType.MakeNullable();
		}
		return Expression.Call(EF.PropertyMethod.MakeGenericMethod(propertyType), target, Expression.Constant(propertyName));
	}
}
