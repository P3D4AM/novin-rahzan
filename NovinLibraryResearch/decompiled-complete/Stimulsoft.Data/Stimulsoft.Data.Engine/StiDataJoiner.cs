using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Data.Engine;

public static class StiDataJoiner
{
	public enum StiDataJoinEngine
	{
		V1,
		V2,
		V3,
		V4
	}

	private static object lockObject = new object();

	private static Dictionary<string, DataTable> cache = new Dictionary<string, DataTable>();

	public static StiDataJoinEngine JoinEngine { get; set; } = StiDataJoinEngine.V4;


	public static DataTable Join(IEnumerable<DataTable> tables, List<StiDataLink> links, IEnumerable<IStiMeter> meters, IStiApp app)
	{
		if (tables.Count() < 2)
		{
			return tables.FirstOrDefault();
		}
		DataTable fromCache = GetFromCache(tables, app);
		if (fromCache == DataTableExt.NullTable)
		{
			return null;
		}
		if (fromCache != null)
		{
			return fromCache;
		}
		List<DataTable> source = tables.ToList();
		DataTable dataTable = source.First();
		source = source.Skip(1).ToList();
		Hashtable hashtable = new Hashtable();
		List<DataTable> list = new List<DataTable>();
		fromCache = dataTable?.AsEnumerable().CopyToDataTableExt();
		CopyColumns(dataTable, fromCache);
		List<DataTable> list2 = new List<DataTable> { dataTable };
		List<DataTable> list3 = new List<DataTable>();
		while (source.Any())
		{
			DataTable stackedTable = source.FirstOrDefault();
			if (stackedTable == null)
			{
				continue;
			}
			DataTable dataTable2 = list2.FirstOrDefault((DataTable t) => FindLink(stackedTable, t, links) != null);
			StiDataLink stiDataLink = FindLink(dataTable2, stackedTable, links);
			if ((JoinEngine == StiDataJoinEngine.V2 || JoinEngine == StiDataJoinEngine.V3 || JoinEngine == StiDataJoinEngine.V4) && stiDataLink == null && source.Count > 2 && list3.Count < source.Count)
			{
				source.Remove(stackedTable);
				source.Add(stackedTable);
				list3.Add(stackedTable);
			}
			else if (dataTable2 == null || stiDataLink == null)
			{
				source.Remove(stackedTable);
				if (hashtable[stackedTable] == null)
				{
					source.Add(stackedTable);
				}
				else
				{
					list.Add(stackedTable);
				}
				hashtable[stackedTable] = stackedTable;
			}
			else
			{
				CopyColumns(stackedTable, fromCache);
				DataTable table = ((fromCache.Rows.Count == 0) ? dataTable2 : fromCache);
				IEnumerable<DataRow> source2 = new StiDataRowJoiner(fromCache, table, stackedTable).Join(StiDataJoinType.Left, stiDataLink, meters);
				fromCache = (source2.Any() ? source2.CopyToDataTable() : new DataTable());
				source.Remove(stackedTable);
				list2.Add(stackedTable);
			}
		}
		foreach (DataTable item in list)
		{
			fromCache = MergeInSequence(fromCache, item);
		}
		AddToCache(tables, fromCache, app);
		return fromCache;
	}

	public static List<StiDataLink> FindPath(IEnumerable<string> tables, List<StiDataLink> links)
	{
		if (tables.Count() < 2)
		{
			return null;
		}
		List<string> source = tables.ToList();
		string item = source.First();
		source = source.Skip(1).ToList();
		List<StiDataLink> list = new List<StiDataLink>();
		List<string> list2 = new List<string> { item };
		while (source.Any())
		{
			string stackedTable = source.FirstOrDefault();
			if (stackedTable != null)
			{
				string text = list2.FirstOrDefault((string t) => FindLink(stackedTable, t, links) != null);
				StiDataLink stiDataLink = FindLink(text, stackedTable, links);
				if (stiDataLink != null)
				{
					list.Add(stiDataLink);
				}
				if (text == null || stiDataLink == null)
				{
					source.Remove(stackedTable);
					continue;
				}
				source.Remove(stackedTable);
				list2.Add(stackedTable);
			}
		}
		return list;
	}

