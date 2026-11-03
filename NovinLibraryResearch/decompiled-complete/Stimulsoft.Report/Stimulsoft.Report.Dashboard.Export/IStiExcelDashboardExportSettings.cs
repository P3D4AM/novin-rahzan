namespace Stimulsoft.Report.Dashboard.Export;

public interface IStiExcelDashboardExportSettings : IStiDashboardExportSettings
{
	int ImageQuality { get; set; }

	bool ExportDataOnly { get; set; }
}
