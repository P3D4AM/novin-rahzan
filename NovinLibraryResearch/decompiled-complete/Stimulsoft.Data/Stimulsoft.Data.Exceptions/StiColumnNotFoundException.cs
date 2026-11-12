namespace Stimulsoft.Data.Exceptions;

internal class StiColumnNotFoundException : StiDataException
{
	public string Name { get; }

	public StiColumnNotFoundException(string name)
		: base("Column '" + name + "' not found!")
	{
		Name = name;
	}
}
