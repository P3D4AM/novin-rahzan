using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class CompositePrincipalKeyValueFactory : CompositeValueFactory, IPrincipalKeyValueFactory<object[]>
{
	public virtual IEqualityComparer<object[]> EqualityComparer { get; }

	public CompositePrincipalKeyValueFactory([JetBrains.Annotations.NotNull] IKey key)
		: base(key.Properties)
	{
		EqualityComparer = CompositeValueFactory.CreateEqualityComparer(key.Properties);
	}

	public virtual object CreateFromKeyValues(object[] keyValues)
	{
		if (!keyValues.Any((object v) => v == null))
		{
			return keyValues;
		}
		return null;
	}

	public virtual object CreateFromBuffer(ValueBuffer valueBuffer)
	{
		if (!TryCreateFromBuffer(in valueBuffer, out var key))
		{
			return null;
		}
		return key;
	}

	public virtual IProperty FindNullPropertyInKeyValues(object[] keyValues)
	{
		int index = -1;
		for (int i = 0; i < keyValues.Length; i++)
		{
			if (keyValues[i] == null)
			{
				index = i;
				break;
			}
		}
		return Properties[index];
	}

	public virtual IProperty FindNullPropertyInValueBuffer(ValueBuffer valueBuffer)
	{
		return Properties.FirstOrDefault((IProperty p) => valueBuffer[p.GetIndex()] == null);
	}

	public virtual object[] CreateFromCurrentValues(InternalEntityEntry entry)
	{
		return CreateFromEntry(entry, (InternalEntityEntry e, IProperty p) => e.GetCurrentValue(p));
	}

	public virtual IProperty FindNullPropertyInCurrentValues(InternalEntityEntry entry)
	{
		return Properties.FirstOrDefault((IProperty p) => entry.GetCurrentValue(p) == null);
	}

	public virtual object[] CreateFromOriginalValues(InternalEntityEntry entry)
	{
		return CreateFromEntry(entry, (InternalEntityEntry e, IProperty p) => e.GetOriginalValue(p));
	}

	public virtual object[] CreateFromRelationshipSnapshot(InternalEntityEntry entry)
	{
		return CreateFromEntry(entry, (InternalEntityEntry e, IProperty p) => e.GetRelationshipSnapshotValue(p));
	}

	private object[] CreateFromEntry(InternalEntityEntry entry, Func<InternalEntityEntry, IProperty, object> getValue)
	{
		object[] array = new object[Properties.Count];
		int num = 0;
		foreach (IProperty property in Properties)
		{
			if ((array[num++] = getValue(entry, property)) == null)
			{
				return null;
			}
		}
		return array;
	}
}
