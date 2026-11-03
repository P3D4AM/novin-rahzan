using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Png.png")]
public class StiPngExportService : StiImageExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypePngFile");

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "PngFiles");
	}
}
