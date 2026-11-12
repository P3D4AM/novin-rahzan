namespace Stimulsoft.Data.Exceptions;

public class StiTypeNotRecognizedException : StiDataException
{
	public StiTypeNotRecognizedException(object type)
		: base($"'{type}' Is Not Recognized!")
	{
	}
}
