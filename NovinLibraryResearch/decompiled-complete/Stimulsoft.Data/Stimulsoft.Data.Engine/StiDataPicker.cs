using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Data.Engine;

public class StiDataPicker
{
	private static object lockObject = new object();

	private static Dictionary<string, DataTable> cache = new Dictionary<string, DataTable>();

	public static IEnumerable<DataTable> Fetch(IStiQueryObject query, string group, StiDataRequestOption option = StiDataRequestOption.All, IEnumerable<string> filterNames = null, List<StiDataLink> links = null)
	{
		if (query == null)
		{
			return null;
		}
		List<string> list = query.RetrieveUsedDataNames(group);
		if (filterNames != null)
		{
			list = list.Union(filterNames).ToList();
		}
		IEnumerable<IStiAppDataSource> dataSources = query.GetDataSources(list);
		if (dataSources == null || !dataSources.Any())
		{
			return null;
		}
		IEnumerable<IStiAppDataSource> enumerable2;
		if (StiDataJoiner.JoinEngine != 0)
		{
			IEnumerable<IStiAppDataSource> enumerable = (from d in dataSources
				where d != null
				orderby GetRelationLevel(d, links)
				select d).ToList();
			enumerable2 = enumerable;
		}
		else
		{
			enumerable2 = dataSources;
		}
		dataSources = enumerable2;
		dataSources = StiDataSourceChainFinder.Find(dataSources);
		IEnumerable<IStiAppConnection> connections = from d in dataSources
			select d.GetConnection() into d
			where d != null
			select d;
		if (dataSources.All(ExistsInCache))
		{
			return dataSources.Select(GetFromCache);
		}
		IStiAppDictionary dictionary = query.GetDictionary();
		IEnumerable<IStiAppConnection> enumerable3 = null;
		try
		{
			enumerable3 = (((option & StiDataRequestOption.AllowOpenConnections) > StiDataRequestOption.None) ? dictionary.OpenConnections(connections) : null);
			List<DataTable> list2 = new List<DataTable>();
			foreach (IStiAppDataSource item in dataSources)
			{
				DataTable dataTable = GetDataTable(option, item);
				AddToCache(item, dataTable);
				if (dataTable != null)
				{
					list2.Add(dataTable);
					AddTableNameToColumnNames(dataTable, item);
				}
			}
			return list2;
		}
		finally
		{
			if ((option & StiDataRequestOption.AllowOpenConnections) > StiDataRequestOption.None && enumerable3 != null && enumerable3.Any())
			{
				dictionary.CloseConnections(enumerable3);
			}
		}
	}

	private static int GetRelationLevel(IStiAppDataSource dataSource, List<StiDataLink> links)
	{
		if (!links.Any((StiDataLink l) => l.ParentTable == dataSource.GetName() || l.ChildTable == dataSource.GetName()))
		{
			return 100;
		}
		Hashtable hashtable = new Hashtable();
		int num = 0;
		string table = dataSource.GetName();
		while (hashtable[table] == null)
		{
			hashtable[table] = table;
			table = links.FirstOrDefault((StiDataLink l) => l.ChildTable == table)?.ParentTable;
			if (table == null)
			{
				break;
			}
			num++;
			if (num > 100)
			{
				return 100;
			}
		}
		return num;
	}

	public static IEnumerable<IStiAppDataSource> RetrieveUsedDataSources(IStiQueryObject query, string group, IEnumerable<string> filterNames)
	{
		if (query == null)
		{
			return null;
		}
		List<string> list = query.RetrieveUsedDataNames(group);
		if (filterNames != null)
		{
			list = list.Union(filterNames).ToList();
		}
		IEnumerable<IStiAppDataSource> dataSources = query.GetDataSources(list);
		if (dataSources == null || !dataSources.Any())
		{
			return null;
		}
		return StiDataSourceChainFinder.Find(dataSources);
	}

	public static DataTable Fetch(IStiApp app, string dataSourceName, StiDataRequestOption option = StiDataRequestOption.All)
	{
		IStiAppDataSource dataSourceByName = app.GetDictionary().GetDataSourceByName(dataSourceName);
		return Fetch(app, dataSourceByName, option);
	}

	public static DataTable Fetch(IStiApp app, IStiAppDataSource dataSource, StiDataRequestOption option = StiDataRequestOption.All)
	{
		IStiAppDictionary dictionary = app.GetDictionary();
		if (!ExistsInCache(dataSource))
		{
			IStiAppConnection connection = dataSource.GetConnection();
			dictionary.OpenConnections(new IStiAppConnection[1] { connection });
			try
			{
				DataTable dataTable = GetDataTable(option, dataSource);
				AddToCache(dataSource, dataTable);
				AddTableNameToColumnNames(dataTable, dataSource);
				return dataTable;
			}
			finally
			{
				dictionary.CloseConnections(new IStiAppConnection[1] { connection });
			}
		}
		return GetFromCache(dataSource);
	}

