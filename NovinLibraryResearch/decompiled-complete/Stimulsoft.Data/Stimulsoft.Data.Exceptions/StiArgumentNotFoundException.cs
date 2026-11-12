namespace Stimulsoft.Data.Exceptions;

internal class StiArgumentNotFoundException : StiDataException
{
	public string FunctionName { get; }

	public string ArgumentName { get; }

	public StiArgumentNotFoundException(string functionName, string argumentName)
		: base("Argument '" + argumentName + "' in the function '" + functionName + "' not found!")
	{
		FunctionName = functionName;
		ArgumentName = argumentName;
	}
}
