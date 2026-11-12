namespace Serilog.Parsing;

public readonly struct Alignment
{
	public AlignmentDirection Direction { get; }

	public int Width { get; }

	public Alignment(AlignmentDirection direction, int width)
	{
		Direction = direction;
		Width = width;
	}
}
