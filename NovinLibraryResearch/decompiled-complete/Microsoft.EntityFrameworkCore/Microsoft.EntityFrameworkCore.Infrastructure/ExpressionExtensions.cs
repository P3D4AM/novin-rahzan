using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Versioning;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class ExpressionExtensions
{
	private static readonly Type _assignBinaryExpressionType = typeof(Expression).Assembly.GetType("System.Linq.Expressions.AssignBinaryExpression");

	private static readonly MethodInfo _fieldInfoSetValueMethod = typeof(FieldInfo).GetTypeInfo().GetDeclaredMethods("SetValue").Single((MethodInfo m) => m.GetParameters().Length == 2);

	public static string Print([JetBrains.Annotations.NotNull] this Expression expression, int? characterLimit = null)
	{
		return new ExpressionPrinter().Print(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(expression, "expression"), characterLimit);
	}

	public static MemberExpression MakeMemberAccess([JetBrains.Annotations.CanBeNull] this Expression expression, [JetBrains.Annotations.NotNull] MemberInfo member)
	{
		Type declaringType = member.DeclaringType;
		if (expression != null && declaringType != expression.Type && expression.Type.GetTypeInfo().IsAssignableFrom(declaringType.GetTypeInfo()))
		{
			expression = Expression.Convert(expression, declaringType);
		}
		return Expression.MakeMemberAccess(expression, member);
	}

	public static Expression Assign([JetBrains.Annotations.NotNull] this MemberExpression memberExpression, [JetBrains.Annotations.NotNull] Expression valueExpression)
	{
		if (memberExpression.Member is FieldInfo { IsInitOnly: not false } fieldInfo)
		{
			if (new FrameworkName(AppContext.TargetFrameworkName).Identifier == ".NETFramework")
			{
				return Expression.Call(Expression.Constant(fieldInfo), _fieldInfoSetValueMethod, memberExpression.Expression, Expression.Convert(valueExpression, typeof(object)));
			}
			return (BinaryExpression)Activator.CreateInstance(_assignBinaryExpressionType, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[2] { memberExpression, valueExpression }, null);
		}
		return Expression.Assign(memberExpression, valueExpression);
	}

	public static bool TryGetEFPropertyArguments([JetBrains.Annotations.NotNull] this MethodCallExpression methodCallExpression, out Expression entityExpression, out string propertyName)
	{
		if (methodCallExpression.Method.IsEFPropertyMethod() && methodCallExpression.Arguments[1] is ConstantExpression constantExpression)
		{
			entityExpression = methodCallExpression.Arguments[0];
			propertyName = (string)constantExpression.Value;
			return true;
		}
		entityExpression = null;
		propertyName = null;
		return false;
	}

	public static PropertyInfo GetPropertyAccess([JetBrains.Annotations.NotNull] this LambdaExpression propertyAccessExpression)
	{
		ParameterExpression parameterExpression = propertyAccessExpression.Parameters.Single();
		PropertyInfo propertyInfo = parameterExpression.MatchSimplePropertyAccess(propertyAccessExpression.Body);
		if (propertyInfo == null)
		{
			throw new ArgumentException(CoreStrings.InvalidPropertyExpression(propertyAccessExpression), "propertyAccessExpression");
		}
		Type declaringType = propertyInfo.DeclaringType;
		Type type = parameterExpression.Type;
		if (declaringType != null && declaringType != type && declaringType.GetTypeInfo().IsInterface && declaringType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
		{
			MethodInfo propertyGetter = propertyInfo.GetMethod;
			InterfaceMapping runtimeInterfaceMap = type.GetTypeInfo().GetRuntimeInterfaceMap(declaringType);
			int num = Array.FindIndex(runtimeInterfaceMap.InterfaceMethods, (MethodInfo p) => propertyGetter.Equals(p));
			MethodInfo methodInfo = runtimeInterfaceMap.TargetMethods[num];
			foreach (PropertyInfo runtimeProperty in type.GetRuntimeProperties())
			{
				if (methodInfo.Equals(runtimeProperty.GetMethod))
				{
					return runtimeProperty;
				}
			}
		}
		return propertyInfo;
	}

	public static IReadOnlyList<PropertyInfo> GetPropertyAccessList([JetBrains.Annotations.NotNull] this LambdaExpression propertyAccessExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyAccessExpression, "propertyAccessExpression");
		if (propertyAccessExpression.Parameters.Count != 1)
		{
			throw new ArgumentException(CoreStrings.InvalidPropertiesExpression(propertyAccessExpression), "propertyAccessExpression");
		}
		return propertyAccessExpression.MatchPropertyAccessList((Expression p, Expression e) => e.MatchSimplePropertyAccess(p)) ?? throw new ArgumentException(CoreStrings.InvalidPropertiesExpression(propertyAccessExpression), "propertyAccessExpression");
	}

	[Obsolete("Unwrap each convert manually by evaluating how they are used.")]
	public static Expression RemoveConvert([JetBrains.Annotations.CanBeNull] this Expression expression)
	{
		while (expression != null && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
		{
			expression = ((UnaryExpression)expression).Operand.RemoveConvert();
		}
		return expression;
	}
}
