using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Bmp.png")]
public class StiBmpExportService : StiImageExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeBmpFile");

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "BmpFiles");
	}
}
