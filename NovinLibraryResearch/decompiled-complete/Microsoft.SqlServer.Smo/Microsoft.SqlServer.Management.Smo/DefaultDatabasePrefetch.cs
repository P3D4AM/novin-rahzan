using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal class DefaultDatabasePrefetch : DatabasePrefetchBase, IDatabasePrefetch
{
	private static int MAX_BATCH_SIZE = 9000;

	private Dictionary<Urn, int> idDictionary;

	public DefaultDatabasePrefetch(Database db, ScriptingPreferences scriptingPreferences, HashSet<UrnTypeKey> filteredTypes)
		: base(db, scriptingPreferences, filteredTypes)
	{
		batchSize = MAX_BATCH_SIZE;
		idDictionary = new Dictionary<Urn, int>();
	}

	private void InitializeTableSets(HashSet<Urn> userTables, HashSet<Urn> systemTables)
	{
		GetIsSystemObjectForCollection<Table>("Table");
		GetUserAndSystemObjects<Table>(Database.Tables, userTables, systemTables);
	}

	private void InitializeViewSets(HashSet<Urn> userViews, HashSet<Urn> systemViews)
	{
		GetIsSystemObjectForCollection<View>("View");
		GetUserAndSystemObjects<View>(Database.Views, userViews, systemViews);
	}

	private void GetIsSystemObjectForCollection<T>(string urnSuffix) where T : SqlSmoObject
	{
		StringCollection defaultInitFields = Database.GetServerObject().GetDefaultInitFields(typeof(T), Database.DatabaseEngineEdition);
		Database.GetServerObject().SetDefaultInitFields(typeof(T), Database.DatabaseEngineEdition, "IsSystemObject");
		Database.InitChildLevel(urnSuffix, scriptingPreferences, forScripting: false);
		Database.GetServerObject().SetDefaultInitFields(typeof(T), defaultInitFields, Database.DatabaseEngineEdition);
	}

	private void GetUserAndSystemObjects<T>(SmoCollectionBase collection, ICollection<Urn> userObjects, ICollection<Urn> systemObjects) where T : SqlSmoObject
	{
		foreach (T item in collection)
		{
			if (!item.GetPropValueOptional("IsSystemObject", defaultValue: false))
			{
				userObjects.Add(item.Urn);
			}
			else
			{
				systemObjects.Add(item.Urn);
			}
		}
	}

	protected override void InitializeBatchedPrefetchDictionary()
	{
		batchedPrefetchDictionary = new Dictionary<string, List<Urn>>();
		foreach (string item in new List<string> { "Table", "View" })
		{
			batchedPrefetchDictionary.Add(item, new List<Urn>());
		}
	}

	protected override void InitializePrefetchableTypes()
	{
		prefetchableTypes = new HashSet<string>();
		foreach (string item in new List<string> { "Table", "View" })
		{
			prefetchableTypes.Add(item);
		}
	}

	protected override IEnumerable<Urn> PrePrefetchBatches()
	{
		if (batchedPrefetchDictionary["Table"].Count > 1)
		{
			HashSet<Urn> hashSet = new HashSet<Urn>();
			HashSet<Urn> hashSet2 = new HashSet<Urn>();
			InitializeTableSets(hashSet2, hashSet);
			if (IsAllObjectPrefetchPossible(batchedPrefetchDictionary["Table"], hashSet2, hashSet, out var filter))
			{
				PrefetchUsingIN(filter, string.Empty, "Table", GetTablePrefetchList());
				foreach (Urn item in batchedPrefetchDictionary["Table"])
				{
					yield return item;
				}
				batchedPrefetchDictionary["Table"].Clear();
			}
		}
		if (batchedPrefetchDictionary["View"].Count <= 1)
		{
			yield break;
		}
		HashSet<Urn> hashSet3 = new HashSet<Urn>();
		HashSet<Urn> hashSet4 = new HashSet<Urn>();
		InitializeViewSets(hashSet4, hashSet3);
		if (!IsAllObjectPrefetchPossible(batchedPrefetchDictionary["View"], hashSet4, hashSet3, out var filter2))
		{
			yield break;
		}
		PrefetchUsingIN(filter2, string.Empty, "View", GetViewPrefetchList());
		foreach (Urn item2 in batchedPrefetchDictionary["View"])
		{
			yield return item2;
		}
		batchedPrefetchDictionary["View"].Clear();
	}

	private bool IsAllObjectPrefetchPossible(List<Urn> inputList, HashSet<Urn> userObjectSet, HashSet<Urn> systemObjectSet, out string filter)
	{
		bool flag = true;
		filter = string.Empty;
		foreach (Urn input in inputList)
		{
			userObjectSet.Remove(input);
			if (systemObjectSet.Remove(input))
			{
				flag = false;
			}
		}
		if (userObjectSet.Count > 0)
		{
			return false;
		}
		if (!scriptingPreferences.SystemObjects || flag)
		{
			filter = "[@IsSystemObject=false()]";
			return true;
		}
		if (systemObjectSet.Count > 0)
		{
			return false;
		}
		return true;
	}

	protected override void PrefetchBatch(string urnType, HashSet<Urn> urnBatch, int currentBatchCount, int totalBatchCount)
	{
		string idFilter = GetIdFilter(urnBatch);
		List<string> childrenList = GetChildrenList(urnType);
		PrefetchUsingIN(idFilter, idFilter, urnType, childrenList);
	}

	protected virtual List<string> GetChildrenList(string urnType)
	{
		if (urnType.Equals("Table"))
		{
			return GetTablePrefetchList();
		}
		return GetViewPrefetchList();
	}

	private string GetIdFilter(HashSet<Urn> urnBatch)
	{
		if (urnBatch.Count == 1)
		{
			Urn urn = urnBatch.First();
			SqlSmoObject smoObject = Database.Parent.GetSmoObject(urn);
			NamedSmoObject namedSmoObject = smoObject as NamedSmoObject;
			ScriptSchemaObjectBase scriptSchemaObjectBase = smoObject as ScriptSchemaObjectBase;
			if (namedSmoObject != null)
			{
				string arg = SqlSmoObject.SqlString(namedSmoObject.Name);
				if (scriptSchemaObjectBase != null)
				{
					string arg2 = SqlSmoObject.SqlString(scriptSchemaObjectBase.Schema);
					return string.Format(SmoApplication.DefaultCulture, "[@Name='{0}' and @Schema='{1}']", arg, arg2);
				}
				return string.Format(SmoApplication.DefaultCulture, "[@Name='{0}']", arg);
			}
		}
		return string.Format(SmoApplication.DefaultCulture, "[in(@ID, '{0}')]", GetFilteringids(urnBatch));
	}

	private string GetFilteringids(HashSet<Urn> objects)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Urn @object in objects)
		{
			int num = idDictionary[@object];
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0},", num);
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return stringBuilder.ToString();
	}

	protected virtual void PrefetchUsingIN(string idFilter, string initializeCollectionsFilter, string type, IEnumerable<string> prefetchingList)
	{
		foreach (string prefetching in prefetchingList)
		{
			string value = string.Format(SmoApplication.DefaultCulture, "{0}/{1}{2}{3}", Database.Urn, type, idFilter, prefetching);
			string value2 = string.Format(SmoApplication.DefaultCulture, "{0}/{1}{2}{3}", Database.Urn, type, initializeCollectionsFilter, prefetching);
			Database.Parent.InitQueryUrns(new Urn(value), null, null, null, scriptingPreferences, new Urn(value2), Database.DatabaseEngineEdition);
		}
	}

	protected override void AddUrn(Urn item)
	{
		int value = (int)Database.Parent.GetSmoObject(item).Properties.GetValueWithNullReplacement("ID");
		idDictionary.Add(item, value);
	}
}
