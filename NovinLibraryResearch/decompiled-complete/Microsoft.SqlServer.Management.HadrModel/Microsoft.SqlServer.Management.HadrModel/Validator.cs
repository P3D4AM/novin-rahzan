using System;
using System.Threading;

namespace Microsoft.SqlServer.Management.HadrModel;

public abstract class Validator
{
	public string Name { get; private set; }

	public event ValidatorUpdateEventHandler ValidatorProgressEventHandler;

	public Validator(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		Name = name;
		this.ValidatorProgressEventHandler = null;
	}

	public void Validate(IExecutionPolicy policy, ScenarioValidatorHandler validationDelegate)
	{
		Exception ex = null;
		if (validationDelegate != null)
		{
			validationDelegate(policy);
			return;
		}
		UpdateStatus(new ValidatorEventArgs(Name, Resource.ValidatorEventArgsValidationStarted));
		while (policy.ResumeExecution())
		{
			try
			{
				Validate(policy);
			}
			catch (Exception ex2)
			{
				if (policy.Expired)
				{
					UpdateStatus(new ValidatorEventArgs(Name, string.Format(Resource.ValidatorEventArgsValidationFailed, ex2.Message)));
					ex = ex2;
					break;
				}
				UpdateStatus(new ValidatorEventArgs(Name, string.Format(Resource.ValidatorEventArgsValidationFailedWithRetry, ex2.Message)));
				Thread.Sleep(policy.BackoffInterval());
			}
		}
		if (ex != null)
		{
			throw ex;
		}
		UpdateStatus(new ValidatorEventArgs(Name, Resource.ValidatorEventArgsValidationComplete));
	}

	protected abstract void Validate(IExecutionPolicy policy);

	protected void UpdateStatus(ValidatorEventArgs e)
	{
		if (this.ValidatorProgressEventHandler != null)
		{
			this.ValidatorProgressEventHandler(this, e);
		}
	}
}
