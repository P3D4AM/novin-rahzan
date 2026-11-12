using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Data.Engine;

public static class StiDataActionOperator
{
	private static object lockObject = new object();

	private static Dictionary<string, DataTable> netCache = new Dictionary<string, DataTable>();

	private static Dictionary<string, StiDataTable> meterCache = new Dictionary<string, StiDataTable>();

	public static DataTable Apply(DataTable inTable, IEnumerable<StiDataActionRule> actions, IStiReport report, int hash)
	{
		if (actions == null)
		{
			return inTable;
		}
		actions = actions.Where((StiDataActionRule a) => a.Priority == StiDataActionPriority.BeforeTransformation);
		if (!actions.Any())
		{
			return inTable;
		}
		DataTable dataTable = GetFromCache(inTable, report, hash);
		if (dataTable == DataTableExt.NullTable)
		{
			return null;
		}
		if (dataTable == null)
		{
			dataTable = inTable.Copy();
			List<string> columnNames = (from DataColumn c in inTable.Columns
				select c.ColumnName).ToList();
			StiDataActionRuleHelper.ApplyActions(dataTable, actions.ToList(), null, columnNames, report);
			AddToCache(inTable, report, hash, dataTable);
		}
		return dataTable;
	}

	public static StiDataTable ApplyAfterTransformation(StiDataTable inTable, IEnumerable<StiDataActionRule> actions, StiDataActionPriority priority, IStiReport report, int hash)
	{
		if (actions == null)
		{
			return inTable;
		}
		actions = actions.Where((StiDataActionRule a) => a.Priority == priority);
		if (!actions.Any())
		{
			return inTable;
		}
		StiDataTable stiDataTable = GetFromCache(inTable, report, hash);
		if (stiDataTable == null)
		{
			DataTable dataTable = inTable.ToNetTable();
			List<string> columnKeys = inTable.Meters.Select((IStiMeter m) => m.Key).ToList();
			List<string> columnNames = (from DataColumn c in dataTable.Columns
				select c.ColumnName).ToList();
			StiDataActionRuleHelper.ApplyActions(dataTable, actions.ToList(), columnKeys, columnNames, report);
			List<object[]> rows = (from r in dataTable.DefaultView.ToTable().AsEnumerable()
				select r.ItemArray).ToList();
			stiDataTable = new StiDataTable(inTable.Meters, rows);
			AddToCache(inTable, report, hash, stiDataTable);
		}
		return stiDataTable;
	}

	internal static void CleanCache(string appKey)
	{
		lock (lockObject)
		{
			if (appKey == null)
			{
				netCache.Clear();
				meterCache.Clear();
				return;
			}
			netCache.Keys.Where((string k) => k.StartsWith(appKey)).ToList().ForEach(delegate(string k)
			{
				netCache.Remove(k);
			});
			meterCache.Keys.Where((string k) => k.StartsWith(appKey)).ToList().ForEach(delegate(string k)
			{
				meterCache.Remove(k);
			});
		}
	}

	private static string GetCacheKey(DataTable table, IStiApp app, int hash)
	{
		app?.SetKey(StiKeyHelper.GetOrGeneratedKey(app.GetKey()));
		string arg = app?.GetKey() ?? string.Empty;
		int num = (from DataColumn c in table.Columns
			select c.GetHashCode()).Aggregate(0, (int c1, int c2) => c1 + c2);
		return $"{arg}.{num + hash}";
	}

	private static string GetCacheKey(StiDataTable table, IStiApp app, int hash)
	{
		app?.SetKey(StiKeyHelper.GetOrGeneratedKey(app.GetKey()));
		string arg = app?.GetKey() ?? string.Empty;
		int num = table.Meters.Select((IStiMeter c) => c.GetHashCode()).Aggregate(0, (int c1, int c2) => c1 + c2);
		return $"{arg}.{num + hash}";
	}

	private static DataTable GetFromCache(DataTable inTable, IStiApp app, int hash)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(inTable, app, hash);
			return netCache.ContainsKey(cacheKey) ? netCache[cacheKey] : null;
		}
	}

	private static StiDataTable GetFromCache(StiDataTable inTable, IStiApp app, int hash)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(inTable, app, hash);
			return meterCache.ContainsKey(cacheKey) ? meterCache[cacheKey] : null;
		}
	}

	private static void AddToCache(DataTable inTable, IStiApp app, int hash, DataTable outTable)
	{
		lock (lockObject)
		{
			if (outTable == null)
			{
				outTable = DataTableExt.NullTable;
			}
			string cacheKey = GetCacheKey(inTable, app, hash);
			netCache[cacheKey] = outTable;
		}
	}

	private static void AddToCache(StiDataTable inTable, IStiApp app, int hash, StiDataTable outTable)
	{
		lock (lockObject)
		{
			if (outTable == null)
			{
				outTable = StiDataTable.NullTable;
			}
			string cacheKey = GetCacheKey(inTable, app, hash);
			meterCache[cacheKey] = outTable;
		}
	}
}
