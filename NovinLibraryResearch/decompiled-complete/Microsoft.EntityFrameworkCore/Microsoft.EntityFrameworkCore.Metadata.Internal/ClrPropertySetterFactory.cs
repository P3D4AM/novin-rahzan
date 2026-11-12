using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ClrPropertySetterFactory : ClrAccessorFactory<IClrPropertySetter>
{
	public override IClrPropertySetter Create(IPropertyBase property)
	{
		return (property as IClrPropertySetter) ?? Create(property.GetMemberInfo(forMaterialization: false, forSet: true), property);
	}

	protected override IClrPropertySetter CreateGeneric<TEntity, TValue, TNonNullableEnumValue>(MemberInfo memberInfo, IPropertyBase propertyBase)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "entity");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(TValue), "value");
		Type memberType = memberInfo.GetMemberType();
		Expression valueExpression = ((memberType == typeof(TValue)) ? ((Expression)parameterExpression2) : ((Expression)Expression.Convert(parameterExpression2, memberType)));
		Expression body;
		if (memberInfo.DeclaringType.GetTypeInfo().IsAssignableFrom(typeof(TEntity).GetTypeInfo()))
		{
			body = Expression.MakeMemberAccess(parameterExpression, memberInfo).Assign(valueExpression);
		}
		else
		{
			ParameterExpression parameterExpression3 = Expression.Variable(memberInfo.DeclaringType, "converted");
			body = Expression.Block(new ParameterExpression[1] { parameterExpression3 }, new List<Expression>
			{
				Expression.Assign(parameterExpression3, Expression.TypeAs(parameterExpression, memberInfo.DeclaringType)),
				Expression.IfThen(Expression.ReferenceNotEqual(parameterExpression3, Expression.Constant(null)), Expression.MakeMemberAccess(parameterExpression3, memberInfo).Assign(valueExpression))
			});
		}
		Action<TEntity, TValue> setter = Expression.Lambda<Action<TEntity, TValue>>(body, new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
		Type type = propertyBase?.ClrType ?? memberInfo.GetMemberType();
		if (!type.IsNullableType() || !type.UnwrapNullableType().GetTypeInfo().IsEnum)
		{
			return new ClrPropertySetter<TEntity, TValue>(setter);
		}
		return new NullableEnumClrPropertySetter<TEntity, TValue, TNonNullableEnumValue>(setter);
	}
}