	private static void CopyColumns(DataTable from, DataTable to)
	{
		if (from == null || to == null)
		{
			return;
		}
		foreach (DataColumn column in from.Columns)
		{
			if (!to.Columns.Contains(column.ColumnName))
			{
				to.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
			}
		}
	}

	private static DataTable MergeInSequence(DataTable table1, DataTable table2)
	{
		DataTable dataTable = new DataTable();
		CopyColumns(table1, dataTable);
		CopyColumns(table2, dataTable);
		for (int i = 0; i < table1.Rows.Count; i++)
		{
			DataRow dataRow = dataTable.NewRow();
			if (i < table1.Rows.Count)
			{
				for (int j = 0; j < table1.Columns.Count; j++)
				{
					try
					{
						dataRow[j] = table1.Rows[i][j];
					}
					catch
					{
					}
				}
			}
			dataTable.Rows.Add(dataRow);
		}
		for (int k = 0; k < table2.Rows.Count; k++)
		{
			DataRow dataRow2 = dataTable.NewRow();
			if (k < table2.Rows.Count)
			{
				for (int l = 0; l < table2.Columns.Count; l++)
				{
					try
					{
						dataRow2[table1.Columns.Count + l] = table2.Rows[k][l];
					}
					catch
					{
					}
				}
			}
			dataTable.Rows.Add(dataRow2);
		}
		return dataTable;
	}

	public static StiDataLink FindLink(DataTable table1, DataTable table2, List<StiDataLink> links)
	{
		return FindLink(table1?.TableName, table2?.TableName, links);
	}

	public static StiDataLink FindLink(string table1, string table2, List<StiDataLink> links)
	{
		if (table1 == null || table2 == null)
		{
			return null;
		}
		IEnumerable<StiDataLink> source = links.Where((StiDataLink l) => (l.ParentTable == table1 && l.ChildTable == table2) || (l.ParentTable == table2 && l.ChildTable == table1));
		if (source.Any((StiDataLink r) => r.Active))
		{
			return source.First((StiDataLink r) => r.Active);
		}
		if (source.Any())
		{
			return source.FirstOrDefault();
		}
		source = links.Where((StiDataLink r) => (r.ParentTable == table1 && r.ChildTable == table2) || (r.ParentTable == table2 && r.ChildTable == table1));
		if (source.Any((StiDataLink r) => r.Active))
		{
			return source.First((StiDataLink r) => r.Active);
		}
		if (source.Any())
		{
			return source.FirstOrDefault();
		}
		return null;
	}

	private static string GetCacheKey(IEnumerable<DataTable> tables, IStiApp app)
	{
		app?.SetKey(StiKeyHelper.GetOrGeneratedKey(app.GetKey()));
		string text = app?.GetKey() ?? string.Empty;
		if (tables == null || tables.Count((DataTable t) => t != null) == 0)
		{
			return text + ".0";
		}
		int num = (from c in tables.Where((DataTable t) => t != null).SelectMany((DataTable t) => t.Columns.Cast<DataColumn>())
			select c.GetHashCode()).Aggregate(0, (int c1, int c2) => c1 + c2);
		return $"{text}.{num}";
	}

	private static DataTable GetFromCache(IEnumerable<DataTable> tables, IStiApp app)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(tables, app);
			return cache.ContainsKey(cacheKey) ? cache[cacheKey] : null;
		}
	}

	private static void AddToCache(IEnumerable<DataTable> tables, DataTable dataTable, IStiApp app)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(tables, app);
			if (dataTable == null)
			{
				dataTable = DataTableExt.NullTable;
			}
			cache[cacheKey] = dataTable;
		}
	}

	internal static void CleanCache(string appKey)
	{
		lock (cache)
		{
			if (appKey == null)
			{
				cache.Clear();
				return;
			}
			cache.Keys.Where((string k) => k.StartsWith(appKey)).ToList().ForEach(delegate(string k)
			{
				cache.Remove(k);
			});
		}
	}
}
