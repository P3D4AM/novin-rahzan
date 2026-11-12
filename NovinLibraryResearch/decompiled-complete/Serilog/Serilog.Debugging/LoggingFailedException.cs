using System;

namespace Serilog.Debugging;

public class LoggingFailedException : Exception
{
	public LoggingFailedException(string message)
		: base(message)
	{
	}
}
