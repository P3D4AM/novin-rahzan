using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

internal static class StiChartElementFilterHelper
{
	private static bool IsAllowInteractive(StiChartElement chartElement)
	{
		if (chartElement.Arguments.FirstOrDefault() == null)
		{
			return chartElement.Series != null;
		}
		return true;
	}

	internal static List<StiDataFilterRule> GetElementGeomFilter(StiChartElement chartElement, StiSeriesElementGeom geom, bool isDrillDown = false)
	{
		List<StiDataFilterRule> list = new List<StiDataFilterRule>();
		if (geom.Series is StiBubbleSeries)
		{
			list.Add(new StiDataFilterRule(GetBubbleXColumnPath(chartElement), StiDataFilterCondition.EqualTo, GetBubbleXArgument(geom)));
			list.Add(new StiDataFilterRule(GetBubbleYColumnPath(chartElement), StiDataFilterCondition.EqualTo, GetBubbleYValue(geom)));
		}
		else
		{
			int argumentIndex = (isDrillDown ? chartElement.DrillDownCurrentLevel : 0);
			string argumentColumnPath = GetArgumentColumnPath(chartElement, argumentIndex);
			string seriesColumnPath = GetSeriesColumnPath(chartElement);
			if (!string.IsNullOrEmpty(argumentColumnPath) && string.IsNullOrEmpty(seriesColumnPath))
			{
				StiDataFilterRule item = new StiDataFilterRule(geom.ElementIndex, argumentColumnPath, StiDataFilterCondition.EqualTo, GetValue(geom));
				list.Add(item);
			}
			else if (string.IsNullOrEmpty(argumentColumnPath) && !string.IsNullOrEmpty(seriesColumnPath))
			{
				StiDataFilterRule item2 = new StiDataFilterRule(geom.ElementIndex, seriesColumnPath, StiDataFilterCondition.EqualTo, GetSeries(geom));
				list.Add(item2);
			}
			else if (!string.IsNullOrEmpty(argumentColumnPath) && !string.IsNullOrEmpty(seriesColumnPath))
			{
				StiDataFilterRule item3 = new StiDataFilterRule(geom.ElementIndex, argumentColumnPath, seriesColumnPath, StiDataFilterCondition.PairEqualTo, GetValue(geom), GetSeries(geom), isEnabled: true, isExpression: false);
				list.Add(item3);
			}
		}
		return list;
	}

	private static string GetArgumentColumnPath(StiChartElement chartElement, int argumentIndex = 0)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		List<StiArgumentChartMeter> arguments = chartElement.Arguments;
		arguments = ((arguments != null && argumentIndex < arguments.Count) ? arguments.GetRange(argumentIndex, 1) : arguments);
		StiArgumentChartMeter stiArgumentChartMeter = arguments.FirstOrDefault();
		if (stiArgumentChartMeter != null)
		{
			expression = StiExpressionHelper.RemoveFunction(stiArgumentChartMeter.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as StiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	private static string GetSeriesColumnPath(StiChartElement chartElement)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		StiSeriesChartMeter series = chartElement.Series;
		if (series != null)
		{
			expression = StiExpressionHelper.RemoveFunction(series.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as StiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	private static string GetBubbleXColumnPath(StiChartElement chartElement)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		StiArgumentChartMeter stiArgumentChartMeter = chartElement.Arguments.FirstOrDefault();
		if (stiArgumentChartMeter != null)
		{
			expression = StiExpressionHelper.RemoveFunction(stiArgumentChartMeter.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as StiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	private static string GetBubbleYColumnPath(StiChartElement chartElement)
	{
		if (!IsAllowInteractive(chartElement))
		{
			return null;
		}
		string expression = null;
		StiValueChartMeter stiValueChartMeter = chartElement.Values.FirstOrDefault();
		if (stiValueChartMeter != null)
		{
			expression = StiExpressionHelper.RemoveFunction(stiValueChartMeter.Expression);
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(chartElement.Page as StiDashboard, expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}

	private static string GetSeries(StiSeriesElementGeom g)
	{
		return g.Interaction.Series.CoreTitle;
	}

	private static string GetValue(object value)
	{
		object obj = ((!ListExt.IsList(value)) ? value : ListExt.ToList(value)?.FirstOrDefault());
		if (obj != null && obj != DBNull.Value)
		{
			return obj.ToString();
		}
		return null;
	}

	private static string GetValue(StiSeriesElementGeom g)
	{
		object[] array = (g.Interaction?.Series as StiSeries)?.OriginalArguments;
		if (array != null && g.Interaction.PointIndex >= 0 && g.Interaction.PointIndex < array.Length)
		{
			return GetValue(array[g.Interaction.PointIndex]);
		}
		return GetValue(g.Interaction?.Argument);
	}

	private static string GetBubbleXArgument(StiSeriesElementGeom g)
	{
		return g.Interaction.Argument?.ToString();
	}

	private static string GetBubbleYValue(StiSeriesElementGeom g)
	{
		return g.Interaction.Value?.ToString();
	}
}
