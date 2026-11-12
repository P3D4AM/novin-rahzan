using System.Drawing.Printing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard.Export;

namespace Stimulsoft.Dashboard.Export.Settings;

public class StiPdfDashboardExportSettings : StiDashboardExportSettings, IStiPdfDashboardExportSettings, IStiDashboardExportSettings
{
	public override StiDashboardExportFormat Format => StiDashboardExportFormat.Pdf;

	public bool AutoPrint { get; set; }

	public int ImageQuality { get; set; } = 200;


	public virtual PaperKind PaperSize { get; set; } = (PaperKind)9;


	public virtual StiPageOrientation Orientation { get; set; } = StiPageOrientation.Landscape;

}
