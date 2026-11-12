using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface IDiagnosticsLogger
{
	ILoggingOptions Options { get; }

	LoggingDefinitions Definitions { get; }

	ILogger Logger { get; }

	DiagnosticSource DiagnosticSource { get; }

	bool ShouldLogSensitiveData();
}
public interface IDiagnosticsLogger<TLoggerCategory> : IDiagnosticsLogger where TLoggerCategory : LoggerCategory<TLoggerCategory>, new()
{
	IInterceptors Interceptors { get; }
}
