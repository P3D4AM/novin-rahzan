namespace Stimulsoft.Data.Exceptions;

internal class StiArgumentCountException : StiDataException
{
	public string FunctionName { get; }

	public StiArgumentCountException(string functionName)
		: base("Parameter count mismatch in the function '" + functionName + "'!")
	{
		FunctionName = functionName;
	}
}
