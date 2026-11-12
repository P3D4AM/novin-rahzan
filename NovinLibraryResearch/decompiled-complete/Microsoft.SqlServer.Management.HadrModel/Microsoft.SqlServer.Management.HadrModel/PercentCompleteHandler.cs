using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public class PercentCompleteHandler
{
	private string taskName;

	public const int PercentCompleteNotification = 5;

	protected int percentComplete;

	public event TaskUpdateEventHandler TaskProgressEventHandler;

	public PercentCompleteHandler(string taskName)
	{
		this.taskName = taskName;
	}

	public void UpdateStatus(TaskEventArgs e)
	{
		if (this.TaskProgressEventHandler != null)
		{
			this.TaskProgressEventHandler(this, e);
		}
	}

	public void percentCompleteHandler(object sender, PercentCompleteEventArgs e)
	{
		percentComplete = e.Percent;
		UpdateStatus(new TaskEventArgs(taskName, "Perform", TaskEventStatus.Running));
	}
}
