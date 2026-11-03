using System.Drawing.Printing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard.Export;

public interface IStiPdfDashboardExportSettings : IStiDashboardExportSettings
{
	bool AutoPrint { get; set; }

	int ImageQuality { get; set; }

	PaperKind PaperSize { get; set; }

	StiPageOrientation Orientation { get; set; }
}
