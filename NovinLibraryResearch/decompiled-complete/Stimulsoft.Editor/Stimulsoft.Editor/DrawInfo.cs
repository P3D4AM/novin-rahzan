namespace Stimulsoft.Editor;

public struct DrawInfo
{
	public string Text;

	public bool Selection;

	public short Style;

	public ColorFlags Flags;

	public int Char;

	public int Line;

	public int GutterImage;

	public int Page;

	public void Init()
	{
		Text = string.Empty;
		Selection = false;
		Style = -1;
		Flags = ColorFlags.None;
		Char = -1;
		Line = -1;
		Page = -1;
		GutterImage = -1;
	}
}
