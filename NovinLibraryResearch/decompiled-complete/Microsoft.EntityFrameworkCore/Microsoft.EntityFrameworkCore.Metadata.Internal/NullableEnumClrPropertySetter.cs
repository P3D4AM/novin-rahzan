using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class NullableEnumClrPropertySetter<TEntity, TValue, TNonNullableEnumValue> : IClrPropertySetter where TEntity : class
{
	private readonly Action<TEntity, TValue> _setter;

	public NullableEnumClrPropertySetter([JetBrains.Annotations.NotNull] Action<TEntity, TValue> setter)
	{
		_setter = setter;
	}

	public virtual void SetClrValue(object instance, object value)
	{
		if (value != null)
		{
			value = (TNonNullableEnumValue)value;
		}
		_setter((TEntity)instance, (TValue)value);
	}
}
