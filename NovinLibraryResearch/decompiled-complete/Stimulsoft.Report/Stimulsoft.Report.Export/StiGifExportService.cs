using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Gif.png")]
public class StiGifExportService : StiImageExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeGifFile");

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "GifFiles");
	}
}
