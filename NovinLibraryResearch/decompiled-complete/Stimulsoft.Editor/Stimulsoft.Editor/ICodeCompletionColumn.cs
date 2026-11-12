using System.Drawing;

namespace Stimulsoft.Editor;

public interface ICodeCompletionColumn
{
	string Name { get; set; }

	FontStyle FontStyle { get; set; }

	Color ForeColor { get; set; }

	bool Visible { get; set; }

	void ResetFontStyle();

	void ResetForeColor();

	void ResetVisible();
}
