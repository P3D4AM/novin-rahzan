using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

internal static class StiDrillDownHelper
{
	internal static bool IsDrillAvailable(IStiElement element)
	{
		if (!((element as IStiElementInteraction)?.DashboardInteraction is IStiAllowUserDrillDownDashboardInteraction { AllowUserDrillDown: not false }))
		{
			return false;
		}
		IStiDrillDownElement stiDrillDownElement = element as IStiDrillDownElement;
		return stiDrillDownElement != null;
	}

	internal static bool IsDrillUpAvailable(IStiElement element)
	{
		if (!IsDrillAvailable(element))
		{
			return false;
		}
		IStiDrillDownElement stiDrillDownElement = element as IStiDrillDownElement;
		return stiDrillDownElement.DrillDownCurrentLevel > 0;
	}

	internal static bool IsDrillDownAvailable(IStiElement element)
	{
		if (!IsDrillAvailable(element))
		{
			return false;
		}
		IStiDrillDownElement stiDrillDownElement = element as IStiDrillDownElement;
		return stiDrillDownElement.DrillDownCurrentLevel < stiDrillDownElement.DrillDownLevelCount - 1;
	}

	internal static void DrillUp(IStiElement element)
	{
		IStiDrillDownElement stiDrillDownElement = element as IStiDrillDownElement;
		stiDrillDownElement.DrillDownCurrentLevel--;
		stiDrillDownElement.DrillDownFilters = stiDrillDownElement.DrillDownFiltersList[stiDrillDownElement.DrillDownCurrentLevel];
		stiDrillDownElement.DrillDownFiltersList.RemoveAt(stiDrillDownElement.DrillDownFiltersList.Count - 1);
	}

	internal static void DrillDown(IStiElement element, List<StiDataFilterRule> drillDownFilters = null)
	{
		IStiDrillDownElement stiDrillDownElement = element as IStiDrillDownElement;
		stiDrillDownElement.DrillDownFiltersList.Add(stiDrillDownElement.DrillDownFilters);
		List<StiDataFilterRule> list = stiDrillDownElement.DrillDownFilters?.ToList();
		if (list == null)
		{
			list = new List<StiDataFilterRule>();
		}
		if (drillDownFilters != null)
		{
			list.AddRange(drillDownFilters);
		}
		stiDrillDownElement.DrillDownFilters = list;
		stiDrillDownElement.DrillDownCurrentLevel++;
	}
}
