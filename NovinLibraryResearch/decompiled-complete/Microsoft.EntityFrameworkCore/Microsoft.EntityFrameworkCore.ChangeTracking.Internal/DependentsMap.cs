using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class DependentsMap<TKey> : IDependentsMap
{
	private readonly IForeignKey _foreignKey;

	private readonly IPrincipalKeyValueFactory<TKey> _principalKeyValueFactory;

	private readonly IDependentKeyValueFactory<TKey> _dependentKeyValueFactory;

	private readonly Dictionary<TKey, HashSet<InternalEntityEntry>> _map;

	public DependentsMap([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IPrincipalKeyValueFactory<TKey> principalKeyValueFactory, [JetBrains.Annotations.NotNull] IDependentKeyValueFactory<TKey> dependentKeyValueFactory)
	{
		_foreignKey = foreignKey;
		_principalKeyValueFactory = principalKeyValueFactory;
		_dependentKeyValueFactory = dependentKeyValueFactory;
		_map = new Dictionary<TKey, HashSet<InternalEntityEntry>>(principalKeyValueFactory.EqualityComparer);
	}

	public virtual void Add(InternalEntityEntry entry)
	{
		if (_foreignKey.DeclaringEntityType.IsAssignableFrom(entry.EntityType) && TryCreateFromCurrentValues(entry, out var key))
		{
			if (!_map.TryGetValue(key, out var value))
			{
				value = new HashSet<InternalEntityEntry>();
				_map[key] = value;
			}
			value.Add(entry);
		}
	}

	public virtual void Remove(InternalEntityEntry entry)
	{
		if (_foreignKey.DeclaringEntityType.IsAssignableFrom(entry.EntityType) && TryCreateFromCurrentValues(entry, out var key) && _map.TryGetValue(key, out var value))
		{
			value.Remove(entry);
		}
	}

	public virtual void Update(InternalEntityEntry entry)
	{
		if (!_foreignKey.DeclaringEntityType.IsAssignableFrom(entry.EntityType))
		{
			return;
		}
		if (_dependentKeyValueFactory.TryCreateFromRelationshipSnapshot(entry, out var key) && _map.TryGetValue(key, out var value))
		{
			value.Remove(entry);
		}
		if (TryCreateFromCurrentValues(entry, out key))
		{
			if (!_map.TryGetValue(key, out value))
			{
				value = new HashSet<InternalEntityEntry>();
				_map[key] = value;
			}
			value.Add(entry);
		}
	}

	private bool TryCreateFromCurrentValues(InternalEntityEntry entry, out TKey key)
	{
		foreach (IProperty property in _foreignKey.Properties)
		{
			if (entry.IsConceptualNull(property))
			{
				key = default(TKey);
				return false;
			}
		}
		return _dependentKeyValueFactory.TryCreateFromCurrentValues(entry, out key);
	}

	public virtual IEnumerable<InternalEntityEntry> GetDependents(InternalEntityEntry principalEntry)
	{
		if (!_map.TryGetValue(_principalKeyValueFactory.CreateFromCurrentValues(principalEntry), out var value))
		{
			return Enumerable.Empty<InternalEntityEntry>();
		}
		return value;
	}

	public virtual IEnumerable<InternalEntityEntry> GetDependentsUsingRelationshipSnapshot(InternalEntityEntry principalEntry)
	{
		if (!_map.TryGetValue(_principalKeyValueFactory.CreateFromRelationshipSnapshot(principalEntry), out var value))
		{
			return Enumerable.Empty<InternalEntityEntry>();
		}
		return value;
	}
}
