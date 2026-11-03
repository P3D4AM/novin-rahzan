using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public static class StiDataLeader
{
	private static object lockObject = new object();

	private static List<StiDataSource> FetchAll(StiDatabase database, StiDictionary dictionary)
	{
		return (from d in dictionary.DataSources.ToList()
			where d.GetCategoryName()?.ToLowerInvariant() == database.Name?.ToLowerInvariant()
			select d).ToList();
	}

	public static void RegData(StiDatabase database, StiDictionary dictionary, bool loadData)
	{
		if (!ExistsInCache(database, dictionary))
		{
			database?.RegData(dictionary, loadData);
			RegDataAfter(database, dictionary, loadData);
		}
	}

	private static void RegDataAfter(StiDatabase database, StiDictionary dictionary, bool loadData)
	{
		if (!StiBIDataCacheOptions.Enabled)
		{
			return;
		}
		if (database is StiFileDatabase)
		{
			loadData = true;
		}
		if (!loadData)
		{
			return;
		}
		List<StiDataSource> list = FetchAll(database, dictionary);
		foreach (StiDataSource item in list)
		{
			if (StiBIDataCacheHelper.Exists((IStiAppDataSource)(object)item))
			{
				continue;
			}
			lock (lockObject)
			{
				item.Connect(loadData);
				if (item.DataTable != null && item.DataTable.Columns.Count > 0)
				{
					StiBIDataCacheHelper.SaveData((IStiAppDataSource)(object)item, item.DataTable);
				}
				item.Disconnect();
			}
		}
	}

	public static bool ExistsInCache(StiDatabase database, StiDictionary dictionary)
	{
		if (!StiBIDataCacheOptions.Enabled)
		{
			return false;
		}
		return FetchAll(database, dictionary)?.All((Func<StiDataSource, bool>)StiBIDataCacheHelper.Exists) ?? false;
	}

	public static StiDataSchema RetrieveSchema(StiFileDataConnector connector, StiFileDataOptions options)
	{
		if (connector == null)
		{
			return null;
		}
		return connector.RetrieveSchema(options);
	}

	public static StiDataSchema RetrieveSchema(StiSqlDataConnector connector, bool allowException = false)
	{
		if (connector == null)
		{
			return null;
		}
		return connector.RetrieveSchema(allowException);
	}

	public static DataSet GetDataSet(StiFileDataConnector connector, StiFileDataOptions options)
	{
		if (connector == null)
		{
			return null;
		}
		return connector.GetDataSet(options);
	}

	public static StiDataColumnsCollection GetColumnsFromData(StiDataAdapterService adapter, StiData data, StiDataSource dataSource, CommandBehavior retrieveMode = CommandBehavior.SchemaOnly)
	{
		StiDataColumnsCollection stiDataColumnsCollection = adapter?.GetColumnsFromData(data, dataSource, retrieveMode);
		if (stiDataColumnsCollection != null)
		{
			return stiDataColumnsCollection;
		}
		return new StiDataColumnsCollection();
	}

	public static void ConnectDataSourceToData(StiDataAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		if (StiBIDataCacheOptions.Enabled && StiBIDataCacheHelper.Exists((IStiAppDataSource)(object)dataSource))
		{
			dataSource.DataTable = StiBIDataCacheHelper.Get((IStiAppDataSource)(object)dataSource, loadData);
			return;
		}
		adapter?.ConnectDataSourceToData(dictionary, dataSource, loadData);
		if (StiBIDataCacheOptions.Enabled && dataSource.DataTable != null && dataSource.DataTable.Columns.Count > 0 && loadData && dataSource.DataTable.Rows.Count > 0)
		{
			lock (lockObject)
			{
				StiBIDataCacheHelper.SaveData((IStiAppDataSource)(object)dataSource, dataSource.DataTable);
			}
		}
	}

	public static void RetrieveData(StiSqlSource dataSource, bool schemaOnly = false)
	{
		dataSource?.RetrieveData(schemaOnly);
	}

	public static void Connect(StiDataSource dataSource, bool loadData = true, bool invokeEvents = true)
	{
		dataSource?.Connect(loadData, invokeEvents);
	}

	public static void Connect(StiDataSource dataSource, StiDataCollection datas, bool loadData = true)
	{
		dataSource?.Connect(datas, loadData);
	}

	public static void Disconnect(StiDataSource dataSource)
	{
		dataSource?.Disconnect();
	}
}
