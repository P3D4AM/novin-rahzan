using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IGutter
{
	int Width { get; set; }

	Brush Brush { get; set; }

	Pen Pen { get; set; }

	Color BrushColor { get; set; }

	Color PenColor { get; set; }

	bool Visible { get; set; }

	Rectangle Rect { get; }

	int LineNumbersStart { get; set; }

	int LineNumbersLeftIndent { get; set; }

	int LineNumbersRightIndent { get; set; }

	Color LineNumbersForeColor { get; set; }

	Color LineNumbersBackColor { get; set; }

	StringAlignment LineNumbersAlignment { get; set; }

	int OutliningLeftIndent { get; set; }

	int OutliningRightIndent { get; set; }

	GutterOptions Options { get; set; }

	ImageList Images { get; set; }

	int BookMarkImageIndex { get; set; }

	int WrapImageIndex { get; set; }

	bool DrawLineBookmarks { get; set; }

	Color LineBookmarksColor { get; set; }

	bool ShowBookmarkHints { get; set; }

	Color LineModificatorChangedColor { get; set; }

	Color LineModificatorSavedColor { get; set; }

	int UserMarginWidth { get; set; }

	Color UserMarginForeColor { get; set; }

	Color UserMarginBackColor { get; set; }

	string UserMarginText { get; set; }

	event EventHandler Click;

	event EventHandler DoubleClick;

	event DrawUserMarginEvent DrawUserMargin;

	void Assign(IGutter source);

	void Paint(IPainter painter, Rectangle rect, int startLine);

	void Paint(IPainter painter, Rectangle rect);

	void OnClick(EventArgs e);

	void OnDoubleClick(EventArgs e);

	void ResetBrushColor();

	void ResetPenColor();

	void ResetVisible();

	void ResetLineNumbersStart();

	void ResetLineNumbersLeftIndent();

	void ResetLineNumbersRightIndent();

	void ResetLineNumbersForeColor();

	void ResetLineNumbersBackColor();

	void ResetLineNumbersAlignment();

	void ResetOutliningLeftIndent();

	void ResetOutliningRightIndent();

	void ResetOptions();

	void ResetBookMarkImageIndex();

	void ResetWrapImageIndex();

	void ResetDrawLineBookmarks();

	void ResetLineBookmarksColor();

	void ResetShowBookmarkHints();

	void ResetLineModificatorChangedColor();

	void ResetLineModificatorSavedColor();

	void ResetWidth();

	void ResetUserMarginWidth();

	void ResetUserMarginForeColor();

	void ResetUserMarginBackColor();

	void ResetUserMarginText();
}
