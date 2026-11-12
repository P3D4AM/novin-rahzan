using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query;

public class EntityMaterializerSource : IEntityMaterializerSource
{
	private ConcurrentDictionary<IEntityType, Func<MaterializationContext, object>> _materializers;

	public static readonly MethodInfo TryReadValueMethod = typeof(EntityMaterializerSource).GetTypeInfo().GetDeclaredMethod("TryReadValue");

	private ConcurrentDictionary<IEntityType, Func<MaterializationContext, object>> Materializers => LazyInitializer.EnsureInitialized(ref _materializers, () => new ConcurrentDictionary<IEntityType, Func<MaterializationContext, object>>());

	public EntityMaterializerSource([JetBrains.Annotations.NotNull] EntityMaterializerSourceDependencies dependencies)
	{
	}

	public virtual Expression CreateReadValueExpression(Expression valueBufferExpression, Type type, int index, IPropertyBase property)
	{
		return Expression.Call(TryReadValueMethod.MakeGenericMethod(type), valueBufferExpression, Expression.Constant(index), Expression.Constant(property, typeof(IPropertyBase)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static TValue TryReadValue<TValue>(in ValueBuffer valueBuffer, int index, IPropertyBase property)
	{
		object obj = valueBuffer[index];
		if (obj is TValue)
		{
			return (TValue)obj;
		}
		return default(TValue);
	}

	public virtual Expression CreateMaterializeExpression(IEntityType entityType, string entityInstanceName, Expression materializationContextExpression)
	{
		if (!entityType.HasClrType())
		{
			throw new InvalidOperationException(CoreStrings.NoClrType(entityType.DisplayName()));
		}
		if (entityType.IsAbstract())
		{
			throw new InvalidOperationException(CoreStrings.CannotMaterializeAbstractType(entityType));
		}
		InstantiationBinding instantiationBinding = (InstantiationBinding)entityType["ConstructorBinding"];
		if (instantiationBinding == null)
		{
			ConstructorInfo declaredConstructor = entityType.ClrType.GetDeclaredConstructor(null);
			if (declaredConstructor == null)
			{
				throw new InvalidOperationException(CoreStrings.NoParameterlessConstructor(entityType.DisplayName()));
			}
			instantiationBinding = new ConstructorBinding(declaredConstructor, Array.Empty<ParameterBinding>());
		}
		ParameterBindingInfo bindingInfo = new ParameterBindingInfo(entityType, materializationContextExpression);
		HashSet<IPropertyBase> hashSet = new HashSet<IPropertyBase>(entityType.GetServiceProperties().Cast<IPropertyBase>().Concat(from p in entityType.GetProperties()
			where !p.IsShadowProperty()
			select p));
		foreach (IPropertyBase item in instantiationBinding.ParameterBindings.SelectMany((ParameterBinding p) => p.ConsumedProperties))
		{
			hashSet.Remove(item);
		}
		Expression expression = instantiationBinding.CreateConstructorExpression(bindingInfo);
		if (hashSet.Count == 0)
		{
			return expression;
		}
		ParameterExpression parameterExpression = Expression.Variable(instantiationBinding.RuntimeType, entityInstanceName);
		List<Expression> list = new List<Expression> { Expression.Assign(parameterExpression, expression) };
		MethodCallExpression valueBufferExpression = Expression.Call(materializationContextExpression, MaterializationContext.GetValueBufferMethod);
		foreach (IPropertyBase item2 in hashSet)
		{
			MemberInfo memberInfo = item2.GetMemberInfo(forMaterialization: true, forSet: true);
			Expression valueExpression = ((item2 is IServiceProperty serviceProperty) ? serviceProperty.GetParameterBinding().BindToParameter(bindingInfo) : CreateReadValueExpression(valueBufferExpression, memberInfo.GetMemberType(), item2.GetIndex(), item2));
			list.Add(Expression.MakeMemberAccess(parameterExpression, memberInfo).Assign(valueExpression));
		}
		list.Add(parameterExpression);
		return Expression.Block(new ParameterExpression[1] { parameterExpression }, list);
	}

	public virtual Func<MaterializationContext, object> GetMaterializer(IEntityType entityType)
	{
		return Materializers.GetOrAdd(entityType, delegate(IEntityType e)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(MaterializationContext), "materializationContext");
			return Expression.Lambda<Func<MaterializationContext, object>>(CreateMaterializeExpression(e, "instance", parameterExpression), new ParameterExpression[1] { parameterExpression }).Compile();
		});
	}
}
