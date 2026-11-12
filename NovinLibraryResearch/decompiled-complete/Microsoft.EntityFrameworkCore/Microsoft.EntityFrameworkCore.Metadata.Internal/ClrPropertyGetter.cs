using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public sealed class ClrPropertyGetter<TEntity, TValue> : IClrPropertyGetter where TEntity : class
{
	private readonly Func<TEntity, TValue> _getter;

	private readonly Func<TEntity, bool> _hasDefaultValue;

	public ClrPropertyGetter([JetBrains.Annotations.NotNull] Func<TEntity, TValue> getter, [JetBrains.Annotations.NotNull] Func<TEntity, bool> hasDefaultValue)
	{
		_getter = getter;
		_hasDefaultValue = hasDefaultValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public object GetClrValue(object entity)
	{
		return _getter((TEntity)entity);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasDefaultValue(object entity)
	{
		return _hasDefaultValue((TEntity)entity);
	}
}
