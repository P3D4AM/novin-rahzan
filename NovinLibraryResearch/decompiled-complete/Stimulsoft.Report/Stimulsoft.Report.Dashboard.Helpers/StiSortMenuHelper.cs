using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard.Helpers;

internal static class StiSortMenuHelper
{
	internal static string GetSortElementName(string key, IStiElement element)
	{
		if (element is IStiTableElement stiTableElement)
		{
			IStiMeter val = ((IEnumerable<IStiMeter>)stiTableElement.FetchAllMeters()).FirstOrDefault((Func<IStiMeter, bool>)((IStiMeter t) => t.Key == key));
			if (val != null)
			{
				int num = stiTableElement.FetchAllMeters().IndexOf(val);
				if (!string.IsNullOrWhiteSpace(val.Label))
				{
					return val.Label;
				}
				return val.Expression;
			}
		}
		return Loc.Get("FormBand", "NoSort");
	}

	public static bool IsAllowUserSorting(IStiElement element)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		IStiDataTopN val = (IStiDataTopN)((element is IStiDataTopN) ? element : null);
		if (val != null && (int)val.TopN.Mode != 0)
		{
			return false;
		}
		if (element is IStiElementInteraction { DashboardInteraction: IStiAllowUserSortingDashboardInteraction { AllowUserSorting: false } })
		{
			return false;
		}
		if (element is IStiChartElement stiChartElement)
		{
			if ((!stiChartElement.IsAxisAreaChart || stiChartElement.IsParetoChart || stiChartElement.IsScatterChart) && !stiChartElement.IsPieChart && !stiChartElement.IsPie3dChart)
			{
				return stiChartElement.IsDoughnutChart;
			}
			return true;
		}
		if (element is IStiGaugeElement && (element as IStiGaugeElement)?.GetSeries() != null)
		{
			IStiManuallyEnteredData obj = element as IStiManuallyEnteredData;
			if (obj != null && obj.DataMode == StiDataMode.UsingDataFields)
			{
				return true;
			}
		}
		if (element is IStiProgressElement && (element as IStiProgressElement)?.GetSeries() != null)
		{
			IStiManuallyEnteredData obj2 = element as IStiManuallyEnteredData;
			if (obj2 != null && obj2.DataMode == StiDataMode.UsingDataFields)
			{
				return true;
			}
		}
		if (element is IStiIndicatorElement && (element as IStiIndicatorElement)?.GetSeries() != null)
		{
			IStiManuallyEnteredData obj3 = element as IStiManuallyEnteredData;
			if (obj3 != null && obj3.DataMode == StiDataMode.UsingDataFields)
			{
				return true;
			}
		}
		return false;
	}
}
