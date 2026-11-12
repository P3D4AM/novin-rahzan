using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class CompositeValueFactory : IDependentKeyValueFactory<object[]>
{
	private sealed class CompositeCustomComparer : IEqualityComparer<object[]>
	{
		private readonly Func<object, object, bool>[] _equals;

		private readonly Func<object, int>[] _hashCodes;

		public CompositeCustomComparer(IList<ValueComparer> comparers)
		{
			_equals = ((IEnumerable<ValueComparer>)comparers).Select((Func<ValueComparer, Func<object, object, bool>>)((ValueComparer c) => c.Equals)).ToArray();
			_hashCodes = ((IEnumerable<ValueComparer>)comparers).Select((Func<ValueComparer, Func<object, int>>)((ValueComparer c) => c.GetHashCode)).ToArray();
		}

		public bool Equals(object[] x, object[] y)
		{
			if (x == y)
			{
				return true;
			}
			if (x.Length != y.Length)
			{
				return false;
			}
			for (int i = 0; i < x.Length; i++)
			{
				if (!_equals[i](x[i], y[i]))
				{
					return false;
				}
			}
			return true;
		}

		public int GetHashCode(object[] obj)
		{
			int num = 0;
			for (int i = 0; i < obj.Length; i++)
			{
				num = (num * 397) ^ _hashCodes[i](obj[i]);
			}
			return num;
		}
	}

	private sealed class CompositeComparer : IEqualityComparer<object[]>
	{
		public bool Equals(object[] x, object[] y)
		{
			if (x == y)
			{
				return true;
			}
			if (x.Length != y.Length)
			{
				return false;
			}
			for (int i = 0; i < x.Length; i++)
			{
				if (!object.Equals(x[i], y[i]))
				{
					return false;
				}
			}
			return true;
		}

		public int GetHashCode(object[] obj)
		{
			HashCode hashCode = default(HashCode);
			foreach (object value in obj)
			{
				hashCode.Add(value);
			}
			return hashCode.ToHashCode();
		}
	}

	private sealed class StructuralCompositeComparer : IEqualityComparer<object[]>
	{
		private readonly IEqualityComparer _structuralEqualityComparer = StructuralComparisons.StructuralEqualityComparer;

		public bool Equals(object[] x, object[] y)
		{
			if (x == y)
			{
				return true;
			}
			if (x.Length != y.Length)
			{
				return false;
			}
			for (int i = 0; i < x.Length; i++)
			{
				if (!_structuralEqualityComparer.Equals(x[i], y[i]))
				{
					return false;
				}
			}
			return true;
		}

		public int GetHashCode(object[] obj)
		{
			int num = 0;
			for (int i = 0; i < obj.Length; i++)
			{
				num = (num * 397) ^ _structuralEqualityComparer.GetHashCode(obj[i]);
			}
			return num;
		}
	}

	protected virtual IReadOnlyList<IProperty> Properties { get; }

	public CompositeValueFactory([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Properties = properties;
	}

	public virtual bool TryCreateFromBuffer(in ValueBuffer valueBuffer, out object[] key)
	{
		key = new object[Properties.Count];
		int num = 0;
		foreach (IProperty property in Properties)
		{
			if ((key[num++] = valueBuffer[property.GetIndex()]) == null)
			{
				key = null;
				return false;
			}
		}
		return true;
	}

	public virtual bool TryCreateFromCurrentValues(InternalEntityEntry entry, out object[] key)
	{
		return TryCreateFromEntry(entry, (InternalEntityEntry e, IProperty p) => e.GetCurrentValue(p), out key);
	}

	public virtual bool TryCreateFromPreStoreGeneratedCurrentValues(InternalEntityEntry entry, out object[] key)
	{
		return TryCreateFromEntry(entry, (InternalEntityEntry e, IProperty p) => e.GetPreStoreGeneratedCurrentValue(p), out key);
	}

	public virtual bool TryCreateFromOriginalValues(InternalEntityEntry entry, out object[] key)
	{
		return TryCreateFromEntry(entry, (InternalEntityEntry e, IProperty p) => e.GetOriginalValue(p), out key);
	}

	public virtual bool TryCreateFromRelationshipSnapshot(InternalEntityEntry entry, out object[] key)
	{
		return TryCreateFromEntry(entry, (InternalEntityEntry e, IProperty p) => e.GetRelationshipSnapshotValue(p), out key);
	}

	protected virtual bool TryCreateFromEntry([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] Func<InternalEntityEntry, IProperty, object> getValue, out object[] key)
	{
		key = new object[Properties.Count];
		int num = 0;
		foreach (IProperty property in Properties)
		{
			if ((key[num++] = getValue(entry, property)) == null)
			{
				key = null;
				return false;
			}
		}
		return true;
	}

	protected static IEqualityComparer<object[]> CreateEqualityComparer([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		List<ValueComparer> list = properties.Select(delegate(IProperty p)
		{
			ValueComparer valueComparer = p.GetKeyValueComparer();
			if (valueComparer == null)
			{
				CoreTypeMapping coreTypeMapping = p.FindTypeMapping();
				if (coreTypeMapping == null)
				{
					return (ValueComparer)null;
				}
				valueComparer = coreTypeMapping.KeyComparer;
			}
			return valueComparer;
		}).ToList();
		if (!list.All((ValueComparer c) => c != null))
		{
			if (!properties.Any((IProperty p) => typeof(IStructuralEquatable).GetTypeInfo().IsAssignableFrom(p.ClrType.GetTypeInfo())))
			{
				return new CompositeComparer();
			}
			return new StructuralCompositeComparer();
		}
		return new CompositeCustomComparer(list);
	}
}