	public static bool IsAllBICached(IStiQueryObject query, string group, StiDataRequestOption option = StiDataRequestOption.All)
	{
		if (!StiBIDataCacheOptions.Enabled)
		{
			return false;
		}
		if (query == null)
		{
			return false;
		}
		List<string> dataNames = query.RetrieveUsedDataNames(group);
		IEnumerable<IStiAppDataSource> dataSources = query.GetDataSources(dataNames);
		if (dataSources == null || !dataSources.Any())
		{
			return false;
		}
		dataSources = StiDataSourceChainFinder.Find(dataSources);
		return dataSources.All((IStiAppDataSource d) => StiBIDataCacheOptions.Cache.Exists(d));
	}

	public static DataTable GetDataTable(IStiApp app, IStiAppDataSource dataSource, StiDataRequestOption option = StiDataRequestOption.All)
	{
		IStiAppDictionary dictionary = app.GetDictionary();
		IStiAppConnection connection = dataSource.GetConnection();
		dictionary.OpenConnections(new IStiAppConnection[1] { connection });
		try
		{
			return GetDataTable(option, dataSource);
		}
		finally
		{
			dictionary.CloseConnections(new IStiAppConnection[1] { connection });
		}
	}

	internal static DataTable GetDataTable(StiDataRequestOption option, IStiAppDataSource dataSource)
	{
		DataTable dataTable = dataSource.GetDataTable((option & StiDataRequestOption.AllowOpenConnections) > StiDataRequestOption.None);
		if (dataTable != null)
		{
			dataTable = dataTable.Copy();
			dataTable.TableName = dataSource.GetName();
		}
		return ProcessCalculatedColumns(dataTable, dataSource);
	}

	internal static DataTable ProcessCalculatedColumns(DataTable dataTable, IStiAppDataSource dataSource)
	{
		if (dataTable == null)
		{
			return null;
		}
		IEnumerable<IStiAppDataColumn> enumerable = from c in dataSource.FetchColumns()
			where c is IStiAppCalcDataColumn
			select c;
		if (!enumerable.Any())
		{
			return dataTable;
		}
		foreach (IStiAppDataColumn item in enumerable)
		{
			if (!dataTable.Columns.Contains(item.GetName()))
			{
				dataTable.Columns.Add(item.GetName(), item.GetDataType());
			}
		}
		string[] names = enumerable.Select((IStiAppDataColumn c) => c.GetName()).ToArray();
		object[][] array = dataSource.FetchColumnValues(names).ToArray();
		if (!array.Any())
		{
			return dataTable;
		}
		try
		{
			dataTable.BeginLoadData();
			IEnumerable<int> enumerable2 = enumerable.Select((IStiAppDataColumn c) => dataTable.Columns.IndexOf(c.GetName()));
			int num = 0;
			foreach (DataRow row in dataTable.Rows)
			{
				try
				{
					object[] array2 = ((num >= array.Length) ? array[^1] : array[num]);
					row.BeginEdit();
					int num2 = 0;
					foreach (int item2 in enumerable2)
					{
						if (num2 < array2.Length)
						{
							row[item2] = array2[num2++];
							continue;
						}
						break;
					}
					row.EndEdit();
				}
				catch
				{
				}
				num++;
			}
		}
		finally
		{
			dataTable.AcceptChanges();
			dataTable.EndLoadData();
		}
		return dataTable;
	}

	internal static void AddTableNameToColumnNames(DataTable table, IStiAppDataSource dataSource)
	{
		if (table == null)
		{
			return;
		}
		IEnumerable<IStiAppDataColumn> source = dataSource.FetchColumns();
		string text = table.TableName + ".";
		foreach (DataColumn tableColumn in table.Columns)
		{
			if (!tableColumn.ColumnName.StartsWith(text))
			{
				IStiAppDataColumn stiAppDataColumn = source.FirstOrDefault((IStiAppDataColumn c) => c.GetNameInSource() == tableColumn.ColumnName);
				tableColumn.ColumnName = ((stiAppDataColumn?.GetName() != stiAppDataColumn?.GetNameInSource()) ? (text + stiAppDataColumn.GetName()) : (text + tableColumn.ColumnName));
			}
		}
	}

	public static DataTable GetFromCache(IStiAppDataSource dataSource)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(dataSource);
			return cache.ContainsKey(cacheKey) ? cache[cacheKey] : null;
		}
	}

	public static bool ExistsInCache(IStiAppDataSource dataSource)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(dataSource);
			return cache.ContainsKey(cacheKey);
		}
	}

	internal static void AddToCache(IStiAppDataSource dataSource, DataTable dataTable)
	{
		lock (lockObject)
		{
			if (dataTable == null)
			{
				dataTable = DataTableExt.NullTable;
			}
			string cacheKey = GetCacheKey(dataSource);
			cache[cacheKey] = dataTable;
		}
	}

	private static string GetCacheKey(IStiAppDataSource dataSource)
	{
		if (dataSource == null)
		{
			return null;
		}
		string text = StiAppKey.GetOrGeneratedKey(dataSource) ?? string.Empty;
		IStiAppConnection stiAppConnection = dataSource?.GetConnection();
		string text2 = ((dataSource.GetNameInSource() == dataSource.GetName()) ? dataSource.GetName() : (dataSource.GetNameInSource() + "." + dataSource.GetName()));
		if (stiAppConnection != null)
		{
			return text + "." + stiAppConnection.GetName() + "." + text2;
		}
		return text + "." + text2;
	}

	public static void CleanCache(string appKey)
	{
		lock (lockObject)
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
