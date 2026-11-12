using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface ISyntaxSettings : IPersistentSettings
{
	LexStyle[] LexStyles { get; set; }

	IColorThemes ColorThemes { get; set; }

	Font Font { get; set; }

	NavigateOptions NavigateOptions { get; set; }

	RichTextBoxScrollBars ScrollBars { get; set; }

	SelectionOptions SelectionOptions { get; set; }

	GutterOptions GutterOptions { get; set; }

	SeparatorOptions SeparatorOptions { get; set; }

	OutlineOptions OutlineOptions { get; set; }

	bool ShowMargin { get; set; }

	bool ShowGutter { get; set; }

	bool HighlightUrls { get; set; }

	bool AllowOutlining { get; set; }

	bool UseSpaces { get; set; }

	bool WordWrap { get; set; }

	bool WhiteSpaceVisible { get; set; }

	int GutterWidth { get; set; }

	int MarginPos { get; set; }

	int[] TabStops { get; set; }

	string[] EventNames { get; set; }

	KeyListData[] EventData { get; set; }

	void LoadFromEdit(SyntaxEdit edit);

	void ApplyToEdit(SyntaxEdit edit);

	bool IsDescriptionEnabled(int index);

	bool IsFontStyleEnabled(int index);

	bool IsBackColorEnabled(int index);

	void Localize();
}
