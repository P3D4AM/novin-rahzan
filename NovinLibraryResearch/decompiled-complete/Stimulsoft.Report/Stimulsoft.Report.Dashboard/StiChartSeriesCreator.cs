using System;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Report.Dashboard;

internal static class StiChartSeriesCreator
{
	public static StiSeries New(string typeName)
	{
		if (string.IsNullOrEmpty(typeName))
		{
			return null;
		}
		if (!Enum.TryParse<StiChartSeriesType>(typeName, ignoreCase: true, out var result))
		{
			return null;
		}
		return New(result);
	}

	public static StiSeries New(StiChartSeriesType type)
	{
		return type switch
		{
			StiChartSeriesType.ClusteredColumn => new StiClusteredColumnSeries(), 
			StiChartSeriesType.StackedColumn => new StiStackedColumnSeries(), 
			StiChartSeriesType.FullStackedColumn => new StiFullStackedColumnSeries(), 
			StiChartSeriesType.Pareto => new StiParetoSeries(), 
			StiChartSeriesType.Histogram => new StiHistogramSeries(), 
			StiChartSeriesType.Line => new StiLineSeries(), 
			StiChartSeriesType.StackedLine => new StiStackedLineSeries(), 
			StiChartSeriesType.FullStackedLine => new StiFullStackedLineSeries(), 
			StiChartSeriesType.Spline => new StiSplineSeries(), 
			StiChartSeriesType.StackedSpline => new StiStackedSplineSeries(), 
			StiChartSeriesType.FullStackedSpline => new StiFullStackedSplineSeries(), 
			StiChartSeriesType.SteppedLine => new StiSteppedLineSeries(), 
			StiChartSeriesType.Area => new StiAreaSeries(), 
			StiChartSeriesType.StackedArea => new StiStackedAreaSeries(), 
			StiChartSeriesType.FullStackedArea => new StiFullStackedAreaSeries(), 
			StiChartSeriesType.SplineArea => new StiSplineAreaSeries(), 
			StiChartSeriesType.StackedSplineArea => new StiStackedSplineAreaSeries(), 
			StiChartSeriesType.FullStackedSplineArea => new StiFullStackedSplineAreaSeries(), 
			StiChartSeriesType.SteppedArea => new StiSteppedAreaSeries(), 
			StiChartSeriesType.Range => new StiRangeSeries(), 
			StiChartSeriesType.SplineRange => new StiSplineRangeSeries(), 
			StiChartSeriesType.SteppedRange => new StiSteppedRangeSeries(), 
			StiChartSeriesType.RangeBar => new StiRangeBarSeries(), 
			StiChartSeriesType.ClusteredBar => new StiClusteredBarSeries(), 
			StiChartSeriesType.StackedBar => new StiStackedBarSeries(), 
			StiChartSeriesType.FullStackedBar => new StiFullStackedBarSeries(), 
			StiChartSeriesType.Scatter => new StiScatterSeries(), 
			StiChartSeriesType.ScatterLine => new StiScatterLineSeries(), 
			StiChartSeriesType.ScatterSpline => new StiScatterSplineSeries(), 
			StiChartSeriesType.Pie => new StiPieSeries(), 
			StiChartSeriesType.Pie3d => new StiPie3dSeries(), 
			StiChartSeriesType.RadarPoint => new StiRadarPointSeries(), 
			StiChartSeriesType.RadarLine => new StiRadarLineSeries(), 
			StiChartSeriesType.RadarArea => new StiRadarAreaSeries(), 
			StiChartSeriesType.Funnel => new StiFunnelSeries(), 
			StiChartSeriesType.FunnelWeightedSlices => new StiFunnelWeightedSlicesSeries(), 
			StiChartSeriesType.Candlestick => new StiCandlestickSeries(), 
			StiChartSeriesType.Stock => new StiStockSeries(), 
			StiChartSeriesType.Treemap => new StiTreemapSeries(), 
			StiChartSeriesType.Gantt => new StiGanttSeries(), 
			StiChartSeriesType.Doughnut => new StiDoughnutSeries(), 
			StiChartSeriesType.Bubble => new StiBubbleSeries(), 
			StiChartSeriesType.Pictorial => new StiPictorialSeries(), 
			StiChartSeriesType.Sunburst => new StiSunburstSeries(), 
			StiChartSeriesType.Waterfall => new StiWaterfallSeries(), 
			StiChartSeriesType.BoxAndWhisker => new StiBoxAndWhiskerSeries(), 
			_ => throw new NotSupportedException(), 
		};
	}
}
