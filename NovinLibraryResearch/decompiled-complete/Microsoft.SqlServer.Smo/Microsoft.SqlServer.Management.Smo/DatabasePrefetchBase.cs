using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal abstract class DatabasePrefetchBase : IDatabasePrefetch
{
	protected Database Database;

	protected ScriptingPreferences scriptingPreferences;

	protected HashSet<UrnTypeKey> filteredTypes;

	protected HashSet<string> prefetchableTypes;

	protected Dictionary<string, List<Urn>> batchedPrefetchDictionary;

	protected int batchSize;

	public CreatingObjectDictionary creatingDictionary { get; set; }

	public DatabasePrefetchBase(Database db, ScriptingPreferences scriptingPreferences, HashSet<UrnTypeKey> filteredTypes)
	{
		Database = db;
		this.scriptingPreferences = scriptingPreferences;
		this.filteredTypes = filteredTypes;
		InitializePrefetchableTypes();
		InitializeBatchedPrefetchDictionary();
	}

	private void InitializeObjectCollection(string type)
	{
		if (type.Equals(Table.UrnSuffix) && !Database.Tables.initialized)
		{
			Database.InitChildLevel(Table.UrnSuffix, scriptingPreferences, forScripting: false);
		}
		else if (type.Equals(View.UrnSuffix) && !Database.Views.initialized)
		{
			Database.InitChildLevel(View.UrnSuffix, scriptingPreferences, forScripting: false);
		}
	}

	protected abstract void InitializeBatchedPrefetchDictionary();

	protected abstract void InitializePrefetchableTypes();

	public virtual IEnumerable<Urn> PrefetchObjects(IEnumerable<Urn> input)
	{
		foreach (Urn item3 in input)
		{
			if (prefetchableTypes.Contains(item3.Type) && Database.Parent.CompareUrn(Database.Urn, item3.Parent) == 0 && !creatingDictionary.ContainsKey(item3))
			{
				if (batchedPrefetchDictionary.TryGetValue(item3.Type, out var value))
				{
					InitializeObjectCollection(item3.Type);
					AddUrn(item3);
					value.Add(item3);
				}
				else
				{
					PrefetchAllObjects(item3.Type);
					prefetchableTypes.Remove(item3.Type);
					yield return item3;
				}
			}
			else if (item3.Type == "Database")
			{
				Database.PrefetchScriptingOnlyChildren(scriptingPreferences);
			}
			else
			{
				yield return item3;
			}
		}
		foreach (Urn item4 in PrePrefetchBatches())
		{
			yield return item4;
		}
		foreach (KeyValuePair<string, List<Urn>> item2 in batchedPrefetchDictionary.SkipWhile((KeyValuePair<string, List<Urn>> kvp) => kvp.Value.Count > batchSize))
		{
			if (item2.Value.Count <= 0)
			{
				continue;
			}
			HashSet<Urn> urnBatch2 = new HashSet<Urn>(item2.Value);
			PrefetchBatch(item2.Key, urnBatch2, 1, 1);
			foreach (Urn item5 in item2.Value)
			{
				yield return item5;
			}
			PostPrefetchBatch(item2.Key, urnBatch2, 1, 1);
			item2.Value.Clear();
		}
		foreach (KeyValuePair<string, List<Urn>> item2 in batchedPrefetchDictionary)
		{
			int batchCount2 = 0;
			int num = item2.Value.Count / batchSize;
			int totalBatchCount = num;
			if (item2.Value.Count % batchSize != 0)
			{
				totalBatchCount++;
			}
			HashSet<Urn> urnBatch2 = new HashSet<Urn>();
			foreach (Urn table in item2.Value)
			{
				if (urnBatch2.Count >= batchSize)
				{
					DatabasePrefetchBase databasePrefetchBase = this;
					string key = item2.Key;
					HashSet<Urn> urnBatch3 = urnBatch2;
					num = batchCount2 + 1;
					batchCount2 = num;
					databasePrefetchBase.PrefetchBatch(key, urnBatch3, num, totalBatchCount);
					foreach (Urn item6 in urnBatch2)
					{
						yield return item6;
					}
					PostPrefetchBatch(item2.Key, urnBatch2, batchCount2, totalBatchCount);
					urnBatch2.Clear();
				}
				urnBatch2.Add(table);
			}
			if (urnBatch2.Count <= 0)
			{
				continue;
			}
			DatabasePrefetchBase databasePrefetchBase2 = this;
			string key2 = item2.Key;
			HashSet<Urn> urnBatch4 = urnBatch2;
			num = batchCount2 + 1;
			batchCount2 = num;
			databasePrefetchBase2.PrefetchBatch(key2, urnBatch4, num, totalBatchCount);
			foreach (Urn item7 in urnBatch2)
			{
				yield return item7;
			}
			PostPrefetchBatch(item2.Key, urnBatch2, batchCount2, totalBatchCount);
		}
	}

	protected virtual IEnumerable<Urn> PrePrefetchBatches()
	{
		return Enumerable.Empty<Urn>();
	}

	protected virtual void PostPrefetchBatch(string urnType, HashSet<Urn> urnBatch, int currentBatchCount, int totalBatchCount)
	{
	}

	protected virtual void PrefetchBatch(string urnType, HashSet<Urn> urnBatch, int currentBatchCount, int totalBatchCount)
	{
		throw new NotImplementedException();
	}

	protected virtual void AddUrn(Urn item)
	{
		throw new NotImplementedException();
	}

	protected virtual void PrefetchAllObjects(string urnType)
	{
		switch (urnType)
		{
		case "StoredProcedure":
			Database.PrefetchStoredProcedures(scriptingPreferences);
			break;
		case "User":
			Database.PrefetchUsers(scriptingPreferences);
			break;
		case "DatabaseRole":
			Database.PrefetchDatabaseRoles(scriptingPreferences);
			break;
		case "Default":
			Database.PrefetchDefaults(scriptingPreferences);
			break;
		case "Rule":
			Database.PrefetchRules(scriptingPreferences);
			break;
		case "UserDefinedFunction":
			Database.PrefetchUserDefinedFunctions(scriptingPreferences);
			break;
		case "ExtendedStoredProcedure":
			Database.PrefetchExtendedStoredProcedures(scriptingPreferences);
			break;
		case "UserDefinedType":
			Database.PrefetchUserDefinedTypes(scriptingPreferences);
			break;
		case "UserDefinedTableType":
			Database.PrefetchUserDefinedTableTypes(scriptingPreferences);
			break;
		case "UserDefinedAggregate":
			Database.PrefetchUserDefinedAggregates(scriptingPreferences);
			break;
		case "UserDefinedDataType":
			Database.PrefetchUDDT(scriptingPreferences);
			break;
		case "XmlSchemaCollection":
			Database.PrefetchXmlSchemaCollections(scriptingPreferences);
			break;
		case "SqlAssembly":
			Database.PrefetchSqlAssemblies(scriptingPreferences);
			break;
		case "Schema":
			Database.PrefetchSchemas(scriptingPreferences);
			break;
		case "PartitionScheme":
			Database.PrefetchPartitionSchemes(scriptingPreferences);
			break;
		case "PartitionFunction":
			Database.PrefetchPartitionFunctions(scriptingPreferences);
			break;
		case "Table":
			Database.PrefetchTables(scriptingPreferences);
			break;
		case "View":
			Database.PrefetchViews(scriptingPreferences);
			break;
		case "ColumnEncryptionKey":
			Database.PrefetchColumnEncryptionKey(scriptingPreferences);
			break;
		}
	}

	protected List<string> GetTablePrefetchList()
	{
		List<string> list = new List<string>();
		list.Add(string.Empty);
		if (Database.IsSupportedObject<Column>(scriptingPreferences))
		{
			list.Add("/Column");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/Column/ExtendedProperty");
			}
			if (scriptingPreferences.IncludeScripts.Permissions)
			{
				list.Add("/Column/Permission");
			}
			if (Database.IsSupportedObject<Default>(scriptingPreferences))
			{
				list.Add("/Column/Default");
				if (!filteredTypes.Contains(new UrnTypeKey("DefaultColumn")) && !filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
				{
					list.Add("/Column/Default/ExtendedProperty");
				}
			}
		}
		if (Database.IsSupportedObject<Index>(scriptingPreferences))
		{
			list.Add("/Index");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/Index/ExtendedProperty");
			}
			if (Database.IsSupportedObject<IndexedColumn>(scriptingPreferences))
			{
				list.Add("/Index/IndexedColumn");
			}
		}
		if (Database.IsSupportedObject<FullTextIndex>(scriptingPreferences))
		{
			list.Add("/FullTextIndex");
			if (Database.IsSupportedObject<FullTextIndexColumn>(scriptingPreferences))
			{
				list.Add("/FullTextIndex/FullTextIndexColumn");
			}
		}
		if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
		{
			list.Add("/ExtendedProperty");
		}
		if (!filteredTypes.Contains(new UrnTypeKey(Check.UrnSuffix)))
		{
			list.Add("/Check");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/Check/ExtendedProperty");
			}
		}
		if (!filteredTypes.Contains(new UrnTypeKey(ForeignKey.UrnSuffix)) && Database.IsSupportedObject<ForeignKey>(scriptingPreferences))
		{
			list.Add("/ForeignKey");
			list.Add("/ForeignKey/Column");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/ForeignKey/ExtendedProperty");
			}
		}
		if (!filteredTypes.Contains(new UrnTypeKey(Trigger.UrnSuffix)) && Database.IsSupportedObject<Trigger>(scriptingPreferences))
		{
			list.Add("/Trigger");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/Trigger/ExtendedProperty");
			}
		}
		if (!filteredTypes.Contains(new UrnTypeKey(Statistic.UrnSuffix)) && Database.IsSupportedObject<Statistic>(scriptingPreferences))
		{
			list.Add("/Statistic");
			list.Add("/Statistic/Column");
		}
		if (scriptingPreferences.IncludeScripts.Permissions)
		{
			list.Add("/Permission");
		}
		return list;
	}

	protected List<string> GetViewPrefetchList()
	{
		List<string> list = new List<string>();
		list.Add(string.Empty);
		if (Database.IsSupportedObject<Column>(scriptingPreferences))
		{
			list.Add("/Column");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/Column/ExtendedProperty");
			}
			if (scriptingPreferences.IncludeScripts.Permissions)
			{
				list.Add("/Column/Permission");
			}
			if (Database.IsSupportedObject<Default>(scriptingPreferences))
			{
				list.Add("/Column/Default");
				if (!filteredTypes.Contains(new UrnTypeKey("DefaultColumn")) && !filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
				{
					list.Add("/Column/Default/ExtendedProperty");
				}
			}
		}
		if (Database.IsSupportedObject<Index>(scriptingPreferences))
		{
			list.Add("/Index");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/Index/ExtendedProperty");
			}
			if (Database.IsSupportedObject<IndexedColumn>(scriptingPreferences))
			{
				list.Add("/Index/IndexedColumn");
			}
		}
		if (Database.ServerVersion.Major > 8 && Database.IsSupportedObject<FullTextIndex>(scriptingPreferences))
		{
			list.Add("/FullTextIndex");
			list.Add("/FullTextIndex/FullTextIndexColumn");
		}
		if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
		{
			list.Add("/ExtendedProperty");
		}
		if (!filteredTypes.Contains(new UrnTypeKey(Trigger.UrnSuffix)) && Database.IsSupportedObject<Trigger>(scriptingPreferences))
		{
			list.Add("/Trigger");
			if (!filteredTypes.Contains(new UrnTypeKey(ExtendedProperty.UrnSuffix)) && Database.IsSupportedObject<ExtendedProperty>(scriptingPreferences))
			{
				list.Add("/Trigger/ExtendedProperty");
			}
		}
		if (!filteredTypes.Contains(new UrnTypeKey(Statistic.UrnSuffix)) && Database.IsSupportedObject<Statistic>(scriptingPreferences))
		{
			list.Add("/Statistic");
			list.Add("/Statistic/Column");
		}
		if (scriptingPreferences.IncludeScripts.Permissions)
		{
			list.Add("/Permission");
		}
		return list;
	}
}
