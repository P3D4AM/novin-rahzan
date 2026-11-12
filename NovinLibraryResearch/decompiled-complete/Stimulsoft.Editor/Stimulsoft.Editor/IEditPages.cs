using System.Collections;
using System.Drawing;
using System.Drawing.Printing;

namespace Stimulsoft.Editor;

public interface IEditPages : IEnumerable
{
	IList Items { get; }

	IEditPage this[int index] { get; set; }

	PageType PageType { get; set; }

	PaperKind PageKind { get; set; }

	IEditPage DefaultPage { get; set; }

	Color BackColor { get; set; }

	Color BorderColor { get; set; }

	bool DisplayWhiteSpace { get; set; }

	int Count { get; }

	int Height { get; }

	int Width { get; }

	IEditRuler VertRuler { get; }

	IEditRuler HorzRuler { get; }

	RulerUnits RulerUnits { get; set; }

	RulerOptions RulerOptions { get; set; }

	EditRulers Rulers { get; set; }

	bool ApplyRulerToAllPages { get; set; }

	Color RulerBackColor { get; set; }

	Color RulerIndentBackColor { get; set; }

	bool Transparent { get; set; }

	event DrawHeaderEvent DrawHeader;

	int BeginUpdate();

	int EndUpdate();

	void Update();

	void Update(IEditPage page);

	void Update(IEditPage page, bool changed);

	IEditPage Add();

	int Add(IEditPage page);

	void Remove(IEditPage page);

	void Clear();

	int GetPageIndexAt(Point position);

	int GetPageIndexAt(int x, int y);

	int GetPageIndexAtPoint(Point position);

	int GetPageIndexAtPoint(int x, int y);

	int GetPageIndexAtCursor();

	IEditPage GetPageAt(Point position);

	IEditPage GetPageAt(int x, int y);

	IEditPage GetPageAtPoint(Point position);

	IEditPage GetPageAtPoint(int x, int y);

	IEditPage GetPageAtCursor();

	void Paint(IPainter painter, Rectangle rect);

	void InitDefaultPageSettings(PageSettings settings);

	void CancelDragging();

	void ResetPageType();

	void ResetBackColor();

	void ResetBorderColor();

	void ResetDisplayWhiteSpace();

	void ResetRulerUnits();

	void ResetRulerOptions();

	void ResetRulers();

	void ResetRulerBackColor();

	void ResetRulerIndentBackColor();
}
