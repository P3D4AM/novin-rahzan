using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Surface;

namespace Stimulsoft.Report.App;

public interface IStiScreen : IStiSurface, IStiPanelUI, IStiComponentUI, IStiBrush, IStiDashboardElementStyle
{
	StiSurfaceViewMode SurfaceViewMode { get; set; }

	StiLayoutKind LayoutKind { get; set; }
}
