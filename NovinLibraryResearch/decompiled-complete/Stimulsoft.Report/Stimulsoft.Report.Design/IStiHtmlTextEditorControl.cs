namespace Stimulsoft.Report.Design;

public interface IStiHtmlTextEditorControl
{
	string HtmlText { get; set; }

	bool ShowFontNameBox { get; set; }

	bool ShowFontSizeBox { get; set; }

	bool ShowFontBold { get; set; }

	bool ShowFontItalic { get; set; }

	bool ShowFontUnderline { get; set; }

	bool ShowFontColor { get; set; }

	bool ShowAlignLeft { get; set; }

	bool ShowAlignCenter { get; set; }

	bool ShowAlignRight { get; set; }

	bool ShowAlignJustify { get; set; }

	bool ShowClearAllFormatting { get; set; }
}
