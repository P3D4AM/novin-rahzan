using System.Drawing;

namespace Stimulsoft.Editor;

public interface ILexStyle
{
	string Name { get; set; }

	string Desc { get; set; }

	Color ForeColor { get; set; }

	Color BackColor { get; set; }

	FontStyle FontStyle { get; set; }

	bool ForeColorEnabled { get; set; }

	bool BackColorEnabled { get; set; }

	bool BoldEnabled { get; set; }

	bool ItalicEnabled { get; set; }

	bool UnderlineEnabled { get; set; }

	bool PlainText { get; set; }

	int Index { get; }

	void Assign(ILexStyle source);

	void ResetForeColor();

	void ResetBackColor();

	void ResetFontStyle();

	void ResetPlainText();
}
