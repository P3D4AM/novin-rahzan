using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal class GswDatabasePrefetch : DefaultDatabasePrefetch, IDatabasePrefetch
{
	private static int MAX_BATCH_SIZE = 9000;

	private PrefetchBatchEventHandler prefetchBatchEvent;

	public event PrefetchBatchEventHandler PrefetchBatchEvent
	{
		add
		{
			prefetchBatchEvent = (PrefetchBatchEventHandler)Delegate.Combine(prefetchBatchEvent, value);
		}
		remove
		{
			prefetchBatchEvent = (PrefetchBatchEventHandler)Delegate.Remove(prefetchBatchEvent, value);
		}
	}

	public GswDatabasePrefetch(Database db, ScriptingPreferences scriptingPreferences, HashSet<UrnTypeKey> filteredTypes)
		: base(db, scriptingPreferences, filteredTypes)
	{
		batchSize = MAX_BATCH_SIZE;
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
		foreach (string item in new List<string>
		{
			"StoredProcedure", "User", "DatabaseScopedConfiguration", "DatabaseRole", "Default", "Rule", "UserDefinedFunction", "ExtendedStoredProcedure", "UserDefinedType", "UserDefinedTableType",
			"UserDefinedAggregate", "UserDefinedDataType", "XmlSchemaCollection", "SqlAssembly", "Schema", "PartitionScheme", "PartitionFunction", "Table", "View", "ColumnEncryptionKey"
		})
		{
			prefetchableTypes.Add(item);
		}
	}

	protected override IEnumerable<Urn> PrePrefetchBatches()
	{
		return Enumerable.Empty<Urn>();
	}

	protected override void PostPrefetchBatch(string urnType, HashSet<Urn> urnBatch, int currentBatchCount, int totalBatchCount)
	{
		if (totalBatchCount > 1)
		{
			if (urnType.Equals("Table"))
			{
				Database.Tables.Clear();
			}
			else
			{
				Database.Views.Clear();
			}
		}
	}

	protected override void PrefetchBatch(string urnType, HashSet<Urn> urnBatch, int currentBatchCount, int totalBatchCount)
	{
		base.PrefetchBatch(urnType, urnBatch, currentBatchCount, totalBatchCount);
		if (prefetchBatchEvent != null)
		{
			prefetchBatchEvent(this, new PrefetchBatchEventArgs(urnType, urnBatch.Count, currentBatchCount, totalBatchCount));
		}
	}

	protected override void PrefetchUsingIN(string idFilter, string initializeCollectionsFilter, string type, IEnumerable<string> prefetchingList)
	{
		base.PrefetchUsingIN(idFilter, string.Empty, type, prefetchingList);
	}

	protected override List<string> GetChildrenList(string urnType)
	{
		if (urnType.Equals("Table"))
		{
			Database.Tables.Clear();
		}
		else
		{
			Database.Views.Clear();
		}
		return base.GetChildrenList(urnType);
	}
}
