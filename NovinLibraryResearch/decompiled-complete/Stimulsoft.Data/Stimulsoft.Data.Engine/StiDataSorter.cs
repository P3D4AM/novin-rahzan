using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Data.Engine;

public static class StiDataSorter
{
	private static object lockObject = new object();

	private static Dictionary<string, StiDataTable> hashCache = new Dictionary<string, StiDataTable>();

	public static StiDataTable Sort(StiDataTable inTable, IEnumerable<StiDataSortRule> sorts, IStiApp app, int hash, StiDataRequestOption option = StiDataRequestOption.All)
	{
		if (StiDataSortVariation.IsVariationSort(sorts))
		{
			return inTable;
		}
		bool flag = (option & StiDataRequestOption.AllowDataSort) > StiDataRequestOption.None;
		if (sorts == null || !sorts.Any() || !flag)
		{
			return inTable;
		}
		DataTable dataTable = inTable.ToNetTable();
		dataTable.Columns.Cast<DataColumn>().ToList().ForEach(delegate(DataColumn c)
		{
			if (c.ColumnName.Contains(","))
			{
				c.ColumnName = c.ColumnName.Replace(",", "");
			}
		});
		List<string> columnKeys = inTable.Meters.Select((IStiMeter m) => m.Key).ToList();
		List<string> columnNames = (from DataColumn c in dataTable.Columns
			select c.ColumnName).ToList();
		sorts = GetFixedDataSortRules(sorts, columnNames, columnKeys);
		string dataTableSortQuery = StiDataSortRuleHelper.GetDataTableSortQuery(sorts.ToList(), columnKeys, columnNames);
		if (string.IsNullOrWhiteSpace(dataTableSortQuery))
		{
			return inTable;
		}
		StiDataTable fromCache = GetFromCache(inTable, app, hash);
		if (fromCache == StiDataTable.NullTable)
		{
			return null;
		}
		if (fromCache != null)
		{
			return fromCache;
		}
		dataTable.DefaultView.Sort = dataTableSortQuery;
		List<object[]> rows = dataTable.DefaultView.ToTable().AsEnumerableArray().ToList();
		fromCache = new StiDataTable(inTable.Meters, rows);
		AddToCache(inTable, app, hash, fromCache);
		return fromCache;
	}

	private static IEnumerable<StiDataSortRule> GetFixedDataSortRules(IEnumerable<StiDataSortRule> sorts, List<string> columnNames, List<string> columnKeys)
	{
		List<StiDataSortRule> list = sorts.Select((StiDataSortRule s) => s.Clone() as StiDataSortRule).ToList();
		foreach (StiDataSortRule item in list)
		{
			int num = columnNames.IndexOf(item.Key);
			if (num != -1)
			{
				item.Key = columnKeys[num];
			}
		}
		return list;
	}

	internal static void CleanCache(string appKey)
	{
		lock (lockObject)
		{
			if (appKey == null)
			{
				hashCache.Clear();
				return;
			}
			hashCache.Keys.Where((string k) => k.StartsWith(appKey)).ToList().ForEach(delegate(string k)
			{
				hashCache.Remove(k);
			});
		}
	}

	private static string GetCacheKey(StiDataTable table, IStiApp app, int hash)
	{
		app?.SetKey(StiKeyHelper.GetOrGeneratedKey(app.GetKey()));
		string arg = app?.GetKey() ?? string.Empty;
		int num = table.Meters.Select((IStiMeter c) => c.GetHashCode()).Aggregate(0, (int c1, int c2) => c1 + c2);
		return $"{arg}.{num + hash}";
	}

	private static StiDataTable GetFromCache(StiDataTable inTable, IStiApp app, int hash)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(inTable, app, hash);
			return hashCache.ContainsKey(cacheKey) ? hashCache[cacheKey] : null;
		}
	}

	private static void AddToCache(StiDataTable inTable, IStiApp app, int hash, StiDataTable outTable)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(inTable, app, hash);
			if (outTable == null)
			{
				outTable = StiDataTable.NullTable;
			}
			hashCache[cacheKey] = outTable;
		}
	}
}
