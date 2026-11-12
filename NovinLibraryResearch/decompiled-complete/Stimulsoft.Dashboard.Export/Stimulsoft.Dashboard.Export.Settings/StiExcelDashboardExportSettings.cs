using Stimulsoft.Report.Dashboard.Export;

namespace Stimulsoft.Dashboard.Export.Settings;

public class StiExcelDashboardExportSettings : StiDashboardExportSettings, IStiExcelDashboardExportSettings, IStiDashboardExportSettings
{
	public override StiDashboardExportFormat Format => StiDashboardExportFormat.Excel;

	public int ImageQuality { get; set; } = 200;


	public bool ExportDataOnly { get; set; }
}
