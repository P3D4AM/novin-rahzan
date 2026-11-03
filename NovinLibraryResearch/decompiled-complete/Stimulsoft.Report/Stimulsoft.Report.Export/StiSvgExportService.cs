using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Svg.png")]
public class StiSvgExportService : StiImageExportService
{
	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeSvgFile");

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "SvgFiles");
	}
}
