namespace Stimulsoft.Base.TaskScheduler;

public interface IStiSchedulerItemsControl
{
	event StiSchedulerSaveReportEventHandler AutoSaveReport;

	void Update(object report, string reportFileName);
}
