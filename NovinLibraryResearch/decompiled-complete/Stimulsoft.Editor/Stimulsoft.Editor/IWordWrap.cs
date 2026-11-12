namespace Stimulsoft.Editor;

public interface IWordWrap
{
	bool WordWrap { get; set; }

	bool WrapAtMargin { get; set; }

	int WrapMargin { get; }

	bool UpdateWordWrap();

	bool UpdateWordWrap(int first, int last);

	int GetWrapMargin();

	void ResetWordWrap();

	void ResetWrapAtMargin();
}
