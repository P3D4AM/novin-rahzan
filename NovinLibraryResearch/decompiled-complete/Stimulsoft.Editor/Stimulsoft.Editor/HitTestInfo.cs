namespace Stimulsoft.Editor;

public struct HitTestInfo
{
	public HitTest HitTest;

	public int Line;

	public int Pos;

	public IStrItem Item;

	public string String;

	public string Word;

	public string Url;

	public int GutterImage;

	public IOutlineRange OutlineRange;

	public int Page;

	public int Style;

	public ColorFlags ColorFlags;

	public void InitHitTestInfo()
	{
		HitTest = HitTest.None;
		Line = -1;
		Pos = -1;
		Item = null;
		String = null;
		Word = null;
		Url = null;
		GutterImage = -1;
		OutlineRange = null;
		Page = -1;
		Style = -1;
		ColorFlags = ColorFlags.None;
	}
}
