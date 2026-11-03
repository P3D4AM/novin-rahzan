using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Pcx.png")]
public class StiPcxExportService : StiImageExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypePcxFile");

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "PcxFiles");
	}
}
