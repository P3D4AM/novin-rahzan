using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class OperationReporter : IOperationReporter
{
	private readonly IOperationReportHandler _handler;

	public OperationReporter([JetBrains.Annotations.CanBeNull] IOperationReportHandler handler)
	{
		_handler = handler;
	}

	public virtual void WriteError(string message)
	{
		_handler?.OnError(message);
	}

	public virtual void WriteWarning(string message)
	{
		_handler?.OnWarning(message);
	}

	public virtual void WriteInformation(string message)
	{
		_handler?.OnInformation(message);
	}

	public virtual void WriteVerbose(string message)
	{
		_handler?.OnVerbose(message);
	}
}
