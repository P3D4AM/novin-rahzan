namespace Stimulsoft.Editor;

public interface IStrItem
{
	string String { get; set; }

	short[] ColorData { get; set; }

	StrItemState State { get; set; }

	int LexState { get; set; }

	int PrevLexState { get; set; }

	void SetColorFlag(int start, int len, ColorFlags flag, bool setFlag);

	void GetColorFlag(int pos, out ColorFlags flag);
}
