using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DiagnosticsLogger<TLoggerCategory> : IDiagnosticsLogger<TLoggerCategory>, IDiagnosticsLogger where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
{
	public virtual ILoggingOptions Options { get; }

	public virtual ILogger Logger { get; }

	public virtual IInterceptors Interceptors { get; }

	public virtual DiagnosticSource DiagnosticSource { get; }

	public virtual LoggingDefinitions Definitions { get; }

	public DiagnosticsLogger([JetBrains.Annotations.NotNull] ILoggerFactory loggerFactory, [JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, [JetBrains.Annotations.NotNull] DiagnosticSource diagnosticSource, [JetBrains.Annotations.NotNull] LoggingDefinitions loggingDefinitions, [JetBrains.Annotations.CanBeNull] IInterceptors interceptors = null)
	{
		DiagnosticSource = diagnosticSource;
		Definitions = loggingDefinitions;
		Logger = loggerFactory.CreateLogger(new TLoggerCategory());
		Options = loggingOptions;
		Interceptors = interceptors;
	}

	public virtual bool ShouldLogSensitiveData()
	{
		ILoggingOptions options = Options;
		if (options == null)
		{
			return false;
		}
		if (options.IsSensitiveDataLoggingEnabled && !options.IsSensitiveDataLoggingWarned)
		{
			this.SensitiveDataLoggingEnabledWarning();
			options.IsSensitiveDataLoggingWarned = true;
		}
		return options.IsSensitiveDataLoggingEnabled;
	}
}
