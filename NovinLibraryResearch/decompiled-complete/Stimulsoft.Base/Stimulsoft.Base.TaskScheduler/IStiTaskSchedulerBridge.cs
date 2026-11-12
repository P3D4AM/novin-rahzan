using System;

namespace Stimulsoft.Base.TaskScheduler;

public interface IStiTaskSchedulerBridge
{
	void Run(IntPtr handler, string reportGuid, string reportPath);

	IStiSchedulerItemsControl GetSchedulerItemsControl(IntPtr intPtr);
}
