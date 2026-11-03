namespace EntityCache.Uploader.Events;

public class LogEventArgs
{
	public string Message { get; set; }

	public LogEventArgs()
	{
	}

	public LogEventArgs(string message)
	{
		Message = message;
	}
}
