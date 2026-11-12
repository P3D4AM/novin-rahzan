using Stimulsoft.Data.Exceptions;

namespace Stimulsoft.Dashboard.Interactions.Design;

internal static class StiDashboardInteractionCreator
{
	public static StiDashboardInteraction New(string identName)
	{
		return identName switch
		{
			"Chart" => new StiChartDashboardInteraction(), 
			"Gauge" => new StiGaugeDashboardInteraction(), 
			"Image" => new StiImageDashboardInteraction(), 
			"Indicator" => new StiIndicatorDashboardInteraction(), 
			"Table" => new StiTableDashboardInteraction(), 
			"TableColumn" => new StiTableColumnDashboardInteraction(), 
			"Text" => new StiTextDashboardInteraction(), 
			"OnlineMap" => new StiOnlineMapDashboardInteraction(), 
			"PivotTable" => new StiPivotTableDashboardInteraction(), 
			"Progress" => new StiProgressDashboardInteraction(), 
			"RegionMap" => new StiRegionMapDashboardInteraction(), 
			_ => throw new StiTypeNotRecognizedException(identName), 
		};
	}
}
