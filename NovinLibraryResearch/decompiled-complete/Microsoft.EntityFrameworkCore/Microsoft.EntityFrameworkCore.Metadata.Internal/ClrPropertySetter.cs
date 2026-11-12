using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public sealed class ClrPropertySetter<TEntity, TValue> : IClrPropertySetter where TEntity : class
{
	private readonly Action<TEntity, TValue> _setter;

	public ClrPropertySetter([JetBrains.Annotations.NotNull] Action<TEntity, TValue> setter)
	{
		_setter = setter;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetClrValue(object instance, object value)
	{
		_setter((TEntity)instance, (TValue)value);
	}
}
