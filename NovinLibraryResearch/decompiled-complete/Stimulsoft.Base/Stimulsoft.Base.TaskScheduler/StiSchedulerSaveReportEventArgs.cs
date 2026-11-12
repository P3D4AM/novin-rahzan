using System;

namespace Stimulsoft.Base.TaskScheduler;

public sealed class StiSchedulerSaveReportEventArgs : EventArgs
{
	public string Path { get; set; }
}
