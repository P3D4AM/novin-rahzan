using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class SharedTableEntryMap<TValue>
{
	private class EntryComparer : IComparer<IUpdateEntry>
	{
		private readonly IReadOnlyDictionary<IEntityType, IReadOnlyList<IEntityType>> _principals;

		public EntryComparer(IReadOnlyDictionary<IEntityType, IReadOnlyList<IEntityType>> principals)
		{
			_principals = principals;
		}

		public int Compare(IUpdateEntry x, IUpdateEntry y)
		{
			if (_principals[x.EntityType].Count != 0)
			{
				if (_principals[y.EntityType].Count != 0)
				{
					return StringComparer.Ordinal.Compare(x.EntityType.Name, y.EntityType.Name);
				}
				return 1;
			}
			return -1;
		}
	}

	private readonly IUpdateAdapter _updateAdapter;

	private readonly IReadOnlyDictionary<IEntityType, IReadOnlyList<IEntityType>> _principals;

	private readonly IReadOnlyDictionary<IEntityType, IReadOnlyList<IEntityType>> _dependents;

	private readonly string _name;

	private readonly string _schema;

	private readonly SharedTableEntryValueFactory<TValue> _createElement;

	private readonly IComparer<IUpdateEntry> _comparer;

	private readonly Dictionary<IUpdateEntry, TValue> _entryValueMap = new Dictionary<IUpdateEntry, TValue>();

	public virtual IEnumerable<TValue> Values => _entryValueMap.Values;

	public SharedTableEntryMap([JetBrains.Annotations.NotNull] IUpdateAdapter updateAdapter, [JetBrains.Annotations.NotNull] IReadOnlyDictionary<IEntityType, IReadOnlyList<IEntityType>> principals, [JetBrains.Annotations.NotNull] IReadOnlyDictionary<IEntityType, IReadOnlyList<IEntityType>> dependents, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] SharedTableEntryValueFactory<TValue> createElement)
	{
		_updateAdapter = updateAdapter;
		_principals = principals;
		_dependents = dependents;
		_name = name;
		_schema = schema;
		_createElement = createElement;
		_comparer = new EntryComparer(principals);
	}

	public static Dictionary<(string Schema, string Name), SharedTableEntryMapFactory<TValue>> CreateSharedTableEntryMapFactories([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IUpdateAdapter updateAdapter)
	{
		Dictionary<(string, string), List<IEntityType>> dictionary = new Dictionary<(string, string), List<IEntityType>>();
		foreach (IEntityType item in from et in model.GetEntityTypes()
			where et.FindPrimaryKey() != null
			select et)
		{
			(string, string) key = (item.GetSchema(), item.GetTableName());
			if (!dictionary.TryGetValue(key, out var value))
			{
				value = new List<IEntityType>();
				dictionary.Add(key, value);
			}
			value.Add(item);
		}
		Dictionary<(string, string), SharedTableEntryMapFactory<TValue>> dictionary2 = new Dictionary<(string, string), SharedTableEntryMapFactory<TValue>>();
		foreach (KeyValuePair<(string, string), List<IEntityType>> item2 in dictionary)
		{
			if (item2.Value.Count > 1)
			{
				SharedTableEntryMapFactory<TValue> value2 = CreateSharedTableEntryMapFactory(item2.Value, updateAdapter, item2.Key.Item2, item2.Key.Item1);
				dictionary2.Add(item2.Key, value2);
			}
		}
		return dictionary2;
	}

	public static SharedTableEntryMapFactory<TValue> CreateSharedTableEntryMapFactory([JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> entityTypes, [JetBrains.Annotations.NotNull] IUpdateAdapter updateAdapter, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string schema)
	{
		Dictionary<IEntityType, IReadOnlyList<IEntityType>> principals = new Dictionary<IEntityType, IReadOnlyList<IEntityType>>(entityTypes.Count);
		Dictionary<IEntityType, IReadOnlyList<IEntityType>> dependents = new Dictionary<IEntityType, IReadOnlyList<IEntityType>>(entityTypes.Count);
		foreach (IEntityType item in entityTypes.Where((IEntityType t) => t.FindPrimaryKey() != null))
		{
			List<IEntityType> list = new List<IEntityType>();
			foreach (IForeignKey item2 in item.FindForeignKeys(item.FindPrimaryKey().Properties))
			{
				if (item2.PrincipalKey.IsPrimaryKey() && entityTypes.Contains(item2.PrincipalEntityType) && !item2.IsIntraHierarchical())
				{
					list.Add(item2.PrincipalEntityType);
				}
			}
			principals[item] = list;
			List<IEntityType> list2 = new List<IEntityType>();
			foreach (IForeignKey referencingForeignKey in item.FindPrimaryKey().GetReferencingForeignKeys())
			{
				if (referencingForeignKey.PrincipalEntityType.IsAssignableFrom(item) && entityTypes.Contains(referencingForeignKey.DeclaringEntityType) && !referencingForeignKey.IsIntraHierarchical() && PropertyListComparer.Instance.Compare(referencingForeignKey.DeclaringEntityType.FindPrimaryKey().Properties, referencingForeignKey.Properties) == 0)
				{
					list2.Add(referencingForeignKey.DeclaringEntityType);
				}
			}
			dependents[item] = list2;
		}
		return (SharedTableEntryValueFactory<TValue> createElement) => new SharedTableEntryMap<TValue>(updateAdapter, principals, dependents, tableName, schema, createElement);
	}

	public virtual TValue GetOrAddValue([JetBrains.Annotations.NotNull] IUpdateEntry entry)
	{
		IUpdateEntry mainEntry = GetMainEntry(entry);
		if (_entryValueMap.TryGetValue(mainEntry, out var value))
		{
			return value;
		}
		value = _createElement(_name, _schema, _comparer);
		_entryValueMap.Add(mainEntry, value);
		return value;
	}

	public virtual IReadOnlyList<IEntityType> GetPrincipals([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		return _principals[entityType];
	}

	public virtual IReadOnlyList<IEntityType> GetDependents([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		return _dependents[entityType];
	}

	private IUpdateEntry GetMainEntry(IUpdateEntry entry)
	{
		IEntityType rootType = entry.EntityType.GetRootType();
		if (_principals[rootType].Count == 0)
		{
			return entry;
		}
		foreach (IForeignKey item in rootType.FindForeignKeys(rootType.FindPrimaryKey().Properties))
		{
			if (item.PrincipalKey.IsPrimaryKey() && _principals.ContainsKey(item.PrincipalEntityType))
			{
				IUpdateEntry updateEntry = _updateAdapter.FindPrincipal(entry, item);
				if (updateEntry != null)
				{
					return GetMainEntry(updateEntry);
				}
			}
		}
		return entry;
	}

	public virtual IReadOnlyList<IUpdateEntry> GetAllEntries([JetBrains.Annotations.NotNull] IUpdateEntry entry)
	{
		List<IUpdateEntry> list = new List<IUpdateEntry>();
		AddAllDependentsInclusive(GetMainEntry(entry), list);
		return list;
	}

	private void AddAllDependentsInclusive(IUpdateEntry entry, List<IUpdateEntry> entries)
	{
		entries.Add(entry);
		foreach (IForeignKey referencingForeignKey in entry.EntityType.GetReferencingForeignKeys())
		{
			if (referencingForeignKey.PrincipalKey.IsPrimaryKey() && referencingForeignKey.IsUnique && _dependents.ContainsKey(referencingForeignKey.DeclaringEntityType))
			{
				IUpdateEntry updateEntry = _updateAdapter.GetDependents(entry, referencingForeignKey).SingleOrDefault();
				if (updateEntry != null)
				{
					AddAllDependentsInclusive(updateEntry, entries);
				}
			}
		}
	}
}
