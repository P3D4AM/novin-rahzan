using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class OperationLoggerProvider : ILoggerProvider, IDisposable
{
	private readonly IOperationReporter _reporter;

	public OperationLoggerProvider([JetBrains.Annotations.NotNull] IOperationReporter reporter)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reporter, "reporter");
		_reporter = reporter;
	}

	public virtual ILogger CreateLogger(string categoryName)
	{
		return new OperationLogger(categoryName, _reporter);
	}

	public virtual void Dispose()
	{
	}
}
