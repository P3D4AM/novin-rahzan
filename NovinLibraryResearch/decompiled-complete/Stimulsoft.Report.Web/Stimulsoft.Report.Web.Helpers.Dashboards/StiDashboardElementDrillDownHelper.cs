using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiDashboardElementDrillDownHelper
{
	public static void ApplyDashboardElementDrillDown(StiReport report, Hashtable parameters)
	{
		if (parameters != null && parameters.Count != 0)
		{
			IStiElement element = report.Pages.GetComponentByName(parameters["elementName"] as string) as IStiElement;
			ArrayList filters = parameters["filters"] as ArrayList;
			ApplyDrillDownToElement(element, filters);
		}
	}

	public static void ApplyDrillDownToElement(IStiElement element, ArrayList filters)
	{
		if (element == null || filters == null || !(element is IStiDrillDownElement stiDrillDownElement))
		{
			return;
		}
		List<StiDataFilterRule> list = new List<StiDataFilterRule>();
		foreach (Hashtable filter in filters)
		{
			StiDataFilterCondition condition = (StiDataFilterCondition)Enum.Parse(typeof(StiDataFilterCondition), filter["condition"] as string);
			list.Add(new StiDataFilterRule(filter["key"] as string, filter["path"] as string, filter["path2"] as string, condition, filter["value"] as string, filter["value2"] as string, isEnabled: true, isExpression: false));
		}
		stiDrillDownElement.DrillDownFiltersList.Add(stiDrillDownElement.DrillDownFilters);
		List<StiDataFilterRule> list2 = stiDrillDownElement.DrillDownFilters?.ToList();
		if (list2 == null)
		{
			list2 = new List<StiDataFilterRule>();
		}
		if (list != null && list.Count > 0)
		{
			list2.AddRange(list);
		}
		stiDrillDownElement.DrillDownFilters = list2;
		stiDrillDownElement.DrillDownCurrentLevel++;
	}

	public static void ApplyDashboardElementDrillUp(StiReport report, Hashtable parameters)
	{
		if (parameters != null && parameters.Count != 0)
		{
			IStiElement element = report.Pages.GetComponentByName(parameters["elementName"] as string) as IStiElement;
			ApplyDrillUpToElement(element);
		}
	}

	public static void ApplyDrillUpToElement(IStiElement element)
	{
		if (element != null && element is IStiDrillDownElement stiDrillDownElement)
		{
			stiDrillDownElement.DrillDownCurrentLevel--;
			if (stiDrillDownElement.DrillDownCurrentLevel < 0)
			{
				stiDrillDownElement.DrillDownCurrentLevel = 0;
			}
			if (stiDrillDownElement.DrillDownFiltersList.Count > 0)
			{
				stiDrillDownElement.DrillDownFilters = stiDrillDownElement.DrillDownFiltersList[stiDrillDownElement.DrillDownCurrentLevel];
				stiDrillDownElement.DrillDownFiltersList.RemoveAt(stiDrillDownElement.DrillDownFiltersList.Count - 1);
			}
		}
	}
}
