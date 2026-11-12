using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public sealed class KeyValueIndex<TKey> : IKeyValueIndex
{
	private readonly IForeignKey _foreignKey;

	private readonly TKey _keyValue;

	private readonly IEqualityComparer<TKey> _keyComparer;

	private readonly bool _fromOriginalValues;

	public KeyValueIndex([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] TKey keyValue, [JetBrains.Annotations.NotNull] IEqualityComparer<TKey> keyComparer, bool fromOriginalValues)
	{
		_foreignKey = foreignKey;
		_keyValue = keyValue;
		_fromOriginalValues = fromOriginalValues;
		_keyComparer = keyComparer;
	}

	public IKeyValueIndex WithOriginalValuesFlag()
	{
		return new KeyValueIndex<TKey>(_foreignKey, _keyValue, _keyComparer, fromOriginalValues: true);
	}

	private bool Equals(KeyValueIndex<TKey> other)
	{
		if (other._fromOriginalValues == _fromOriginalValues && other._foreignKey == _foreignKey)
		{
			return _keyComparer.Equals(_keyValue, other._keyValue);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj.GetType() == GetType())
				{
					return Equals((KeyValueIndex<TKey>)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(typeof(TKey));
		hashCode.Add(_fromOriginalValues);
		hashCode.Add(_foreignKey);
		hashCode.Add(_keyValue, _keyComparer);
		return hashCode.ToHashCode();
	}
}
