using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class EntityReferenceMap
{
	private readonly bool _hasSubMap;

	private Dictionary<object, InternalEntityEntry> _detachedReferenceMap;

	private Dictionary<object, InternalEntityEntry> _unchangedReferenceMap;

	private Dictionary<object, InternalEntityEntry> _addedReferenceMap;

	private Dictionary<object, InternalEntityEntry> _modifiedReferenceMap;

	private Dictionary<object, InternalEntityEntry> _deletedReferenceMap;

	private Dictionary<IEntityType, EntityReferenceMap> _dependentTypeReferenceMap;

	public EntityReferenceMap(bool hasSubMap)
	{
		_hasSubMap = hasSubMap;
	}

	public virtual void Update([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState state, EntityState? oldState)
	{
		object obj = entry.Entity ?? entry;
		IEntityType entityType = entry.EntityType;
		if (_hasSubMap && entityType.HasDefiningNavigation())
		{
			if (_dependentTypeReferenceMap == null)
			{
				_dependentTypeReferenceMap = new Dictionary<IEntityType, EntityReferenceMap>();
			}
			if (!_dependentTypeReferenceMap.TryGetValue(entityType, out var value))
			{
				value = new EntityReferenceMap(hasSubMap: false);
				_dependentTypeReferenceMap[entityType] = value;
			}
			value.Update(entry, state, oldState);
			return;
		}
		if (oldState.HasValue)
		{
			Remove(obj, entityType, oldState.Value);
		}
		if (oldState.HasValue && state == EntityState.Detached)
		{
			return;
		}
		switch (state)
		{
		case EntityState.Detached:
			if (_detachedReferenceMap == null)
			{
				_detachedReferenceMap = new Dictionary<object, InternalEntityEntry>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
			}
			_detachedReferenceMap[obj] = entry;
			break;
		case EntityState.Unchanged:
			if (_unchangedReferenceMap == null)
			{
				_unchangedReferenceMap = new Dictionary<object, InternalEntityEntry>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
			}
			_unchangedReferenceMap[obj] = entry;
			break;
		case EntityState.Deleted:
			if (_deletedReferenceMap == null)
			{
				_deletedReferenceMap = new Dictionary<object, InternalEntityEntry>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
			}
			_deletedReferenceMap[obj] = entry;
			break;
		case EntityState.Modified:
			if (_modifiedReferenceMap == null)
			{
				_modifiedReferenceMap = new Dictionary<object, InternalEntityEntry>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
			}
			_modifiedReferenceMap[obj] = entry;
			break;
		case EntityState.Added:
			if (_addedReferenceMap == null)
			{
				_addedReferenceMap = new Dictionary<object, InternalEntityEntry>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
			}
			_addedReferenceMap[obj] = entry;
			break;
		}
	}

	public virtual bool TryGet([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.CanBeNull] IEntityType entityType, [JetBrains.Annotations.CanBeNull] out InternalEntityEntry entry, bool throwOnNonUniqueness)
	{
		entry = null;
		Dictionary<object, InternalEntityEntry> unchangedReferenceMap = _unchangedReferenceMap;
		int num;
		if (unchangedReferenceMap == null || !unchangedReferenceMap.TryGetValue(entity, out entry))
		{
			Dictionary<object, InternalEntityEntry> modifiedReferenceMap = _modifiedReferenceMap;
			if (modifiedReferenceMap == null || !modifiedReferenceMap.TryGetValue(entity, out entry))
			{
				Dictionary<object, InternalEntityEntry> addedReferenceMap = _addedReferenceMap;
				if (addedReferenceMap == null || !addedReferenceMap.TryGetValue(entity, out entry))
				{
					Dictionary<object, InternalEntityEntry> deletedReferenceMap = _deletedReferenceMap;
					if (deletedReferenceMap == null || !deletedReferenceMap.TryGetValue(entity, out entry))
					{
						num = ((_detachedReferenceMap?.TryGetValue(entity, out entry) ?? false) ? 1 : 0);
						goto IL_0072;
					}
				}
			}
		}
		num = 1;
		goto IL_0072;
		IL_0072:
		bool flag = (byte)num != 0;
		if (!flag && _hasSubMap && _dependentTypeReferenceMap != null)
		{
			if (entityType != null)
			{
				if (_dependentTypeReferenceMap.TryGetValue(entityType, out var value))
				{
					return value.TryGet(entity, entityType, out entry, throwOnNonUniqueness);
				}
			}
			else
			{
				Type type = entity.GetType();
				foreach (KeyValuePair<IEntityType, EntityReferenceMap> item in _dependentTypeReferenceMap)
				{
					if (!item.Key.ClrType.IsAssignableFrom(type) || !item.Value.TryGet(entity, entityType, out var entry2, throwOnNonUniqueness))
					{
						continue;
					}
					if (flag)
					{
						if (!throwOnNonUniqueness)
						{
							entry = null;
							return false;
						}
						throw new InvalidOperationException(CoreStrings.AmbiguousDependentEntity(entity.GetType().ShortDisplayName(), ".Reference().TargetEntry"));
					}
					entry = entry2;
					flag = true;
				}
			}
		}
		return flag;
	}

	public virtual int GetCountForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false)
	{
		int num = 0;
		if (added && _addedReferenceMap != null)
		{
			num = _addedReferenceMap.Count;
		}
		if (modified && _modifiedReferenceMap != null)
		{
			num += _modifiedReferenceMap.Count;
		}
		if (deleted && _deletedReferenceMap != null)
		{
			num += _deletedReferenceMap.Count;
		}
		if (unchanged && _unchangedReferenceMap != null)
		{
			num += _unchangedReferenceMap.Count;
		}
		if (_dependentTypeReferenceMap != null)
		{
			foreach (KeyValuePair<IEntityType, EntityReferenceMap> item in _dependentTypeReferenceMap)
			{
				num += item.Value.GetCountForState(added, modified, deleted, unchanged);
			}
		}
		return num;
	}

	public virtual IEnumerable<InternalEntityEntry> GetEntriesForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false)
	{
		bool flag = added && _addedReferenceMap != null && _addedReferenceMap.Count > 0;
		bool flag2 = modified && _modifiedReferenceMap != null && _modifiedReferenceMap.Count > 0;
		bool flag3 = deleted && _deletedReferenceMap != null && _deletedReferenceMap.Count > 0;
		bool flag4 = unchanged && _unchangedReferenceMap != null && _unchangedReferenceMap.Count > 0;
		bool flag5 = _dependentTypeReferenceMap != null && _dependentTypeReferenceMap.Count > 0;
		if (!flag5)
		{
			int num = (flag ? 1 : 0) + (flag2 ? 1 : 0) + (flag3 ? 1 : 0) + (flag4 ? 1 : 0);
			if (num == 1)
			{
				if (flag4)
				{
					return _unchangedReferenceMap.Values;
				}
				if (flag)
				{
					return _addedReferenceMap.Values;
				}
				if (flag2)
				{
					return _modifiedReferenceMap.Values;
				}
				if (flag3)
				{
					return _deletedReferenceMap.Values;
				}
			}
			if (num == 0)
			{
				return Enumerable.Empty<InternalEntityEntry>();
			}
		}
		return GetEntriesForState(added, modified, deleted, unchanged, flag5, flag, flag2, flag3, flag4);
	}

	private IEnumerable<InternalEntityEntry> GetEntriesForState(bool added, bool modified, bool deleted, bool unchanged, bool hasDependentTypes, bool returnAdded, bool returnModified, bool returnDeleted, bool returnUnchanged)
	{
		if (returnAdded)
		{
			foreach (InternalEntityEntry value in _addedReferenceMap.Values)
			{
				yield return value;
			}
		}
		if (returnModified)
		{
			foreach (InternalEntityEntry value2 in _modifiedReferenceMap.Values)
			{
				yield return value2;
			}
		}
		if (returnDeleted)
		{
			foreach (InternalEntityEntry value3 in _deletedReferenceMap.Values)
			{
				yield return value3;
			}
		}
		if (returnUnchanged)
		{
			foreach (InternalEntityEntry value4 in _unchangedReferenceMap.Values)
			{
				yield return value4;
			}
		}
		if (!hasDependentTypes)
		{
			yield break;
		}
		foreach (EntityReferenceMap value5 in _dependentTypeReferenceMap.Values)
		{
			foreach (InternalEntityEntry item in value5.GetEntriesForState(added, modified, deleted, unchanged))
			{
				if (item.SharedIdentityEntry == null || item.EntityState != EntityState.Deleted)
				{
					yield return item;
				}
			}
		}
	}

	private void Remove(object entity, IEntityType entityType, EntityState oldState)
	{
		if (_dependentTypeReferenceMap != null && entityType.HasDefiningNavigation())
		{
			_dependentTypeReferenceMap[entityType].Remove(entity, entityType, oldState);
			return;
		}
		switch (oldState)
		{
		case EntityState.Detached:
			_detachedReferenceMap?.Remove(entity);
			break;
		case EntityState.Unchanged:
			_unchangedReferenceMap.Remove(entity);
			break;
		case EntityState.Deleted:
			_deletedReferenceMap.Remove(entity);
			break;
		case EntityState.Modified:
			_modifiedReferenceMap.Remove(entity);
			break;
		case EntityState.Added:
			_addedReferenceMap.Remove(entity);
			break;
		}
	}

	public virtual void Clear()
	{
		_unchangedReferenceMap = null;
		_detachedReferenceMap = null;
		_deletedReferenceMap = null;
		_addedReferenceMap = null;
		_modifiedReferenceMap = null;
		_dependentTypeReferenceMap?.Clear();
		_dependentTypeReferenceMap = null;
	}

	public virtual IEnumerable<TEntity> GetNonDeletedEntities<TEntity>() where TEntity : class
	{
		if (_addedReferenceMap != null && _addedReferenceMap.Count > 0)
		{
			foreach (InternalEntityEntry value in _addedReferenceMap.Values)
			{
				if (value.Entity is TEntity val)
				{
					yield return val;
				}
			}
		}
		if (_modifiedReferenceMap != null && _modifiedReferenceMap.Count > 0)
		{
			foreach (InternalEntityEntry value2 in _modifiedReferenceMap.Values)
			{
				if (value2.Entity is TEntity val2)
				{
					yield return val2;
				}
			}
		}
		if (_unchangedReferenceMap != null && _unchangedReferenceMap.Count > 0)
		{
			foreach (InternalEntityEntry value3 in _unchangedReferenceMap.Values)
			{
				if (value3.Entity is TEntity val3)
				{
					yield return val3;
				}
			}
		}
		if (_dependentTypeReferenceMap == null || _dependentTypeReferenceMap.Count <= 0)
		{
			yield break;
		}
		foreach (EntityReferenceMap value4 in _dependentTypeReferenceMap.Values)
		{
			foreach (TEntity nonDeletedEntity in value4.GetNonDeletedEntities<TEntity>())
			{
				yield return nonDeletedEntity;
			}
		}
	}
}
