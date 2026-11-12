namespace Stimulsoft.Data.Exceptions;

public class StiBingException : StiDataException
{
	public StiBingException(string message)
		: base("BING: '" + message + "'")
	{
	}
}
