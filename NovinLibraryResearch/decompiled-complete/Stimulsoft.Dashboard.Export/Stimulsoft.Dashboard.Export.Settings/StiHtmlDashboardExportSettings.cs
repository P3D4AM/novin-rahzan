using Stimulsoft.Report.Dashboard.Export;

namespace Stimulsoft.Dashboard.Export.Settings;

public class StiHtmlDashboardExportSettings : StiDashboardExportSettings, IStiHtmlDashboardExportSettings, IStiDashboardExportSettings
{
	public override StiDashboardExportFormat Format => StiDashboardExportFormat.Html;

	public int ImageQuality { get; set; } = 200;


	public int Scale { get; set; } = 100;


	public bool EnableAnimation { get; set; } = true;

}
