using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Dashboard.Export;

public interface IStiImageDashboardExportSettings : IStiDashboardExportSettings
{
	StiImageType ImageType { get; set; }

	int Scale { get; set; }

	double Width { get; set; }

	double Height { get; set; }
}
