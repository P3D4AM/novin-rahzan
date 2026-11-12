using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Data.Engine;

public static class StiDataAnalyzer
{
	public static StiDataTable Analyze(IStiQueryObject query, string group, IEnumerable<IStiMeter> meters, StiDataRequestOption option = StiDataRequestOption.All, IEnumerable<StiDataSortRule> userSorts = null, IEnumerable<StiDataFilterRule> userFilters = null, IEnumerable<StiDataFilterRule> dataFilters = null, IEnumerable<StiDataActionRule> dataActions = null, IEnumerable<StiDataSortRule> transformSorts = null, IEnumerable<StiDataFilterRule> transformFilters = null, IEnumerable<StiDataActionRule> transformActions = null, IEnumerable<StiDataFilterRule> drillDownFilters = null)
	{
		if (meters == null || !meters.Any())
		{
			return StiDataTable.NullTable;
		}
		IStiAppDictionary dictionary = query.GetDictionary();
		IStiReport stiReport = dictionary?.GetApp() as IStiReport;
		int num = 0;
		IEnumerable<string> namesTo = UnionNames(null, userFilters?.Select((StiDataFilterRule f) => f.Path));
		namesTo = UnionNames(namesTo, dataFilters?.Select((StiDataFilterRule f) => f.Path));
		namesTo = UnionNames(namesTo, dataActions?.Select((StiDataActionRule f) => f.Path));
		namesTo = UnionNames(namesTo, transformFilters?.Select((StiDataFilterRule f) => f.Path));
		namesTo = UnionNames(namesTo, transformActions?.Select((StiDataActionRule f) => f.Path));
		namesTo = UnionNames(namesTo, drillDownFilters?.Select((StiDataFilterRule f) => f.Path));
		List<StiDataLink> links = StiDataLinkHelper.GetLinks(dictionary);
		IEnumerable<DataTable> enumerable = StiDataPicker.Fetch(query, group, option, namesTo, links);
		if (enumerable == null || !enumerable.Any())
		{
			return StiDataCreator.Create(dictionary, meters);
		}
		DataTable dataTable = StiDataJoiner.Join(enumerable, links, meters, stiReport);
		if (dataTable == null)
		{
			return StiDataTable.NullTable;
		}
		List<StiDataFilterRule> list = UnionFilters(dataFilters, userFilters, drillDownFilters);
		num = group?.GetHashCode() ?? 0;
		num = GetUniqueCode(stiReport, list, num);
		dataTable = StiDataFiltrator.Filter(dataTable, list, stiReport, num);
		if (dataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, dataActions?.Where((StiDataActionRule a) => a.Priority == StiDataActionPriority.BeforeTransformation), num);
		dataTable = StiDataActionOperator.Apply(dataTable, dataActions, stiReport, num);
		if (dataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(meters, num);
		StiDataTable stiDataTable = StiDataGrouper.Group(dictionary, dataTable, meters.ToList());
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, dataActions?.Where((StiDataActionRule a) => a.Priority == StiDataActionPriority.AfterGroupingData), num);
		stiDataTable = StiDataActionOperator.ApplyAfterTransformation(stiDataTable, dataActions, StiDataActionPriority.AfterGroupingData, stiReport, num);
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, transformFilters, num);
		stiDataTable = StiDataFiltrator.Filter(stiDataTable, transformFilters, stiReport, num);
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, transformActions?.Where((StiDataActionRule a) => a.Priority == StiDataActionPriority.AfterGroupingData), num);
		stiDataTable = StiDataActionOperator.ApplyAfterTransformation(stiDataTable, transformActions, StiDataActionPriority.AfterGroupingData, stiReport, num);
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, transformSorts, num);
		stiDataTable = StiDataSorter.Sort(stiDataTable, transformSorts, stiReport, num);
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, transformActions?.Where((StiDataActionRule a) => a.Priority == StiDataActionPriority.AfterSortingData), num);
		stiDataTable = StiDataActionOperator.ApplyAfterTransformation(stiDataTable, transformActions, StiDataActionPriority.AfterSortingData, stiReport, num);
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, userSorts, num);
		stiDataTable = StiDataSorter.Sort(stiDataTable, userSorts, stiReport, num);
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		num = GetUniqueCode(stiReport, dataActions?.Where((StiDataActionRule a) => a.Priority == StiDataActionPriority.AfterSortingData), num);
		stiDataTable = StiDataActionOperator.ApplyAfterTransformation(stiDataTable, dataActions, StiDataActionPriority.AfterSortingData, stiReport, num);
		if (stiDataTable == null)
		{
			return StiDataTable.NullTable;
		}
		return stiDataTable;
	}

	private static List<StiDataFilterRule> UnionFilters(IEnumerable<StiDataFilterRule> dataFilters, IEnumerable<StiDataFilterRule> userFilters, IEnumerable<StiDataFilterRule> drillDownFilters)
	{
		if (dataFilters == null && userFilters == null && drillDownFilters == null)
		{
			return null;
		}
		List<StiDataFilterRule> list = new List<StiDataFilterRule>();
		if (dataFilters != null)
		{
			list = list.Union(dataFilters).ToList();
		}
		if (userFilters != null)
		{
			list = list.Union(userFilters).ToList();
		}
		if (drillDownFilters != null)
		{
			list = list.Union(drillDownFilters).ToList();
		}
		return list;
	}

	private static IEnumerable<string> UnionNames(IEnumerable<string> namesTo, IEnumerable<string> namesFrom)
	{
		if (namesTo == null && namesFrom == null)
		{
			return null;
		}
		if (namesTo != null && namesFrom != null)
		{
			return namesTo.Union(namesFrom);
		}
		return namesTo ?? namesFrom;
	}

	private static int GetUniqueCode(IStiApp app, IEnumerable<StiDataRule> rules, int? initialHash = null)
	{
		if (rules == null || !rules.Any())
		{
			return initialHash.GetValueOrDefault(0);
		}
		int num = rules.Select((StiDataRule r) => r.GetUniqueCode()).Aggregate(0, (int r1, int r2) => r1 + r2);
		if (rules.All((StiDataRule r) => r is StiDataFilterRule))
		{
			int filterRulesHash = StiDataFilterRuleHelper.GetFilterRulesHash(app, rules.Cast<StiDataFilterRule>());
			num += filterRulesHash;
		}
		if (!initialHash.HasValue)
		{
			return num;
		}
		return num + initialHash.Value;
	}

	private static int GetUniqueCode(IEnumerable<IStiMeter> meters, int? initialHash = null)
	{
		if (meters == null || !meters.Any())
		{
			return initialHash.GetValueOrDefault(0);
		}
		int num = meters.Select((IStiMeter c) => c.GetUniqueCode()).Aggregate(0, (int c1, int c2) => c1 + c2);
		if (!initialHash.HasValue)
		{
			return num;
		}
		return num + initialHash.Value;
	}
}
