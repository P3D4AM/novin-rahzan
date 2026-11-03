using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Dashboard.Export;

public interface IStiDataDashboardExportSettings : IStiDashboardExportSettings
{
	StiDataType DataType { get; set; }
}
