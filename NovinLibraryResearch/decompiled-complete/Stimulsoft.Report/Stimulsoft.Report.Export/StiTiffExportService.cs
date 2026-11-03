using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Tiff.png")]
public class StiTiffExportService : StiImageExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeTiffFile");

	public StiTiffExportService()
	{
		MultipleFiles = false;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "TiffFiles");
	}
}
