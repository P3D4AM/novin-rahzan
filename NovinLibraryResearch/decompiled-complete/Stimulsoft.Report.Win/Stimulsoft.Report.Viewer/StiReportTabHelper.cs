using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Viewer;

public class StiReportTabHelper : IStiReportTabHelper
{
	private StiViewerControl viewer;

	public void SelectLastReportTab()
	{
		viewer.SelectLastReportTab();
	}

	public void AddMainReportTabIfNeed()
	{
		viewer.AddMainReportTabIfNeed();
	}

	public void AddReportToTabs(StiPage page, StiReport report, StiDrillDownMode mode, bool checkMainReportTab = true)
	{
		viewer.AddReportToTabs(report, mode, checkMainReportTab);
	}

	public void EnableReportTabs()
	{
		viewer.EnableReportTabs();
	}

	internal StiReportTabHelper(StiViewerControl viewer)
	{
		this.viewer = viewer;
	}
}
