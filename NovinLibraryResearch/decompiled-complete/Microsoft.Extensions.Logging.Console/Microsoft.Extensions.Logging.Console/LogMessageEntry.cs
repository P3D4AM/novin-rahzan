namespace Microsoft.Extensions.Logging.Console;

internal readonly struct LogMessageEntry
{
	public readonly string Message;

	public readonly bool LogAsError;

	public LogMessageEntry(string message, bool logAsError = false)
	{
		Message = message;
		LogAsError = logAsError;
	}
}
