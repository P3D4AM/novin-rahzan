using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiChartElementViewHelper
{
	private static bool IsAllowInteractive(IStiChartElement chartElement)
	{
		if (chartElement.GetArgumentByIndex(0) == null)
		{
			return chartElement.GetSeries() != null;
		}
		return true;
	}

	public static string GetArgumentColumnPath(IStiChartElement chartElement)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		int index = 0;
		if (chartElement is IStiDrillDownElement)
		{
			index = ((IStiDrillDownElement)chartElement).DrillDownCurrentLevel;
		}
		IStiMeter argumentByIndex = chartElement.GetArgumentByIndex(index);
		if (argumentByIndex != null)
		{
			expression = StiExpressionHelper.RemoveFunction(argumentByIndex.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as IStiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	public static string GetSeriesColumnPath(IStiChartElement chartElement)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		IStiMeter series = chartElement.GetSeries();
		if (series != null)
		{
			expression = StiExpressionHelper.RemoveFunction(series.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as IStiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	public static string GetBubleXColumnPath(IStiChartElement chartElement)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		IStiMeter argumentByIndex = chartElement.GetArgumentByIndex(0);
		if (argumentByIndex != null)
		{
			expression = StiExpressionHelper.RemoveFunction(argumentByIndex.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as IStiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	public static string GetBubleYColumnPath(IStiChartElement chartElement)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		IStiMeter valueByIndex = chartElement.GetValueByIndex(0);
		if (valueByIndex != null)
		{
			expression = StiExpressionHelper.RemoveFunction(valueByIndex.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as IStiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	public static List<double?[]> GetSeriesValues(StiChart chart)
	{
		List<double?[]> list = new List<double?[]>();
		foreach (IStiSeries item in chart.Series)
		{
			list.Add(item.Values);
		}
		return list;
	}

	public static object GetChartValuesFromCache(string cacheGuid, StiRequestParams requestParams)
	{
		return requestParams.Cache.Helper.GetObjectInternal(requestParams, cacheGuid);
	}

	public static void SaveChartValuesToCache(string cacheGuid, StiPage page, StiRequestParams requestParams)
	{
		foreach (object component in page.Components)
		{
			if (component is StiChart)
			{
				requestParams.Cache.Helper.SaveObjectInternal(GetSeriesValues(component as StiChart), requestParams, cacheGuid);
			}
		}
	}

	public static object GetChartAnimationsFromCache(string cacheGuid, StiRequestParams requestParams)
	{
		return requestParams.Cache.Helper.GetObjectInternal(requestParams, cacheGuid + "_animations");
	}

	public static void SaveChartAnimationsToCache(IStiElement element, string cacheGuid, StiPage page, StiRequestParams requestParams)
	{
		if (element is IStiChartElement)
		{
			{
				foreach (object component in page.Components)
				{
					if (component is StiChart stiChart)
					{
						requestParams.Cache.Helper.SaveObjectInternal(stiChart.PreviousAnimations, requestParams, cacheGuid + "_animations");
					}
				}
				return;
			}
		}
		if (element is IStiGaugeElement stiGaugeElement)
		{
			requestParams.Cache.Helper.SaveObjectInternal(stiGaugeElement.PreviousAnimations, requestParams, cacheGuid + "_animations");
		}
	}

	public static void RemoveChartAnimationsFromCache(string cacheGuid, StiRequestParams requestParams)
	{
		requestParams.Cache.Helper.RemoveObject(cacheGuid + "_animations");
	}

	public static bool IsBubble(IStiChartElement chartElement)
	{
		IStiMeter valueByIndex = chartElement.GetValueByIndex(0);
		if (valueByIndex != null)
		{
			object propertyValue = StiInvokeMethodsHelper.GetPropertyValue(valueByIndex, "SeriesType");
			if (propertyValue != null && propertyValue.ToString() == "Bubble")
			{
				return true;
			}
		}
		return false;
	}

	internal static ArrayList GetUserViewStates(IStiChartElement chartElement)
	{
		ArrayList viewStates = new ArrayList();
		(chartElement as IStiUserViewStates).UserViewStates.ForEach(delegate(StiUserViewState viewState)
		{
			viewStates.Add(new Hashtable
			{
				["name"] = viewState.Name,
				["key"] = viewState.Key,
				["seriesType"] = viewState.SeriesType
			});
		});
		return viewStates;
	}

	internal static Hashtable ChangeChartElementViewState(StiReport report, StiRequestParams requestParams)
	{
		string @string = requestParams.GetString("chartElementName");
		string string2 = requestParams.GetString("chartElementViewStateKey");
		IStiChartElement stiChartElement = ((report != null && @string != null) ? (report.Pages.GetComponentByName(@string) as IStiChartElement) : null);
		if (stiChartElement != null)
		{
			IStiUserViewStates stiUserViewStates = stiChartElement as IStiUserViewStates;
			stiUserViewStates.SwitchSelectedViewState(string2);
			List<IStiElement> elements = (from e in ((IStiDashboard)stiChartElement.Page).GetElements(nested: true)
				where e.IsEnabled
				select e).ToList();
			bool renderSingleElement = !string.IsNullOrEmpty(requestParams.Viewer.ElementName) && requestParams.Action != StiAction.DashboardDrillDown;
			int totalFixedHeight = 0;
			List<StiRangeBand> bands = null;
			StiReportHelper.CalculatePositionForEachBand(requestParams, elements, stiChartElement.Page, out bands, out totalFixedHeight, stiChartElement as StiComponent);
			return StiReportHelper.GetElementAttributes(stiChartElement.Page, stiChartElement, renderSingleElement, requestParams, bands, totalFixedHeight);
		}
		return null;
	}
}
