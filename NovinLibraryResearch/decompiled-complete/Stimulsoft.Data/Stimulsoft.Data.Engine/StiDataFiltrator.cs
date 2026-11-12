using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Functions;

namespace Stimulsoft.Data.Engine;

public static class StiDataFiltrator
{
	private static object lockObject = new object();

	private static Dictionary<string, StiDataTable> meterCache = new Dictionary<string, StiDataTable>();

	private static Dictionary<string, DataTable> netCache = new Dictionary<string, DataTable>();

	public static DataTable Filter(DataTable inTable, IEnumerable<StiDataFilterRule> filters, IStiReport report, int hash)
	{
		if (filters == null || !filters.Any())
		{
			return inTable;
		}
		List<string> columnNames = (from DataColumn c in inTable.Columns
			select c.ColumnName).ToList();
		List<Type> columnTypes = (from DataColumn c in inTable.Columns
			select c.DataType).ToList();
		IEnumerable<StiDataFilterRule> enumerable = filters.Where((StiDataFilterRule f) => f.Condition == StiDataFilterCondition.MapEqualTo);
		filters = filters.Where((StiDataFilterRule f) => f.Condition != StiDataFilterCondition.MapEqualTo);
		string dataTableFilterQuery = StiDataFilterRuleHelper.GetDataTableFilterQuery(filters.ToList(), columnNames, columnTypes, report);
		if (string.IsNullOrWhiteSpace(dataTableFilterQuery) && !enumerable.Any())
		{
			return inTable;
		}
		DataTable fromCache = GetFromCache(inTable, report, hash);
		if (fromCache == DataTableExt.NullTable)
		{
			return null;
		}
		if (fromCache != null)
		{
			return fromCache;
		}
		if (!string.IsNullOrWhiteSpace(dataTableFilterQuery))
		{
			string rowFilter = inTable.DefaultView.RowFilter;
			try
			{
				inTable.DefaultView.RowFilter = dataTableFilterQuery;
			}
			catch
			{
				inTable.DefaultView.RowFilter = rowFilter;
			}
		}
		fromCache = inTable.DefaultView.ToTable();
		if (enumerable.Any())
		{
			fromCache = FilterMapIdents(enumerable, fromCache);
		}
		AddToCache(inTable, report, hash, fromCache);
		return fromCache;
	}

	public static StiDataTable Filter(StiDataTable inTable, IEnumerable<StiDataFilterRule> filters, IStiReport report, int hash)
	{
		if (filters == null || !filters.Any())
		{
			return inTable;
		}
		DataTable dataTable = inTable.ToNetTable();
		List<string> columnNames = (from DataColumn c in dataTable.Columns
			select c.ColumnName).ToList();
		List<Type> columnTypes = (from DataColumn c in dataTable.Columns
			select c.DataType).ToList();
		string dataTableFilterQuery = StiDataFilterRuleHelper.GetDataTableFilterQuery(filters.ToList(), columnNames, columnTypes, report);
		if (string.IsNullOrWhiteSpace(dataTableFilterQuery))
		{
			return inTable;
		}
		StiDataTable fromCache = GetFromCache(inTable, report, hash);
		if (fromCache == StiDataTable.NullTable)
		{
			return null;
		}
		if (fromCache != null)
		{
			return fromCache;
		}
		dataTable.DefaultView.RowFilter = dataTableFilterQuery;
		dataTable = dataTable.DefaultView.ToTable();
		List<object[]> rows = (from r in dataTable.DefaultView.ToTable().AsEnumerable()
			select r.ItemArray).ToList();
		fromCache = new StiDataTable(inTable.Meters, rows);
		AddToCache(inTable, report, hash, fromCache);
		return fromCache;
	}

	private static DataTable FilterMapIdents(IEnumerable<StiDataFilterRule> filters, DataTable outTable)
	{
		EnumerableRowCollection<DataRow> source = outTable.AsEnumerable();
		var idioms = filters.Select((StiDataFilterRule f) => new
		{
			Field = f.Path,
			Idents = (from v in Funcs.GetMapIdents(Simplify(f.Value))
				select Simplify(v)).Distinct().ToList()
		});
		source = source.Where((DataRow r) => idioms.Any(idiom => idiom.Idents.Any((string i) => i == Simplify(r.Field<string>(idiom.Field)))));
		if (source.Any())
		{
			outTable = source.CopyToDataTable();
		}
		else
		{
			outTable.Rows.Clear();
		}
		return outTable;
	}

	private static string Simplify(string key)
	{
		return key?.ToLowerInvariant()?.Replace(" ", "")?.Replace("-", "");
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
		int num = table.Meters.Select((IStiMeter m) => m.GetUniqueCode()).Aggregate(0, (int c1, int c2) => c1 + c2);
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
			string cacheKey = GetCacheKey(inTable, app, hash);
			if (outTable == null)
			{
				outTable = DataTableExt.NullTable;
			}
			netCache[cacheKey] = outTable;
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
			meterCache[cacheKey] = outTable;
		}
	}
}
