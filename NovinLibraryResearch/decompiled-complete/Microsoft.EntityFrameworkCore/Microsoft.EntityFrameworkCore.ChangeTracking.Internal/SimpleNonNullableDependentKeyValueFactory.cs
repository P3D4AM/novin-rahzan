using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class SimpleNonNullableDependentKeyValueFactory<TKey> : IDependentKeyValueFactory<TKey>
{
	private readonly PropertyAccessors _propertyAccessors;

	public SimpleNonNullableDependentKeyValueFactory([JetBrains.Annotations.NotNull] PropertyAccessors propertyAccessors)
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
		key = ((Func<InternalEntityEntry, TKey>)_propertyAccessors.CurrentValueGetter)(entry);
		return true;
	}

	public virtual bool TryCreateFromPreStoreGeneratedCurrentValues(InternalEntityEntry entry, out TKey key)
	{
		key = ((Func<InternalEntityEntry, TKey>)_propertyAccessors.PreStoreGeneratedCurrentValueGetter)(entry);
		return true;
	}

	public virtual bool TryCreateFromOriginalValues(InternalEntityEntry entry, out TKey key)
	{
		key = ((Func<InternalEntityEntry, TKey>)_propertyAccessors.OriginalValueGetter)(entry);
		return true;
	}

	public virtual bool TryCreateFromRelationshipSnapshot(InternalEntityEntry entry, out TKey key)
	{
		key = ((Func<InternalEntityEntry, TKey>)_propertyAccessors.RelationshipSnapshotGetter)(entry);
		return true;
	}
}
