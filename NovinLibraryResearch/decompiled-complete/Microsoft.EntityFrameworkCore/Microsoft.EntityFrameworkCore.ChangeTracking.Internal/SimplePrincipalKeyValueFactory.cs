using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class SimplePrincipalKeyValueFactory<TKey> : IPrincipalKeyValueFactory<TKey>
{
	private sealed class NoNullsStructuralEqualityComparer : IEqualityComparer<TKey>
	{
		private readonly IEqualityComparer _comparer = StructuralComparisons.StructuralEqualityComparer;

		public bool Equals(TKey x, TKey y)
		{
			return _comparer.Equals(x, y);
		}

		public int GetHashCode(TKey obj)
		{
			return _comparer.GetHashCode(obj);
		}
	}

	private sealed class NoNullsCustomEqualityComparer : IEqualityComparer<TKey>
	{
		private readonly Func<TKey, TKey, bool> _equals;

		private readonly Func<TKey, int> _hashCode;

		public NoNullsCustomEqualityComparer(ValueComparer comparer)
		{
			if (comparer.Type != typeof(TKey) && comparer.Type == typeof(TKey).UnwrapNullableType())
			{
				comparer = comparer.ToNonNullNullableComparer();
			}
			_equals = (Func<TKey, TKey, bool>)comparer.EqualsExpression.Compile();
			_hashCode = (Func<TKey, int>)comparer.HashCodeExpression.Compile();
		}

		public bool Equals(TKey x, TKey y)
		{
			return _equals(x, y);
		}

		public int GetHashCode(TKey obj)
		{
			return _hashCode(obj);
		}
	}

	private readonly IProperty _property;

	private readonly PropertyAccessors _propertyAccessors;

	public virtual IEqualityComparer<TKey> EqualityComparer { get; }

	public SimplePrincipalKeyValueFactory([JetBrains.Annotations.NotNull] IProperty property)
	{
		_property = property;
		_propertyAccessors = _property.GetPropertyAccessors();
		ValueComparer valueComparer = property.GetKeyValueComparer() ?? property.FindTypeMapping()?.KeyComparer;
		IEqualityComparer<TKey> equalityComparer;
		if (valueComparer == null)
		{
			if (!typeof(IStructuralEquatable).GetTypeInfo().IsAssignableFrom(typeof(TKey).GetTypeInfo()))
			{
				IEqualityComparer<TKey> @default = EqualityComparer<TKey>.Default;
				equalityComparer = @default;
			}
			else
			{
				IEqualityComparer<TKey> @default = new NoNullsStructuralEqualityComparer();
				equalityComparer = @default;
			}
		}
		else
		{
			IEqualityComparer<TKey> @default = new NoNullsCustomEqualityComparer(valueComparer);
			equalityComparer = @default;
		}
		EqualityComparer = equalityComparer;
	}

	public virtual object CreateFromKeyValues(object[] keyValues)
	{
		return keyValues[0];
	}

	public virtual object CreateFromBuffer(ValueBuffer valueBuffer)
	{
		return _propertyAccessors.ValueBufferGetter(valueBuffer);
	}

	public virtual IProperty FindNullPropertyInKeyValues(object[] keyValues)
	{
		return _property;
	}

	public virtual IProperty FindNullPropertyInValueBuffer(ValueBuffer valueBuffer)
	{
		return _property;
	}

	public virtual TKey CreateFromCurrentValues(InternalEntityEntry entry)
	{
		return ((Func<InternalEntityEntry, TKey>)_propertyAccessors.CurrentValueGetter)(entry);
	}

	public virtual IProperty FindNullPropertyInCurrentValues(InternalEntityEntry entry)
	{
		return _property;
	}

	public virtual TKey CreateFromOriginalValues(InternalEntityEntry entry)
	{
		return ((Func<InternalEntityEntry, TKey>)_propertyAccessors.OriginalValueGetter)(entry);
	}

	public virtual TKey CreateFromRelationshipSnapshot(InternalEntityEntry entry)
	{
		return ((Func<InternalEntityEntry, TKey>)_propertyAccessors.RelationshipSnapshotGetter)(entry);
	}
}
