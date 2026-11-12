namespace Stimulsoft.Data.Exceptions;

internal class StiFunctionNotFoundException : StiDataException
{
	public string Name { get; }

	public StiFunctionNotFoundException(string name)
		: base("Function '" + name + "' not found!")
	{
		Name = name;
	}
}
