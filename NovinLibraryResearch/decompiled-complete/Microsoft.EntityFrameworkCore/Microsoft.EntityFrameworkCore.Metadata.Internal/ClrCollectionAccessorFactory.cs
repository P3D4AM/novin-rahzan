using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ClrCollectionAccessorFactory
{
	private static readonly MethodInfo _genericCreate = typeof(ClrCollectionAccessorFactory).GetTypeInfo().GetDeclaredMethod("CreateGeneric");

	private static readonly MethodInfo _createAndSet = typeof(ClrCollectionAccessorFactory).GetTypeInfo().GetDeclaredMethod("CreateAndSet");

	private static readonly MethodInfo _create = typeof(ClrCollectionAccessorFactory).GetTypeInfo().GetDeclaredMethod("CreateCollection");

	private static readonly MethodInfo _createAndSetHashSet = typeof(ClrCollectionAccessorFactory).GetTypeInfo().GetDeclaredMethod("CreateAndSetHashSet");

	private static readonly MethodInfo _createHashSet = typeof(ClrCollectionAccessorFactory).GetTypeInfo().GetDeclaredMethod("CreateHashSet");

	private static readonly MethodInfo _createAndSetObservableHashSet = typeof(ClrCollectionAccessorFactory).GetTypeInfo().GetDeclaredMethod("CreateAndSetObservableHashSet");

	private static readonly MethodInfo _createObservableHashSet = typeof(ClrCollectionAccessorFactory).GetTypeInfo().GetDeclaredMethod("CreateObservableHashSet");

	public virtual IClrCollectionAccessor Create([JetBrains.Annotations.NotNull] INavigation navigation)
	{
		if (navigation is IClrCollectionAccessor result)
		{
			return result;
		}
		MemberInfo memberInfo = GetMostDerivedMemberInfo();
		Type memberType = memberInfo.GetMemberType();
		Type type = memberType.TryGetElementType(typeof(IEnumerable<>));
		if (type == null)
		{
			throw new InvalidOperationException(CoreStrings.NavigationBadType(navigation.Name, navigation.DeclaringEntityType.DisplayName(), memberType.ShortDisplayName(), navigation.GetTargetType().DisplayName()));
		}
		if (memberType.IsArray)
		{
			throw new InvalidOperationException(CoreStrings.NavigationArray(navigation.Name, navigation.DeclaringEntityType.DisplayName(), memberType.ShortDisplayName()));
		}
		MethodInfo methodInfo = _genericCreate.MakeGenericMethod(memberInfo.DeclaringType, memberType, type);
		try
		{
			return (IClrCollectionAccessor)methodInfo.Invoke(null, new object[1] { navigation });
		}
		catch (TargetInvocationException ex)
		{
			throw ex.InnerException;
		}
		MemberInfo GetMostDerivedMemberInfo()
		{
			PropertyInfo propertyInfo = navigation.PropertyInfo;
			FieldInfo fieldInfo = navigation.FieldInfo;
			if (!(fieldInfo == null))
			{
				if (!(propertyInfo == null))
				{
					if (!fieldInfo.FieldType.IsAssignableFrom(propertyInfo.PropertyType))
					{
						return fieldInfo;
					}
					return propertyInfo;
				}
				return fieldInfo;
			}
			return propertyInfo;
		}
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static IClrCollectionAccessor CreateGeneric<TEntity, TCollection, TElement>(INavigation navigation) where TEntity : class where TCollection : class, IEnumerable<TElement> where TElement : class
	{
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(TEntity), "entity");
		ParameterExpression valueParameter2 = Expression.Parameter(typeof(TCollection), "collection");
		MemberInfo memberInfo2 = navigation.GetMemberInfo(forMaterialization: false, forSet: false);
		MemberInfo memberInfo3 = navigation.GetMemberInfo(forMaterialization: false, forSet: true);
		MemberInfo memberInfo4 = navigation.GetMemberInfo(forMaterialization: true, forSet: true);
		Expression expression = Expression.MakeMemberAccess(parameterExpression2, memberInfo2);
		if (expression.Type != typeof(TCollection))
		{
			expression = Expression.Convert(expression, typeof(TCollection));
		}
		Func<TEntity, TCollection> getCollection = Expression.Lambda<Func<TEntity, TCollection>>(expression, new ParameterExpression[1] { parameterExpression2 }).Compile();
		Action<TEntity, TCollection> action = null;
		Action<TEntity, TCollection> action2 = null;
		Func<TEntity, Action<TEntity, TCollection>, TCollection> createAndSetCollection = null;
		Func<TCollection> createCollection = null;
		if (memberInfo3 != null)
		{
			action = CreateSetterDelegate(parameterExpression2, memberInfo3, valueParameter2);
		}
		if (memberInfo4 != null)
		{
			action2 = CreateSetterDelegate(parameterExpression2, memberInfo4, valueParameter2);
		}
		Type type = new CollectionTypeFactory().TryFindTypeToInstantiate(typeof(TEntity), typeof(TCollection));
		if (type != null)
		{
			bool flag = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>);
			if (action != null || action2 != null)
			{
				createAndSetCollection = (flag ? ((Func<TEntity, Action<TEntity, TCollection>, TCollection>)_createAndSetHashSet.MakeGenericMethod(typeof(TEntity), typeof(TCollection), typeof(TElement)).CreateDelegate(typeof(Func<TEntity, Action<TEntity, TCollection>, TCollection>))) : ((!IsObservableHashSet(type)) ? ((Func<TEntity, Action<TEntity, TCollection>, TCollection>)_createAndSet.MakeGenericMethod(typeof(TEntity), typeof(TCollection), type).CreateDelegate(typeof(Func<TEntity, Action<TEntity, TCollection>, TCollection>))) : ((Func<TEntity, Action<TEntity, TCollection>, TCollection>)_createAndSetObservableHashSet.MakeGenericMethod(typeof(TEntity), typeof(TCollection), typeof(TElement)).CreateDelegate(typeof(Func<TEntity, Action<TEntity, TCollection>, TCollection>)))));
			}
			createCollection = (flag ? ((Func<TCollection>)_createHashSet.MakeGenericMethod(typeof(TCollection), typeof(TElement)).CreateDelegate(typeof(Func<TCollection>))) : ((!IsObservableHashSet(type)) ? ((Func<TCollection>)_create.MakeGenericMethod(typeof(TCollection), type).CreateDelegate(typeof(Func<TCollection>))) : ((Func<TCollection>)_createObservableHashSet.MakeGenericMethod(typeof(TCollection), typeof(TElement)).CreateDelegate(typeof(Func<TCollection>)))));
		}
		return new ClrICollectionAccessor<TEntity, TCollection, TElement>(navigation.Name, getCollection, action, action2, createAndSetCollection, createCollection);
		static Action<TEntity, TCollection> CreateSetterDelegate(ParameterExpression parameterExpression, MemberInfo memberInfo, ParameterExpression valueParameter1)
		{
			return Expression.Lambda<Action<TEntity, TCollection>>(Expression.MakeMemberAccess(parameterExpression, memberInfo).Assign(Expression.Convert(valueParameter1, memberInfo.GetMemberType())), new ParameterExpression[2] { parameterExpression, valueParameter1 }).Compile();
		}
	}

	private static bool IsObservableHashSet(Type type)
	{
		if (type.IsGenericType)
		{
			return type.GetGenericTypeDefinition() == typeof(ObservableHashSet<>);
		}
		return false;
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static TCollection CreateAndSet<TEntity, TCollection, TConcreteCollection>(TEntity entity, Action<TEntity, TCollection> setterDelegate) where TEntity : class where TCollection : class where TConcreteCollection : TCollection, new()
	{
		TConcreteCollection val = new TConcreteCollection();
		setterDelegate(entity, (TCollection)(object)val);
		return (TCollection)(object)val;
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static TCollection CreateCollection<TCollection, TConcreteCollection>() where TCollection : class where TConcreteCollection : TCollection, new()
	{
		return (TCollection)(object)new TConcreteCollection();
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static TCollection CreateAndSetHashSet<TEntity, TCollection, TElement>(TEntity entity, Action<TEntity, TCollection> setterDelegate) where TEntity : class where TCollection : class where TElement : class
	{
		TCollection val = (TCollection)(object)new HashSet<TElement>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
		setterDelegate(entity, val);
		return val;
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static TCollection CreateHashSet<TCollection, TElement>() where TCollection : class where TElement : class
	{
		return (TCollection)(object)new HashSet<TElement>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static TCollection CreateAndSetObservableHashSet<TEntity, TCollection, TElement>(TEntity entity, Action<TEntity, TCollection> setterDelegate) where TEntity : class where TCollection : class where TElement : class
	{
		TCollection val = (TCollection)(object)new ObservableHashSet<TElement>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
		setterDelegate(entity, val);
		return val;
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static TCollection CreateObservableHashSet<TCollection, TElement>() where TCollection : class where TElement : class
	{
		return (TCollection)(object)new ObservableHashSet<TElement>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
	}
}
