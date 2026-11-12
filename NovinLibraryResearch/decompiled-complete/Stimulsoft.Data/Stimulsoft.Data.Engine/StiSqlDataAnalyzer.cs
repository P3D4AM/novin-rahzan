using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Data.Engine;

public static class StiSqlDataAnalyzer
{
	public static DataTable FetchAndJoin(IStiQueryObject query, string group, IEnumerable<string> filterNames, IEnumerable<StiDataFilterRule> dataFilters, IStiApp app)
	{
		string query2 = BuildSqlQuery(query, group, filterNames, dataFilters, app);
		return StiBIDataCacheHelper.RunQuery(query2);
	}

	public static string BuildSqlQuery(IStiQueryObject query, string group, IEnumerable<string> filterNames, IEnumerable<StiDataFilterRule> dataFilters, IStiApp app)
	{
		string orGeneratedKey = StiAppKey.GetOrGeneratedKey(app);
		IEnumerable<IStiAppDataSource> enumerable = StiDataPicker.RetrieveUsedDataSources(query, group, filterNames);
		List<string> list = enumerable.Select((IStiAppDataSource d) => d.GetKey() ?? "").ToList();
		List<string> list2 = enumerable.Select(StiBIDataCacheHelper.GetTableName).ToList();
		List<StiDataLink> links = StiDataLinkHelper.GetLinks(app.GetDictionary());
		string text = BuildSelectQuery(enumerable, app);
		string text2 = list.FirstOrDefault();
		List<StiDataLink> list3 = StiDataJoiner.FindPath(list2, links);
		string text3 = BuildJoinQuery(list3, list2, list);
		if (!string.IsNullOrWhiteSpace(text3))
		{
			int index = list2.IndexOf(list3.FirstOrDefault().ParentTable);
			text2 = list[index];
		}
		return "SELECT " + text + " FROM [" + text2 + "] " + text3;
	}

	public static string BuildJoinQuery(List<StiDataLink> path, List<string> realDataNames, List<string> dataNames)
	{
		if (path == null || !path.Any())
		{
			return "";
		}
		IEnumerable<string> values = path.Select(delegate(StiDataLink r)
		{
			int index = realDataNames.IndexOf(r.ParentTable);
			int index2 = realDataNames.IndexOf(r.ChildTable);
			string text = dataNames[index];
			string text2 = dataNames[index2];
			string text3 = "[" + text + "].[" + r.ParentColumn + "]";
			string text4 = "[" + text2 + "].[" + r.ChildColumn + "]";
			return "LEFT JOIN [" + text2 + "] ON " + text3 + " = " + text4;
		});
		return string.Join(" ", values);
	}

	public static string BuildSelectQuery(IEnumerable<IStiAppDataSource> dataSources, IStiApp app)
	{
		string orGeneratedKey = StiAppKey.GetOrGeneratedKey(app);
		List<string> tableNames = dataSources.Select(StiBIDataCacheHelper.GetTableName).ToList();
		int tableIndex = 0;
		IEnumerable<string> values = dataSources.Select(delegate(IStiAppDataSource d)
		{
			string tableName = tableNames[tableIndex++];
			IEnumerable<string> values2 = d.FetchColumns().Select(delegate(IStiAppDataColumn c)
			{
				string nameInSource = c.GetNameInSource();
				return "[" + d.GetKey() + "].[" + nameInSource + "] as [" + tableName + "." + nameInSource + "]";
			});
			return string.Join(", ", values2);
		});
		return string.Join(", ", values);
	}
}
