using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements;

public interface IStiRefreshControl
{
	IStiElement Element { get; set; }

	void RefreshControl(object sender, StiReportControlUpdateEventArgs e);
}
