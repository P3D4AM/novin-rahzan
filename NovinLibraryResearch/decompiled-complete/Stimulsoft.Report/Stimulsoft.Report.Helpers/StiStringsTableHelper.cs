using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Helpers;

public static class StiStringsTableHelper
{
	private static Hashtable rowsCache = new Hashtable();

	public static DataTable LoadDataTableFromPackedString(string content, params string[] columns)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return null;
		}
		byte[] array = StiPacker.UnpackFromString(content);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		content = Encoding.UTF8.GetString(array);
		return LoadDataTableFromString(content, columns);
	}

	public static DataTable LoadDataTableFromString(string content, params string[] columns)
	{
		List<string[]> rows = LoadStringRowsFromString(content, columns);
		return LoadDataTableFromStringRows(rows, columns);
	}

	public static DataTable LoadDataTableFromStringRows(List<string[]> rows, params string[] columns)
	{
		DataTable dataTable = DataTableExtensions.AddColumns(new DataTable(), columns);
		dataTable.BeginLoadData();
		try
		{
			rows?.ForEach(delegate(string[] r)
			{
				dataTable.Rows.Add(r);
			});
		}
		finally
		{
			dataTable.EndLoadData();
		}
		return dataTable;
	}

	public static List<string[]> LoadStringRowsFromPackedString(string content, params string[] columns)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return null;
		}
		byte[] array = StiPacker.UnpackFromString(content);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		content = Encoding.UTF8.GetString(array);
		return LoadStringRowsFromString(content, columns);
	}

	public static List<string[]> LoadColumnsAndStringRowsFromPackedString(string content, out string[] columns)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			columns = new string[0];
			return null;
		}
		byte[] array = StiPacker.UnpackFromString(content);
		if (array == null || array.Length == 0)
		{
			columns = new string[0];
			return null;
		}
		content = Encoding.UTF8.GetString(array);
		return LoadColumnsAndStringRowsFromString(content, out columns);
	}

	public static List<string[]> LoadStringRowsFromString(string content, params string[] columns)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		if (string.IsNullOrWhiteSpace(content))
		{
			return null;
		}
		List<object> list = new List<object>();
		JsonConvert.PopulateObject(content, (object)list);
		if (list.Count < 1)
		{
			return null;
		}
		List<string> columnsList = columns.ToList();
		int[] headerIndexes = default(int[]);
		ref int[] reference = ref headerIndexes;
		object? obj = list.FirstOrDefault();
		reference = (from h in ((JContainer)((obj is JArray) ? obj : null)).Values<string>()
			select columnsList.IndexOf(h)).ToArray();
		list.RemoveAt(0);
		if (!list.Any())
		{
			return null;
		}
		List<string[]> list2 = new List<string[]>();
		foreach (JArray item in list)
		{
			JArray val = item;
			string[] row = new string[columns.Count()];
			int headerIndex = 0;
			((JContainer)val).Values<string>().ToList().ForEach(delegate(string v)
			{
				int num = headerIndexes[headerIndex++];
				if (num != -1)
				{
					row[num] = v;
				}
			});
			list2.Add(row);
		}
		return list2;
	}

	public static List<string[]> LoadColumnsAndStringRowsFromString(string content, out string[] columns)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			columns = new string[0];
			return null;
		}
		List<object> list = new List<object>();
		JsonConvert.PopulateObject(content, (object)list);
		if (list.Count < 1)
		{
			columns = new string[0];
			return null;
		}
		object? obj = list.FirstOrDefault();
		columns = ((JContainer)((obj is JArray) ? obj : null)).Values<string>().ToArray();
		list.RemoveAt(0);
		return (from JToken t in list
			select t.Values<string>().ToArray()).ToList();
	}

	public static string SaveDataTableToPackedString(DataTable table)
	{
		string text = SaveDataTableToString(table);
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		return StiPacker.PackToString(bytes);
	}

	public static string SaveDataTableToString(DataTable table)
	{
		List<string[]> list = SaveDataTableToStringRows(table);
		if (list == null || list.Count == 0)
		{
			return null;
		}
		return JsonConvert.SerializeObject((object)list, (Formatting)1, StiJsonHelper.DefaultSerializerSettings);
	}

	public static List<string[]> SaveDataTableToStringRows(DataTable table, bool saveHeaders = true)
	{
		List<string[]> list = new List<string[]>();
		if (table == null || table.Rows.Count == 0)
		{
			return null;
		}
		if (saveHeaders)
		{
			list.Add((from c in StiDataTableExt.ColumnsList(table)
				select c.ColumnName).ToArray());
		}
		foreach (DataRow row in table.Rows)
		{
			string[] array = row.ItemArray.Select((object i) => i?.ToString()).ToArray();
			if (!array.All((string s) => string.IsNullOrWhiteSpace(s)))
			{
				list.Add(array);
			}
		}
		return list;
	}

	public static List<string[]> LoadStringRowsFromCache(IStiElement element)
	{
		if (StiKeyHelper.IsEmptyKey(element?.Key))
		{
			return null;
		}
		lock (rowsCache)
		{
			if (rowsCache[element.Key] != null)
			{
				return rowsCache[element.Key] as List<string[]>;
			}
			return null;
		}
	}

	public static List<string[]> LoadColumnsAndStringRowsFromCache(IStiElement element, out string[] columns)
	{
		if (StiKeyHelper.IsEmptyKey(element?.Key))
		{
			columns = new string[0];
			return null;
		}
		lock (rowsCache)
		{
			if (rowsCache[element.Key] != null)
			{
				List<string[]> list = (rowsCache[element.Key] as List<string[]>).ToList();
				if (list.Any())
				{
					columns = list.FirstOrDefault();
					list.RemoveAt(0);
				}
				else
				{
					columns = new string[0];
				}
				return list;
			}
			columns = new string[0];
			return null;
		}
	}

	public static void SaveDataTableToCache(IStiElement element, DataTable table, bool saveHeaders = false)
	{
		if (!StiKeyHelper.IsEmptyKey(element?.Key) && table != null)
		{
			SaveStringRowsToCache(element, SaveDataTableToStringRows(table, saveHeaders));
		}
	}

	public static void SaveColumnsAndStringRowsToCache(IStiElement element, string[] columns, List<string[]> rows)
	{
		if (rows != null)
		{
			rows = rows.ToList();
			rows.Insert(0, columns);
			SaveStringRowsToCache(element, rows);
		}
	}

	public static void SaveStringRowsToCache(IStiElement element, List<string[]> rows)
	{
		if (StiKeyHelper.IsEmptyKey(element?.Key) || rows == null)
		{
			return;
		}
		lock (rowsCache)
		{
			rowsCache[element.Key] = rows;
		}
	}

	public static void RemoveFromCache(IStiElement element)
	{
		if (StiKeyHelper.IsEmptyKey(element?.Key))
		{
			return;
		}
		lock (rowsCache)
		{
			if (rowsCache[element.Key] != null)
			{
				rowsCache.Remove(element.Key);
			}
		}
	}
}
