using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ClrPropertyGetterFactory : ClrAccessorFactory<IClrPropertyGetter>
{
	public override IClrPropertyGetter Create(IPropertyBase property)
	{
		return (property as IClrPropertyGetter) ?? Create(property.GetMemberInfo(forMaterialization: false, forSet: false), property);
	}

	protected override IClrPropertyGetter CreateGeneric<TEntity, TValue, TNonNullableEnumValue>(MemberInfo memberInfo, IPropertyBase propertyBase)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "entity");
		Expression expression;
		if (memberInfo.DeclaringType.GetTypeInfo().IsAssignableFrom(typeof(TEntity).GetTypeInfo()))
		{
			expression = Expression.MakeMemberAccess(parameterExpression, memberInfo);
		}
		else
		{
			ParameterExpression parameterExpression2 = Expression.Variable(memberInfo.DeclaringType, "converted");
			expression = Expression.Block(new ParameterExpression[1] { parameterExpression2 }, new List<Expression>
			{
				Expression.Assign(parameterExpression2, Expression.TypeAs(parameterExpression, memberInfo.DeclaringType)),
				Expression.Condition(Expression.ReferenceEqual(parameterExpression2, Expression.Constant(null)), Expression.Default(memberInfo.GetMemberType()), Expression.MakeMemberAccess(parameterExpression2, memberInfo))
			});
		}
		if (expression.Type != typeof(TValue))
		{
			expression = Expression.Convert(expression, typeof(TValue));
		}
		Expression body;
		if (!expression.Type.IsValueType)
		{
			body = Expression.ReferenceEqual(expression, Expression.Constant(null, expression.Type));
		}
		else if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			body = Expression.Not(Expression.Call(expression, expression.Type.GetMethod("get_HasValue")));
		}
		else
		{
			IProperty property = propertyBase as IProperty;
			ValueComparer valueComparer = property?.GetValueComparer() ?? property?.FindTypeMapping()?.Comparer ?? ((ValueComparer)Activator.CreateInstance(typeof(ValueComparer<>).MakeGenericType(typeof(TValue)), new object[1] { false }));
			body = valueComparer.ExtractEqualsBody((valueComparer.Type != typeof(TValue)) ? Expression.Convert(expression, valueComparer.Type) : expression, Expression.Default(valueComparer.Type));
		}
		return new ClrPropertyGetter<TEntity, TValue>(Expression.Lambda<Func<TEntity, TValue>>(expression, new ParameterExpression[1] { parameterExpression }).Compile(), Expression.Lambda<Func<TEntity, bool>>(body, new ParameterExpression[1] { parameterExpression }).Compile());
	}
}
