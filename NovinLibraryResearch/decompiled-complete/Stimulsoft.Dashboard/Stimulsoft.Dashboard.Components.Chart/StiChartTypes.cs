using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

public static class StiChartTypes
{
	public static bool IsAxisArea(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.ClusteredBar && type != StiChartSeriesType.ClusteredColumn && type != StiChartSeriesType.Pareto && type != StiChartSeriesType.Histogram && type != StiChartSeriesType.FullStackedArea && type != StiChartSeriesType.FullStackedBar && type != StiChartSeriesType.FullStackedColumn && type != StiChartSeriesType.FullStackedLine && type != StiChartSeriesType.FullStackedSpline && type != StiChartSeriesType.FullStackedSplineArea && type != StiChartSeriesType.StackedBar && type != StiChartSeriesType.StackedColumn && type != StiChartSeriesType.StackedLine && type != StiChartSeriesType.StackedSpline && type != StiChartSeriesType.StackedArea && type != StiChartSeriesType.StackedSplineArea && type != StiChartSeriesType.Scatter && type != StiChartSeriesType.ScatterLine && type != StiChartSeriesType.ScatterSpline && type != StiChartSeriesType.Candlestick && type != StiChartSeriesType.Stock && type != StiChartSeriesType.Gantt && type != StiChartSeriesType.Bubble && type != StiChartSeriesType.Area && type != StiChartSeriesType.Line && type != StiChartSeriesType.Spline && type != StiChartSeriesType.SplineArea && type != StiChartSeriesType.SplineRange && type != StiChartSeriesType.Range && type != StiChartSeriesType.RangeBar && type != StiChartSeriesType.SteppedArea && type != StiChartSeriesType.SteppedLine && type != StiChartSeriesType.SteppedRange && type != StiChartSeriesType.Waterfall)
		{
			return type == StiChartSeriesType.BoxAndWhisker;
		}
		return true;
	}

	public static bool IsStacked(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.StackedBar && type != StiChartSeriesType.StackedColumn && type != StiChartSeriesType.StackedLine && type != StiChartSeriesType.StackedSpline && type != StiChartSeriesType.StackedArea)
		{
			return type == StiChartSeriesType.StackedSplineArea;
		}
		return true;
	}

	public static bool IsLine(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.FullStackedArea && type != StiChartSeriesType.FullStackedLine && type != StiChartSeriesType.FullStackedSpline && type != StiChartSeriesType.FullStackedSplineArea && type != StiChartSeriesType.StackedLine && type != StiChartSeriesType.StackedSpline && type != StiChartSeriesType.StackedArea && type != StiChartSeriesType.StackedSplineArea && type != StiChartSeriesType.Scatter && type != StiChartSeriesType.ScatterLine && type != StiChartSeriesType.ScatterSpline && type != StiChartSeriesType.Area && type != StiChartSeriesType.Line && type != StiChartSeriesType.Spline && type != StiChartSeriesType.SplineArea && type != StiChartSeriesType.SplineRange && type != StiChartSeriesType.Range && type != StiChartSeriesType.SteppedArea && type != StiChartSeriesType.SteppedLine)
		{
			return type == StiChartSeriesType.SteppedRange;
		}
		return true;
	}

	public static bool IsScatter(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.Scatter && type != StiChartSeriesType.ScatterLine && type != StiChartSeriesType.ScatterSpline)
		{
			return type == StiChartSeriesType.Bubble;
		}
		return true;
	}

	public static bool IsBar(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.ClusteredBar && type != StiChartSeriesType.FullStackedBar && type != StiChartSeriesType.StackedBar)
		{
			return type == StiChartSeriesType.Gantt;
		}
		return true;
	}

	public static bool IsRange(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.Range && type != StiChartSeriesType.RangeBar && type != StiChartSeriesType.SteppedRange && type != StiChartSeriesType.SplineRange)
		{
			return type == StiChartSeriesType.Gantt;
		}
		return true;
	}

	public static bool IsFinancial(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.Candlestick)
		{
			return type == StiChartSeriesType.Stock;
		}
		return true;
	}

	public static bool IsPie(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Pie;
	}

	public static bool IsPie3d(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Pie3d;
	}

	public static bool IsRadar(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.RadarPoint && type != StiChartSeriesType.RadarLine)
		{
			return type == StiChartSeriesType.RadarArea;
		}
		return true;
	}

	public static bool IsDoughnut(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Doughnut;
	}

	public static bool IsFunnel(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.Funnel)
		{
			return type == StiChartSeriesType.FunnelWeightedSlices;
		}
		return true;
	}

	public static bool IsTreemap(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Treemap;
	}

	public static bool IsPareto(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Pareto;
	}

	public static bool IsSunburst(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Sunburst;
	}

	public static bool IsWaterfall(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Waterfall;
	}

	public static bool IsFullStacked(StiChartSeriesType type)
	{
		if (type != StiChartSeriesType.FullStackedArea && type != StiChartSeriesType.FullStackedBar && type != StiChartSeriesType.FullStackedColumn && type != StiChartSeriesType.FullStackedLine && type != StiChartSeriesType.FullStackedSpline)
		{
			return type == StiChartSeriesType.FullStackedSplineArea;
		}
		return true;
	}

	public static bool IsPictorial(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.Pictorial;
	}

	public static bool IsBoxAndWhisker(StiChartSeriesType type)
	{
		return type == StiChartSeriesType.BoxAndWhisker;
	}
}
