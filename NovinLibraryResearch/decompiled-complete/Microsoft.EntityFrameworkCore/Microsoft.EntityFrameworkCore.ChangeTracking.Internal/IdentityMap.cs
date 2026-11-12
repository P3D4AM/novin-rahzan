using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class IdentityMap<TKey> : IIdentityMap
{
	private readonly bool _sensitiveLoggingEnabled;

	private readonly Dictionary<TKey, InternalEntityEntry> _identityMap;

	private readonly IForeignKey[] _foreignKeys;

	private Dictionary<IForeignKey, IDependentsMap> _dependentMaps;

	protected virtual IPrincipalKeyValueFactory<TKey> PrincipalKeyValueFactory { get; }

	public virtual IKey Key { get; }

	public IdentityMap([JetBrains.Annotations.NotNull] IKey key, [JetBrains.Annotations.NotNull] IPrincipalKeyValueFactory<TKey> principalKeyValueFactory, bool sensitiveLoggingEnabled)
	{
		_sensitiveLoggingEnabled = sensitiveLoggingEnabled;
		Key = key;
		PrincipalKeyValueFactory = principalKeyValueFactory;
		_identityMap = new Dictionary<TKey, InternalEntityEntry>(principalKeyValueFactory.EqualityComparer);
		if (key.IsPrimaryKey())
		{
			_foreignKeys = key.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany(EntityTypeExtensions.GetDeclaredForeignKeys).ToArray();
		}
	}

	public virtual bool Contains(in ValueBuffer valueBuffer)
	{
		object obj = PrincipalKeyValueFactory.CreateFromBuffer(valueBuffer);
		if (obj != null)
		{
			return _identityMap.ContainsKey((TKey)obj);
		}
		return false;
	}

	public virtual bool Contains(IForeignKey foreignKey, in ValueBuffer valueBuffer)
	{
		if (foreignKey.GetDependentKeyValueFactory<TKey>().TryCreateFromBuffer(in valueBuffer, out var key))
		{
			return _identityMap.ContainsKey(key);
		}
		return false;
	}

	public virtual InternalEntityEntry TryGetEntry(object[] keyValues)
	{
		object obj = PrincipalKeyValueFactory.CreateFromKeyValues(keyValues);
		if (obj == null || !_identityMap.TryGetValue((TKey)obj, out var value))
		{
			return null;
		}
		return value;
	}

	public virtual InternalEntityEntry TryGetEntry(object[] keyValues, bool throwOnNullKey, out bool hasNullKey)
	{
		object obj = PrincipalKeyValueFactory.CreateFromKeyValues(keyValues);
		if (obj == null)
		{
			if (throwOnNullKey)
			{
				if (Key.IsPrimaryKey())
				{
					throw new InvalidOperationException(CoreStrings.InvalidKeyValue(Key.DeclaringEntityType.DisplayName(), PrincipalKeyValueFactory.FindNullPropertyInKeyValues(keyValues).Name));
				}
				throw new InvalidOperationException(CoreStrings.InvalidAlternateKeyValue(Key.DeclaringEntityType.DisplayName(), PrincipalKeyValueFactory.FindNullPropertyInKeyValues(keyValues).Name));
			}
			hasNullKey = true;
			return null;
		}
		hasNullKey = false;
		try
		{
			InternalEntityEntry value;
			return _identityMap.TryGetValue((TKey)obj, out value) ? value : null;
		}
		catch (InvalidCastException innerException)
		{
			throw new InvalidOperationException(CoreStrings.ErrorMaterializingPropertyInvalidCast(Key.DeclaringEntityType.DisplayName(), Key.Properties.First().Name, typeof(TKey), obj.GetType()), innerException);
		}
	}

	public virtual InternalEntityEntry TryGetEntry(IForeignKey foreignKey, InternalEntityEntry dependentEntry)
	{
		if (!foreignKey.GetDependentKeyValueFactory<TKey>().TryCreateFromCurrentValues(dependentEntry, out var key) || !_identityMap.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	public virtual InternalEntityEntry TryGetEntryUsingPreStoreGeneratedValues(IForeignKey foreignKey, InternalEntityEntry dependentEntry)
	{
		if (!foreignKey.GetDependentKeyValueFactory<TKey>().TryCreateFromPreStoreGeneratedCurrentValues(dependentEntry, out var key) || !_identityMap.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	public virtual InternalEntityEntry TryGetEntryUsingRelationshipSnapshot(IForeignKey foreignKey, InternalEntityEntry dependentEntry)
	{
		if (!foreignKey.GetDependentKeyValueFactory<TKey>().TryCreateFromRelationshipSnapshot(dependentEntry, out var key) || !_identityMap.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	public virtual void AddOrUpdate(InternalEntityEntry entry)
	{
		Add(PrincipalKeyValueFactory.CreateFromCurrentValues(entry), entry, updateDuplicate: true);
	}

	public virtual void Add(InternalEntityEntry entry)
	{
		Add(PrincipalKeyValueFactory.CreateFromCurrentValues(entry), entry);
	}

	public virtual void Add(object[] keyValues, InternalEntityEntry entry)
	{
		Add((TKey)PrincipalKeyValueFactory.CreateFromKeyValues(keyValues), entry);
	}

	protected virtual void Add([JetBrains.Annotations.NotNull] TKey key, [JetBrains.Annotations.NotNull] InternalEntityEntry entry)
	{
		Add(key, entry, updateDuplicate: false);
	}

	private void ThrowIdentityConflict(InternalEntityEntry entry)
	{
		if (entry.EntityType.IsOwned())
		{
			if (_sensitiveLoggingEnabled)
			{
				throw new InvalidOperationException(CoreStrings.IdentityConflictOwnedSensitive(entry.EntityType.DisplayName(), entry.BuildCurrentValuesString(Key.Properties)));
			}
			throw new InvalidOperationException(CoreStrings.IdentityConflictOwned(entry.EntityType.DisplayName(), Key.Properties.Format()));
		}
		if (_sensitiveLoggingEnabled)
		{
			throw new InvalidOperationException(CoreStrings.IdentityConflictSensitive(entry.EntityType.DisplayName(), entry.BuildCurrentValuesString(Key.Properties)));
		}
		throw new InvalidOperationException(CoreStrings.IdentityConflict(entry.EntityType.DisplayName(), Key.Properties.Format()));
	}

	private void Add(TKey key, InternalEntityEntry entry, bool updateDuplicate)
	{
		if (_identityMap.TryGetValue(key, out var value))
		{
			bool flag = entry.EntityState == EntityState.Deleted == (value.EntityState == EntityState.Deleted);
			if (!updateDuplicate)
			{
				if (value == entry)
				{
					return;
				}
				if (flag)
				{
					ThrowIdentityConflict(entry);
				}
				if (value.SharedIdentityEntry != null)
				{
					if (value.SharedIdentityEntry == entry)
					{
						return;
					}
					ThrowIdentityConflict(entry);
				}
			}
			if (!flag)
			{
				entry.SharedIdentityEntry = value;
				value.SharedIdentityEntry = entry;
				if (value.EntityState != EntityState.Deleted)
				{
					return;
				}
			}
		}
		_identityMap[key] = entry;
		if (_dependentMaps == null || _foreignKeys == null)
		{
			return;
		}
		IForeignKey[] foreignKeys = _foreignKeys;
		foreach (IForeignKey key2 in foreignKeys)
		{
			if (_dependentMaps.TryGetValue(key2, out var value2))
			{
				if (value != null)
				{
					value2.Remove(value);
				}
				value2.Add(entry);
			}
		}
	}

	public virtual IDependentsMap GetDependentsMap(IForeignKey foreignKey)
	{
		if (_dependentMaps == null)
		{
			_dependentMaps = new Dictionary<IForeignKey, IDependentsMap>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
		}
		if (!_dependentMaps.TryGetValue(foreignKey, out var value))
		{
			value = foreignKey.CreateDependentsMapFactory();
			foreach (InternalEntityEntry value2 in _identityMap.Values)
			{
				value.Add(value2);
			}
			_dependentMaps[foreignKey] = value;
		}
		return value;
	}

	public virtual IDependentsMap FindDependentsMap(IForeignKey foreignKey)
	{
		if (_dependentMaps == null || !_dependentMaps.TryGetValue(foreignKey, out var value))
		{
			return null;
		}
		return value;
	}

	public virtual void Clear()
	{
		_identityMap?.Clear();
		_dependentMaps?.Clear();
	}

	public virtual void Remove(InternalEntityEntry entry)
	{
		Remove(PrincipalKeyValueFactory.CreateFromCurrentValues(entry), entry);
	}

	public virtual void RemoveUsingRelationshipSnapshot(InternalEntityEntry entry)
	{
		Remove(PrincipalKeyValueFactory.CreateFromRelationshipSnapshot(entry), entry);
	}

	protected virtual void Remove([JetBrains.Annotations.NotNull] TKey key, [JetBrains.Annotations.NotNull] InternalEntityEntry entry)
	{
		InternalEntityEntry internalEntityEntry = null;
		if (entry.SharedIdentityEntry != null)
		{
			internalEntityEntry = entry.SharedIdentityEntry;
			internalEntityEntry.SharedIdentityEntry = null;
			entry.SharedIdentityEntry = null;
			if (internalEntityEntry.EntityState != EntityState.Deleted)
			{
				return;
			}
		}
		if (internalEntityEntry == null)
		{
			_identityMap.Remove(key);
		}
		else
		{
			_identityMap[key] = internalEntityEntry;
		}
		if (_dependentMaps == null || _foreignKeys == null)
		{
			return;
		}
		IForeignKey[] foreignKeys = _foreignKeys;
		foreach (IForeignKey key2 in foreignKeys)
		{
			if (_dependentMaps.TryGetValue(key2, out var value))
			{
				value.Remove(entry);
				if (internalEntityEntry != null)
				{
					value.Add(internalEntityEntry);
				}
			}
		}
	}
}
