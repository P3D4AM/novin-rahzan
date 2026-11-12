using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Dashboard.Viewer;

internal class StiDashboardReportTabHelper : IStiReportTabHelper
{
	private StiDashboardViewerControl viewer;

	public void SelectLastReportTab()
	{
		viewer.SelectLastPage();
	}

	public void AddMainReportTabIfNeed()
	{
	}

	public void AddReportToTabs(StiPage page, StiReport report, StiDrillDownMode mode, bool checkMainReportTab = true)
	{
		viewer.AddReportToTabs(page, report);
	}

	public void EnableReportTabs()
	{
	}

	internal StiDashboardReportTabHelper(StiDashboardViewerControl viewer)
	{
		this.viewer = viewer;
	}
}
