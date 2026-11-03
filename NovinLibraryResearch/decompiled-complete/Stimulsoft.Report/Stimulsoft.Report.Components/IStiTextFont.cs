using System.Drawing;

namespace Stimulsoft.Report.Components;

public interface IStiTextFont
{
	Font GetFont();

	void SetFontName(string fontName);

	void SetFontSize(float fontSize);

	void GrowFontSize();

	void ShrinkFontSize();

	void SetFontBoldStyle(bool isBold);

	void SetFontItalicStyle(bool isItalic);

	void SetFontUnderlineStyle(bool isUnderline);
}
