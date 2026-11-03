using System.Threading.Tasks;
using System.Timers;

namespace Stimulsoft.Report.Helpers;

public static class TaskExt
{
	public static Task Delay(double milliseconds)
	{
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		Timer timer = new Timer();
		timer.Elapsed += delegate
		{
			tcs.TrySetResult(result: true);
		};
		timer.Interval = milliseconds;
		timer.AutoReset = false;
		timer.Start();
		return tcs.Task;
	}
}
