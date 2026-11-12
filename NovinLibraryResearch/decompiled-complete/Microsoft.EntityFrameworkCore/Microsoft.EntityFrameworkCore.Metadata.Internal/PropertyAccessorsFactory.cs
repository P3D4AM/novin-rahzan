using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PropertyAccessorsFactory
{
	private static readonly MethodInfo _genericCreate = typeof(PropertyAccessorsFactory).GetTypeInfo().GetDeclaredMethod("CreateGeneric");

	public virtual PropertyAccessors Create([JetBrains.Annotations.NotNull] IPropertyBase propertyBase)
	{
		return (PropertyAccessors)_genericCreate.MakeGenericMethod(propertyBase.ClrType).Invoke(null, new object[1] { propertyBase });
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static PropertyAccessors CreateGeneric<TProperty>(IPropertyBase propertyBase)
	{
		IProperty property = propertyBase as IProperty;
		return new PropertyAccessors(CreateCurrentValueGetter<TProperty>(propertyBase, useStoreGeneratedValues: true), CreateCurrentValueGetter<TProperty>(propertyBase, useStoreGeneratedValues: false), (property == null) ? null : CreateOriginalValueGetter<TProperty>(property), CreateRelationshipSnapshotGetter<TProperty>(propertyBase), (property == null) ? null : CreateValueBufferGetter(property));
	}

	private static Func<InternalEntityEntry, TProperty> CreateCurrentValueGetter<TProperty>(IPropertyBase propertyBase, bool useStoreGeneratedValues)
	{
		Type clrType = propertyBase.DeclaringType.ClrType;
		ParameterExpression parameterExpression = Expression.Parameter(typeof(InternalEntityEntry), "entry");
		int shadowIndex = propertyBase.GetShadowIndex();
		Expression expression;
		if (shadowIndex >= 0)
		{
			expression = Expression.Call(parameterExpression, InternalEntityEntry.ReadShadowValueMethod.MakeGenericMethod(typeof(TProperty)), Expression.Constant(shadowIndex));
		}
		else
		{
			expression = Expression.MakeMemberAccess(Expression.Convert(Expression.Property(parameterExpression, "Entity"), clrType), propertyBase.GetMemberInfo(forMaterialization: false, forSet: false));
			if (expression.Type != typeof(TProperty))
			{
				expression = Expression.Convert(expression, typeof(TProperty));
			}
		}
		int storeGeneratedIndex = propertyBase.GetStoreGeneratedIndex();
		if (storeGeneratedIndex >= 0)
		{
			if (useStoreGeneratedValues)
			{
				expression = Expression.Condition(Expression.Equal(expression, Expression.Constant(default(TProperty), typeof(TProperty))), Expression.Call(parameterExpression, InternalEntityEntry.ReadStoreGeneratedValueMethod.MakeGenericMethod(typeof(TProperty)), Expression.Constant(storeGeneratedIndex)), expression);
			}
			expression = Expression.Condition(Expression.Equal(expression, Expression.Constant(default(TProperty), typeof(TProperty))), Expression.Call(parameterExpression, InternalEntityEntry.ReadTemporaryValueMethod.MakeGenericMethod(typeof(TProperty)), Expression.Constant(storeGeneratedIndex)), expression);
		}
		return Expression.Lambda<Func<InternalEntityEntry, TProperty>>(expression, new ParameterExpression[1] { parameterExpression }).Compile();
	}

	private static Func<InternalEntityEntry, TProperty> CreateOriginalValueGetter<TProperty>(IProperty property)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(InternalEntityEntry), "entry");
		int originalValueIndex = property.GetOriginalValueIndex();
		return Expression.Lambda<Func<InternalEntityEntry, TProperty>>((originalValueIndex >= 0) ? ((Expression)Expression.Call(parameterExpression, InternalEntityEntry.ReadOriginalValueMethod.MakeGenericMethod(typeof(TProperty)), Expression.Constant(property), Expression.Constant(originalValueIndex))) : ((Expression)Expression.Block(Expression.Throw(Expression.Constant(new InvalidOperationException(CoreStrings.OriginalValueNotTracked(property.Name, property.DeclaringEntityType.DisplayName())))), Expression.Constant(default(TProperty), typeof(TProperty)))), new ParameterExpression[1] { parameterExpression }).Compile();
	}

	private static Func<InternalEntityEntry, TProperty> CreateRelationshipSnapshotGetter<TProperty>(IPropertyBase propertyBase)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(InternalEntityEntry), "entry");
		int num = (propertyBase as IProperty)?.GetRelationshipIndex() ?? (-1);
		return Expression.Lambda<Func<InternalEntityEntry, TProperty>>((num >= 0) ? Expression.Call(parameterExpression, InternalEntityEntry.ReadRelationshipSnapshotValueMethod.MakeGenericMethod(typeof(TProperty)), Expression.Constant(propertyBase), Expression.Constant(num)) : Expression.Call(parameterExpression, InternalEntityEntry.GetCurrentValueMethod.MakeGenericMethod(typeof(TProperty)), Expression.Constant(propertyBase)), new ParameterExpression[1] { parameterExpression }).Compile();
	}

	private static Func<ValueBuffer, object> CreateValueBufferGetter(IProperty property)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(ValueBuffer), "valueBuffer");
		return Expression.Lambda<Func<ValueBuffer, object>>(Expression.Call(parameterExpression, ValueBuffer.GetValueMethod, Expression.Constant(property.GetIndex())), new ParameterExpression[1] { parameterExpression }).Compile();
	}
}
