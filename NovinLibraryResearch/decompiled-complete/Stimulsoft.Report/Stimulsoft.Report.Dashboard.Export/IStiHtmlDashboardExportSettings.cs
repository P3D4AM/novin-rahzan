namespace Stimulsoft.Report.Dashboard.Export;

public interface IStiHtmlDashboardExportSettings : IStiDashboardExportSettings
{
	int ImageQuality { get; set; }

	int Scale { get; set; }

	bool EnableAnimation { get; set; }
}
