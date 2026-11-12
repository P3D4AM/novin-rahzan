using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class SimpleNullableDependentKeyValueFactory<TKey> : IDependentKeyValueFactory<TKey> where TKey : struct
{
	private readonly PropertyAccessors _propertyAccessors;

	public SimpleNullableDependentKeyValueFactory([JetBrains.Annotations.NotNull] PropertyAccessors propertyAccessors)
	{
		_propertyAccessors = propertyAccessors;
	}

	public virtual bool TryCreateFromBuffer(in ValueBuffer valueBuffer, out TKey key)
	{
		object obj = _propertyAccessors.ValueBufferGetter(valueBuffer);
		if (obj == null)
		{
			key = default(TKey);
			return false;
		}
		key = (TKey)obj;
		return true;
	}

	public virtual bool TryCreateFromCurrentValues(InternalEntityEntry entry, out TKey key)
	{
		return HandleNullableValue(((Func<InternalEntityEntry, TKey?>)_propertyAccessors.CurrentValueGetter)(entry), out key);
	}

	public virtual bool TryCreateFromPreStoreGeneratedCurrentValues(InternalEntityEntry entry, out TKey key)
	{
		return HandleNullableValue(((Func<InternalEntityEntry, TKey?>)_propertyAccessors.PreStoreGeneratedCurrentValueGetter)(entry), out key);
	}

	public virtual bool TryCreateFromOriginalValues(InternalEntityEntry entry, out TKey key)
	{
		return HandleNullableValue(((Func<InternalEntityEntry, TKey?>)_propertyAccessors.OriginalValueGetter)(entry), out key);
	}

	public virtual bool TryCreateFromRelationshipSnapshot(InternalEntityEntry entry, out TKey key)
	{
		return HandleNullableValue(((Func<InternalEntityEntry, TKey?>)_propertyAccessors.RelationshipSnapshotGetter)(entry), out key);
	}

	private static bool HandleNullableValue(TKey? value, out TKey key)
	{
		if (value.HasValue)
		{
			key = value.Value;
			return true;
		}
		key = default(TKey);
		return false;
	}
}
