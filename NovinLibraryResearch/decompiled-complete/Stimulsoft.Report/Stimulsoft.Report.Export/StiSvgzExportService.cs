using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Svg.png")]
public class StiSvgzExportService : StiSvgExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeSvgzFile");

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "SvgzFiles");
	}
}
