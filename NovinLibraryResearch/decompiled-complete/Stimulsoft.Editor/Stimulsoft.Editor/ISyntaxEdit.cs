using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface ISyntaxEdit : ISyntaxPaint, ISearch, ITextSearch, INotifier, ICaret, INavigateEx, INavigate, IEdit, IWordWrap, IFmtExportEx, IFmtExport, IExport, IFmtImport, IImport, ICodeCompletion, IRecordPlayBack, IEditColors, ISplitView, IAutoCorrect, IControl
{
	ITextSource Source { get; set; }

	ISyntaxStrings Lines { get; set; }

	IDisplayStrings DisplayLines { get; }

	ISelection Selection { get; set; }

	IGutter Gutter { get; set; }

	IMargin EditMargin { get; set; }

	IPainter Painter { get; }

	IKeyList KeyList { get; }

	ILineStylesEx LineStyles { get; set; }

	IPrinting Printing { get; set; }

	IWhiteSpace WhiteSpace { get; set; }

	ILineSeparator LineSeparator { get; set; }

	IOutlining Outlining { get; set; }

	IBraceMatchingEx Braces { get; set; }

	IHyperTextEx HyperText { get; set; }

	ISpellingEx Spelling { get; set; }

	IScrolling Scrolling { get; set; }

	IEditPages Pages { get; set; }

	bool AcceptTabs { get; set; }

	bool AcceptReturns { get; set; }

	EditBorderStyle BorderStyle { get; set; }

	Color BorderColor { get; set; }

	Rectangle ClientRect { get; }

	bool Transparent { get; set; }

	bool SyntaxErrorsHints { get; set; }

	bool UseDefaultMenu { get; set; }

	bool DrawColumnsIndent { get; set; }

	Color ColumnsIndentForeColor { get; set; }

	event NotifyEvent SourceStateChanged;

	event EventHandler ModifiedChanged;

	event PaintEventHandler PaintBackground;

	Point ScreenToText(Point position);

	Point TextToScreen(Point position);

	Point ScreenToText(int x, int y);

	Point TextToScreen(int x, int y);

	Point ScreenToDisplay(int x, int y);

	Point DisplayToScreen(int x, int y);

	void MoveCaretOnDrag();

	void MakeVisible(Point position);

	void MakeVisible(Point position, bool centerLine);

	void ShowScrollHint(int pos);

	void HideScrollHint();

	int CharsInWidth();

	int CharsInWidth(int width);

	int LinesInHeight();

	int LinesInHeight(int height);

	void GetHitTest(Point position, ref HitTestInfo hitTestInfo);

	void GetHitTest(int x, int y, ref HitTestInfo hitTestInfo);

	void GetHitTestAtTextPoint(Point position, ref HitTestInfo hitTestInfo);

	void GetHitTestAtTextPoint(int x, int y, ref HitTestInfo hitTestInfo);

	string GetTextAtCursor();

	DialogResult DisplayGotoLineDialog();

	DialogResult DisplaySearchDialog();

	DialogResult DisplayReplaceDialog();

	DialogResult DisplayEditorSettingsDialog(EditorSettingsTab hiddenTabs);

	DialogResult DisplayEditorSettingsDialog();

	void ProcessKeyPress(char keyChar);

	void ResetAcceptTabs();

	void ResetAcceptReturns();

	void ResetBorderStyle();

	void ResetBorderColor();

	void ResetSyntaxErrorsHints();

	void ResetDrawColumnsIndent();

	void ResetColumnsIndentForeColor();
}
