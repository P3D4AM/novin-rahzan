namespace Stimulsoft.Dashboard.Viewer.FullScreen;

public abstract class StiFullScreenHelper
{
	public bool IsActivated { get; set; }

	public StiDashboardViewerControl Viewer { get; }

	public StiFullScreenHelper(StiDashboardViewerControl viewer)
	{
		Viewer = viewer;
	}
}
