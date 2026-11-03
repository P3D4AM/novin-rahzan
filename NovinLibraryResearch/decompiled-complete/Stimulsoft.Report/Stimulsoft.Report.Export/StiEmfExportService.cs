using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Emf.png")]
public class StiEmfExportService : StiImageExportService
{
	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "EmfFiles");
	}
}
