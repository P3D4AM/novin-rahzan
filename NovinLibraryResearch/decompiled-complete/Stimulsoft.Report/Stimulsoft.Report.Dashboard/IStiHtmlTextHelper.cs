using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard;

internal interface IStiHtmlTextHelper
{
	string SetFontName(string text, string fontName, Color defaultColor);

	string SetFontSize(string text, float fontSize, Color defaultColor);

	string GrowFontSize(string text, Color defaultColor);

	string ShrinkFontSize(string text, Color defaultColor);

	string SetFontBoldStyle(string text, bool isBold, Color defaultColor);

	string SetFontItalicStyle(string text, bool isItalic, Color defaultColor);

	string SetFontUnderlineStyle(string text, bool isUnderline, Color defaultColor);

	string SetColor(string text, Color color);

	string SetHorAlignment(string text, StiTextHorAlignment alignment, Color defaultColor);

	Font GetFont(string text, Color defaultColor);

	Color GetColor(string text, Color defaultColor);

	StiTextHorAlignment GetHorAlign(string text, Color defaultColor);

	string GetSimpleText(string htmlText, Color defaultColor);
}
