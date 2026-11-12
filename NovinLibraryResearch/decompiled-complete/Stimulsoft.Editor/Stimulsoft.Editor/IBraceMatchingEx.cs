using System.Drawing;

namespace Stimulsoft.Editor;

public interface IBraceMatchingEx : IBraceMatching
{
	Color ForeColor { get; set; }

	Color BackColor { get; set; }

	FontStyle Style { get; set; }

	bool UseRoundRect { get; set; }

	void Assign(IBraceMatchingEx source);

	void ResetForeColor();

	void ResetBackColor();

	void ResetStyle();

	void ResetUseRoundRect();
}
