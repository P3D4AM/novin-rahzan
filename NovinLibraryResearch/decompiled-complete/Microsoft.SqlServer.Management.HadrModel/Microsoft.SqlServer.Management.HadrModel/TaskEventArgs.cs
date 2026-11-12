using System;

namespace Microsoft.SqlServer.Management.HadrModel;

public sealed class TaskEventArgs : EventArgs
{
	public string Name { get; set; }

	public TaskEventStatus Status { get; set; }

	public string Details { get; set; }

	public TaskEventArgs(string taskName, string taskDetails)
	{
		Name = taskName;
		Details = taskDetails;
	}

	public TaskEventArgs(string taskName, string taskDetails, TaskEventStatus taskStatus)
	{
		Name = taskName;
		Status = taskStatus;
		Details = taskDetails;
	}
}
