using Stimulsoft.Report.Dashboard.Export;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Dashboard.Export.Settings;

public class StiDataDashboardExportSettings : StiDashboardExportSettings, IStiDataDashboardExportSettings, IStiDashboardExportSettings
{
	public override StiDashboardExportFormat Format => StiDashboardExportFormat.Data;

	public StiDataType DataType { get; set; } = StiDataType.Csv;


	public StiDataDashboardExportSettings()
	{
	}

	public StiDataDashboardExportSettings(StiDataType dataType)
		: this()
	{
		DataType = dataType;
	}
}
