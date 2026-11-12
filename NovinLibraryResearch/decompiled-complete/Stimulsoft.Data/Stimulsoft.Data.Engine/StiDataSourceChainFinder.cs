using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Data.Engine;

public static class StiDataSourceChainFinder
{
	public static IEnumerable<IStiAppDataSource> Find(IEnumerable<IStiAppDataSource> dataSources)
	{
		dataSources = dataSources.Where((IStiAppDataSource d) => d != null);
		return dataSources.SelectMany((IStiAppDataSource primaryTable) => (from t in (from d in dataSources
				where d != primaryTable
				select d into t
				select Find(primaryTable, t) into t
				where t != null
				select t).SelectMany((IEnumerable<IStiAppDataSource> t) => t)
			where !dataSources.Contains(t)
			select t).Distinct()).Union(dataSources).Distinct();
	}

	public static IEnumerable<IStiAppDataSource> Find(IEnumerable<IStiAppDataSource> dataSources, string table1, string table2)
	{
		IStiAppDataSource item = dataSources.FirstOrDefault((IStiAppDataSource d) => d.GetName() == table1);
		IStiAppDataSource item2 = dataSources.FirstOrDefault((IStiAppDataSource d) => d.GetName() == table2);
		return Find(new List<IStiAppDataSource> { item, item2 });
	}

	private static IEnumerable<IStiAppDataSource> Find(IStiAppDataSource dataSource1, IStiAppDataSource dataSource2)
	{
		IEnumerable<IStiAppDataSource> enumerable = FindInParent(dataSource1, dataSource2);
		IEnumerable<IStiAppDataSource> enumerable2 = FindInChild(dataSource1, dataSource2);
		if (enumerable == null && enumerable2 == null)
		{
			return FindInBoth(dataSource1, dataSource2);
		}
		if (enumerable != null && enumerable2 == null)
		{
			return enumerable;
		}
		if (enumerable == null && enumerable2 != null)
		{
			return enumerable2;
		}
		if (enumerable.Count() < enumerable2.Count())
		{
			return enumerable;
		}
		return enumerable2;
	}

	public static IEnumerable<IStiAppDataSource> FindInParent(IStiAppDataSource dataSource1, IStiAppDataSource dataSource2, List<string> dataPath = null)
	{
		if (dataPath == null)
		{
			dataPath = new List<string>();
		}
		string dataPoint = GetDataPoint(dataSource1, dataSource2);
		if (dataPath.Contains(dataPoint))
		{
			return null;
		}
		dataPath.Add(dataPoint);
		IEnumerable<IStiAppDataRelation> enumerable = dataSource1.FetchParentRelations(activePreferred: true);
		if (!enumerable.Any())
		{
			return null;
		}
		IEnumerable<IStiAppDataRelation> source = enumerable.Where((IStiAppDataRelation r) => r.GetParentDataSource() == dataSource2);
		if (source.Any())
		{
			return new List<IStiAppDataSource> { dataSource1, dataSource2 };
		}
		foreach (IStiAppDataRelation activeRelation in GetActiveRelations(enumerable))
		{
			List<IStiAppDataSource> list = FindInParent(activeRelation.GetParentDataSource(), dataSource2)?.ToList();
			if (list != null)
			{
				list.Insert(0, activeRelation.GetChildDataSource());
				return list;
			}
		}
		return null;
	}

	public static IEnumerable<IStiAppDataSource> FindInChild(IStiAppDataSource dataSource1, IStiAppDataSource dataSource2, List<string> dataPath = null)
	{
		if (dataPath == null)
		{
			dataPath = new List<string>();
		}
		string dataPoint = GetDataPoint(dataSource1, dataSource2);
		if (dataPath.Contains(dataPoint))
		{
			return null;
		}
		dataPath.Add(dataPoint);
		IEnumerable<IStiAppDataRelation> enumerable = dataSource1.FetchChildRelations(activePreferred: true);
		if (!enumerable.Any())
		{
			return FindInParent(dataSource1, dataSource2);
		}
		IStiAppDataRelation stiAppDataRelation = enumerable.FirstOrDefault((IStiAppDataRelation r) => r.GetChildDataSource() == dataSource2);
		if (stiAppDataRelation != null)
		{
			return new List<IStiAppDataSource> { dataSource1, dataSource2 };
		}
		foreach (IStiAppDataRelation activeRelation in GetActiveRelations(enumerable))
		{
			List<IStiAppDataSource> list = FindInChild(activeRelation.GetChildDataSource(), dataSource2, dataPath)?.ToList();
			if (list != null)
			{
				list.Insert(0, activeRelation.GetParentDataSource());
				return list;
			}
		}
		return null;
	}

	public static IEnumerable<IStiAppDataSource> FindInBoth(IStiAppDataSource dataSource1, IStiAppDataSource dataSource2)
	{
		IEnumerable<IStiAppDataRelation> enumerable = dataSource1.FetchParentRelations(activePreferred: true);
		IEnumerable<IStiAppDataRelation> source = dataSource2.FetchParentRelations(activePreferred: true);
		foreach (IStiAppDataRelation rel3 in enumerable)
		{
			IStiAppDataRelation stiAppDataRelation = source.FirstOrDefault((IStiAppDataRelation rel2) => rel3.GetParentDataSource() == rel2.GetParentDataSource());
			if (stiAppDataRelation != null)
			{
				return new List<IStiAppDataSource> { rel3?.GetParentDataSource() };
			}
		}
		return null;
	}

	private static string GetDataPoint(IStiAppDataSource dataSource1, IStiAppDataSource dataSource2)
	{
		return dataSource1.GetName() + dataSource2.GetName();
	}

	private static IEnumerable<IStiAppDataRelation> GetActiveRelations(IEnumerable<IStiAppDataRelation> childRelations)
	{
		if (childRelations.Any((IStiAppDataRelation r) => r.GetActiveState()))
		{
			return childRelations.Where((IStiAppDataRelation r) => r.GetActiveState());
		}
		return childRelations;
	}
}
