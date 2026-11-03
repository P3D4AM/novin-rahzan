using System;
using System.Collections;
using System.Collections.Generic;

namespace Stimulsoft.Report.Dashboard;

internal static class StiChartGroups
{
	private static Hashtable hash = new Hashtable();

	private static bool isInit;

	internal static bool SameGroup(StiChartSeriesType type1, StiChartSeriesType type2)
	{
		List<StiChartSeriesType> group = GetGroup(type1);
		if (group == null)
		{
			throw new NotSupportedException();
		}
		return group.Contains(type2);
	}

	internal static List<StiChartSeriesType> GetGroup(StiChartSeriesType type)
	{
		Init();
		foreach (object key in hash.Keys)
		{
			List<StiChartSeriesType> list = hash[key] as List<StiChartSeriesType>;
			if (list.Contains(type))
			{
				return list;
			}
		}
		return null;
	}

	internal static void Init()
	{
		if (isInit)
		{
			return;
		}
		lock (hash)
		{
			hash.Clear();
			hash["Bubble"] = new List<StiChartSeriesType> { StiChartSeriesType.Bubble };
			hash["Candlestick"] = new List<StiChartSeriesType> { StiChartSeriesType.Candlestick };
			hash["ClusteredBar"] = new List<StiChartSeriesType> { StiChartSeriesType.ClusteredBar };
			hash["ClusteredColumn"] = new List<StiChartSeriesType>
			{
				StiChartSeriesType.ClusteredColumn,
				StiChartSeriesType.Line,
				StiChartSeriesType.SteppedLine,
				StiChartSeriesType.Spline,
				StiChartSeriesType.Area,
				StiChartSeriesType.SteppedArea,
				StiChartSeriesType.SplineArea
			};
			hash["Doughnut"] = new List<StiChartSeriesType> { StiChartSeriesType.Doughnut };
			hash["FullStackedBar"] = new List<StiChartSeriesType> { StiChartSeriesType.FullStackedBar };
			hash["FullStackedColumn"] = new List<StiChartSeriesType>
			{
				StiChartSeriesType.FullStackedArea,
				StiChartSeriesType.FullStackedColumn,
				StiChartSeriesType.FullStackedLine,
				StiChartSeriesType.FullStackedSpline,
				StiChartSeriesType.FullStackedSplineArea
			};
			hash["Funnel"] = new List<StiChartSeriesType> { StiChartSeriesType.Funnel };
			hash["FunnelWeightedSlices"] = new List<StiChartSeriesType> { StiChartSeriesType.FunnelWeightedSlices };
			hash["Gantt"] = new List<StiChartSeriesType> { StiChartSeriesType.Gantt };
			hash["Pie"] = new List<StiChartSeriesType> { StiChartSeriesType.Pie };
			hash["Pie3d"] = new List<StiChartSeriesType> { StiChartSeriesType.Pie3d };
			hash["Radar"] = new List<StiChartSeriesType>
			{
				StiChartSeriesType.RadarArea,
				StiChartSeriesType.RadarLine,
				StiChartSeriesType.RadarPoint
			};
			hash["Range"] = new List<StiChartSeriesType>
			{
				StiChartSeriesType.Range,
				StiChartSeriesType.RangeBar,
				StiChartSeriesType.SplineRange,
				StiChartSeriesType.SteppedRange
			};
			hash["Scatter"] = new List<StiChartSeriesType>
			{
				StiChartSeriesType.Scatter,
				StiChartSeriesType.ScatterLine,
				StiChartSeriesType.ScatterSpline
			};
			hash["StackedBar"] = new List<StiChartSeriesType> { StiChartSeriesType.StackedBar };
			hash["StackedColumn"] = new List<StiChartSeriesType>
			{
				StiChartSeriesType.StackedArea,
				StiChartSeriesType.StackedColumn,
				StiChartSeriesType.StackedLine,
				StiChartSeriesType.StackedSpline,
				StiChartSeriesType.StackedSplineArea
			};
			hash["Stock"] = new List<StiChartSeriesType> { StiChartSeriesType.Stock };
			hash["Treemap"] = new List<StiChartSeriesType> { StiChartSeriesType.Treemap };
			hash["Pareto"] = new List<StiChartSeriesType> { StiChartSeriesType.Pareto };
			hash["Histogram"] = new List<StiChartSeriesType> { StiChartSeriesType.Histogram };
			hash["Pictorial"] = new List<StiChartSeriesType> { StiChartSeriesType.Pictorial };
			hash["Sunburst"] = new List<StiChartSeriesType> { StiChartSeriesType.Sunburst };
			hash["Waterfall"] = new List<StiChartSeriesType> { StiChartSeriesType.Waterfall };
			hash["BoxAndWhisker"] = new List<StiChartSeriesType> { StiChartSeriesType.BoxAndWhisker };
			isInit = true;
		}
	}
}
