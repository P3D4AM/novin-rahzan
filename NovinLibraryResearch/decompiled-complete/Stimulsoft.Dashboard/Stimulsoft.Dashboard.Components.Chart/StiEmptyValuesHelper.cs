using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

public static class StiEmptyValuesHelper
{
	public static bool IsShowZeros(StiChartSeriesType seriesType)
	{
		return StiChartSeriesCreator.New(seriesType) is IStiShowZerosSeries;
	}

	public static bool IsShowNulls(StiChartSeriesType seriesType)
	{
		return StiChartSeriesCreator.New(seriesType) is IStiShowNullsSeries;
	}

	public static bool IsShowZerosSimple(StiChartSeriesType seriesType)
	{
		return StiChartSeriesCreator.New(seriesType) is IStiShowZerosSeries;
	}

	public static bool IsSimple(StiChartSeriesType seriesType)
	{
		return !(StiChartSeriesCreator.New(seriesType) is IStiBaseLineSeries);
	}
}
