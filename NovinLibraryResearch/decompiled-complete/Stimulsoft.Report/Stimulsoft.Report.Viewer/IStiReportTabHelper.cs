using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Viewer;

public interface IStiReportTabHelper
{
	void SelectLastReportTab();

	void AddMainReportTabIfNeed();

	void AddReportToTabs(StiPage page, StiReport report, StiDrillDownMode mode, bool checkMainReportTab = true);

	void EnableReportTabs();
}
