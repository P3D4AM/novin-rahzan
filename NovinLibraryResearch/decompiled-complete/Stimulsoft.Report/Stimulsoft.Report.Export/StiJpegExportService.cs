using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Jpeg.png")]
public class StiJpegExportService : StiImageExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeJpegFile");

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "JpegFiles");
	}
}
