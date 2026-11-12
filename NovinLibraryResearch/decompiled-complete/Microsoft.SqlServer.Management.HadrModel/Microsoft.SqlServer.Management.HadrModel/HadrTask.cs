using System;
using System.Globalization;
using System.Threading;

namespace Microsoft.SqlServer.Management.HadrModel;

public abstract class HadrTask
{
	protected IExecutionPolicy policy;

	private CancellationToken token;

	public string Name { get; set; }

	public bool IsCancelled
	{
		get
		{
			if (token == CancellationToken.None)
			{
				return false;
			}
			return token.IsCancellationRequested;
		}
	}

	public event TaskUpdateEventHandler TaskProgressEventHandler;

	public HadrTask(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		token = CancellationToken.None;
		Name = name;
		this.TaskProgressEventHandler = null;
	}

	protected void TaskWait(int waitTimeInMs)
	{
		int num = 5000;
		int num2 = waitTimeInMs / num;
		while (num2-- > 0)
		{
			if (IsCancelled)
			{
				throw new Exception(Resource.TaskEventArgsTaskExecutionCancelled);
			}
			Thread.Sleep(num);
		}
	}

	public void Perform(IExecutionPolicy policy, CancellationToken token = default(CancellationToken), ScenarioTaskHandler taskDelegate = null)
	{
		Exception ex = null;
		this.token = token;
		if (policy == null)
		{
			throw new ArgumentNullException("policy");
		}
		if (taskDelegate != null)
		{
			taskDelegate(policy);
			return;
		}
		UpdateStatus(new TaskEventArgs(Name, Resource.TaskEventArgsTaskStarted));
		while (policy.ResumeExecution())
		{
			try
			{
				if (IsCancelled)
				{
					UpdateStatus(new TaskEventArgs(Name, Resource.TaskEventArgsTaskExecutionCancelled));
					break;
				}
				Perform(policy);
			}
			catch (Exception ex2)
			{
				if (policy.Expired)
				{
					UpdateStatus(new TaskEventArgs(Name, string.Format(CultureInfo.InvariantCulture, Resource.TaskEventArgsTaskExecutionFailed, ex2.Message)));
					ex = ex2;
					break;
				}
				UpdateStatus(new TaskEventArgs(Name, string.Format(CultureInfo.InvariantCulture, Resource.TaskEventArgsTaskExecutionFailedWithRetry, ex2.Message)));
				Thread.Sleep(policy.BackoffInterval());
			}
		}
		if (ex != null)
		{
			throw ex;
		}
		UpdateStatus(new TaskEventArgs(Name, Resource.TaskEventArgsTaskComplete));
	}

	public void Rollback(IExecutionPolicy policy, ScenarioTaskHandler rollbackDelegate = null)
	{
		Exception ex = null;
		if (policy == null)
		{
			throw new ArgumentNullException("policy");
		}
		if (rollbackDelegate != null)
		{
			rollbackDelegate(policy);
			return;
		}
		UpdateStatus(new TaskEventArgs(Name, Resource.TaskEventArgsTaskStarted));
		while (policy.ResumeExecution())
		{
			try
			{
				Rollback(policy);
			}
			catch (Exception ex2)
			{
				if (policy.Expired)
				{
					UpdateStatus(new TaskEventArgs(Name, string.Format(CultureInfo.InvariantCulture, Resource.TaskEventArgsTaskExecutionFailed, ex2.Message)));
					ex = ex2;
					break;
				}
				UpdateStatus(new TaskEventArgs(Name, string.Format(CultureInfo.InvariantCulture, Resource.TaskEventArgsTaskExecutionFailedWithRetry, ex2.Message)));
				Thread.Sleep(policy.BackoffInterval());
			}
		}
		if (ex != null)
		{
			throw ex;
		}
		UpdateStatus(new TaskEventArgs(Name, Resource.TaskEventArgsTaskComplete));
	}

	public void TaskUpdateEventHandler(object sender, TaskEventArgs e)
	{
		UpdateStatus(e);
	}

	protected abstract void Perform(IExecutionPolicy policy);

	protected abstract void Rollback(IExecutionPolicy policy);

	protected void UpdateStatus(TaskEventArgs e)
	{
		if (this.TaskProgressEventHandler != null)
		{
			this.TaskProgressEventHandler(this, e);
		}
	}
}
