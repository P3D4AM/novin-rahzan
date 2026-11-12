using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design;

public class OperationReportHandler : MarshalByRefObject, IOperationReportHandler
{
	private readonly Action<string> _errorHandler;

	private readonly Action<string> _warningHandler;

	private readonly Action<string> _informationHandler;

	private readonly Action<string> _verboseHandler;

	public virtual int Version => 0;

	public OperationReportHandler([JetBrains.Annotations.CanBeNull] Action<string> errorHandler = null, [JetBrains.Annotations.CanBeNull] Action<string> warningHandler = null, [JetBrains.Annotations.CanBeNull] Action<string> informationHandler = null, [JetBrains.Annotations.CanBeNull] Action<string> verboseHandler = null)
	{
		_errorHandler = errorHandler;
		_warningHandler = warningHandler;
		_informationHandler = informationHandler;
		_verboseHandler = verboseHandler;
	}

	public virtual void OnError(string message)
	{
		_errorHandler?.Invoke(message);
	}

	public virtual void OnWarning(string message)
	{
		_warningHandler?.Invoke(message);
	}

	public virtual void OnInformation(string message)
	{
		_informationHandler?.Invoke(message);
	}

	public virtual void OnVerbose(string message)
	{
		_verboseHandler?.Invoke(message);
	}
}
