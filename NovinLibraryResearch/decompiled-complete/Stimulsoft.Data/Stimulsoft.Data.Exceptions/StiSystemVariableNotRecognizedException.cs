namespace Stimulsoft.Data.Exceptions;

public class StiSystemVariableNotRecognizedException : StiDataException
{
	public StiSystemVariableNotRecognizedException(string name)
		: base("System Variable '" + name + "' Is Not Recognized!")
	{
	}
}
