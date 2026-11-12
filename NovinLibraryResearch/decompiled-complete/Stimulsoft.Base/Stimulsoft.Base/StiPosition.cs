namespace Stimulsoft.Base;

public struct StiPosition
{
	public int Line { get; set; }

	public int Column { get; set; }

	public StiPosition(int line, int column)
	{
		Line = line;
		Column = column;
	}
}
