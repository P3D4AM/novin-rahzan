namespace Stimulsoft.Editor;

public class DrawUserMarginEventArgs : DrawHeaderEventArgs
{
	public int Line;

	public DrawUserMarginEventArgs(string tag, int line)
		: base(tag)
	{
		Line = line;
	}
}
