using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public abstract class ClrAccessorFactory<TAccessor> where TAccessor : class
{
	private static readonly MethodInfo _genericCreate = typeof(ClrAccessorFactory<TAccessor>).GetTypeInfo().GetDeclaredMethods("CreateGeneric").Single();

	public abstract TAccessor Create([JetBrains.Annotations.NotNull] IPropertyBase property);

	public virtual TAccessor Create([JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		return Create(memberInfo, null);
	}

	protected virtual TAccessor Create(MemberInfo memberInfo, IPropertyBase propertyBase)
	{
		MethodInfo methodInfo = ((propertyBase != null) ? _genericCreate.MakeGenericMethod(propertyBase.DeclaringType.ClrType, propertyBase.ClrType, propertyBase.ClrType.UnwrapNullableType()) : _genericCreate.MakeGenericMethod(memberInfo.DeclaringType, memberInfo.GetMemberType(), memberInfo.GetMemberType().UnwrapNullableType()));
		try
		{
			return (TAccessor)methodInfo.Invoke(this, new object[2] { memberInfo, propertyBase });
		}
		catch (TargetInvocationException ex) when (ex.InnerException != null)
		{
			throw ex.InnerException;
		}
	}

	protected abstract TAccessor CreateGeneric<TEntity, TValue, TNonNullableEnumValue>([JetBrains.Annotations.CanBeNull] MemberInfo memberInfo, [JetBrains.Annotations.CanBeNull] IPropertyBase propertyBase) where TEntity : class;
}
