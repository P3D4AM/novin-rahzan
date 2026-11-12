namespace Stimulsoft.Base.TaskScheduler;

public interface IStiTaskSchedulerItem
{
	string ReportGuid { get; set; }

	string Name { get; set; }

	string ID { get; set; }

	bool Enabled { get; set; }

	StiQuickTriggerType TriggerType { get; set; }

	string GetItemTitle();

	void TryDelete();
}
