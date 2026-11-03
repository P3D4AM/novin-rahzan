namespace Stimulsoft.Report.Dashboard;

public interface IStiTableElementAutoSizer
{
	void AutoMeasureToTable(IStiTableElement tableElement);

	void AutoMeasureToColumn(IStiTableElement tableElement);
}
