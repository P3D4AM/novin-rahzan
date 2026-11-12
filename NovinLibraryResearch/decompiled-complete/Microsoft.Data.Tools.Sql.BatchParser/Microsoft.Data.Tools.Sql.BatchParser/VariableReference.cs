namespace Microsoft.Data.Tools.Sql.BatchParser;

internal sealed class VariableReference
{
	public int Length { get; private set; }

	public int Start { get; private set; }

	public string VariableName { get; private set; }

	public string VariableValue { get; internal set; }

	public VariableReference(int start, int length, string variableName)
	{
		Start = start;
		Length = length;
		VariableName = variableName;
		VariableValue = null;
	}
}
