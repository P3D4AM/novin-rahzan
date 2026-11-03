namespace Stimulsoft.Report.Dashboard.Export;

public interface IStiDashboardExportSettings
{
	bool RenderBorders { get; set; }

	bool RenderSingleElement { get; set; }

	bool RenderSinglePage { get; set; }

	bool OpenAfterExport { get; set; }

	bool ApplyDefaultFilters { get; set; }
}
