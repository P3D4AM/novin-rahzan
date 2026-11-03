namespace Stimulsoft.Report.Dashboard;

public interface IStiRefreshViewerElement
{
	bool AllowRefreshControl { get; set; }

	void InvokeRefreshControl(string propertyName);
}
