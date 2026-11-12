using Stimulsoft.Report.Export;

namespace Stimulsoft.Dashboard.Export.Settings;

public class StiSvgDashboardExportSettings : StiImageDashboardExportSettings
{
	public override StiImageType ImageType => StiImageType.Svg;

	public bool RenderEmptyContent { get; set; } = true;


	public bool DesignMode { get; set; }
}
