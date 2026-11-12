using System.Drawing;

namespace Stimulsoft.Editor;

public interface IBraceMatching
{
	BracesOptions BracesOptions { get; set; }

	char[] OpenBraces { get; set; }

	char[] ClosingBraces { get; set; }

	bool FindClosingBrace(ref int x, ref int y);

	bool FindClosingBrace(ref Point position);

	bool FindOpenBrace(ref int x, ref int y);

	bool FindOpenBrace(ref Point position);

	void TempHighlightBraces(Rectangle[] rects);

	void TempUnhighlightBraces();

	void ResetBracesOptions();

	void ResetOpenBraces();

	void ResetClosingBraces();
}
