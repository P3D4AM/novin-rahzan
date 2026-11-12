using System.Drawing;

namespace Stimulsoft.Editor;

public interface ILineSeparator
{
	SeparatorOptions Options { get; set; }

	Color HighlightBackColor { get; set; }

	Color HighlightForeColor { get; set; }

	Color LineColor { get; set; }

	Color ContentDividerColor { get; set; }

	void Assign(ILineSeparator source);

	bool NeedHighlight();

	bool NeedHide();

	void ResetOptions();

	void ResetHighlightForeColor();

	void ResetHighlightBackColor();

	void ResetLineColor();

	void ResetContentDividerColor();

	void TempHighlightLine(int index);

	void TempUnhighlightLine();
}
