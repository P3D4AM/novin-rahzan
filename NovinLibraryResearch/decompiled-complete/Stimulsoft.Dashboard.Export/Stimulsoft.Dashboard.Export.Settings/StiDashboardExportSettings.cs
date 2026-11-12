using Stimulsoft.Dashboard.Export.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard.Export;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Dashboard.Export.Settings;

public abstract class StiDashboardExportSettings : StiExportSettings, IStiDashboardExportSettings
{
	public abstract StiDashboardExportFormat Format { get; }

	public bool RenderBorders { get; set; } = true;


	public bool RenderSingleElement { get; set; }

	public bool RenderSinglePage { get; set; }

	public bool OpenAfterExport { get; set; } = true;


	public bool ApplyDefaultFilters { get; set; }

	internal bool IsDesignMode
	{
		get
		{
			if (this is StiSvgDashboardExportSettings)
			{
				return ((StiSvgDashboardExportSettings)this).DesignMode;
			}
			return false;
		}
	}

	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormatHelper.Convert(Format);
	}
}
