using System.Drawing;

namespace Stimulsoft.Editor;

public interface IMargin
{
	int Position { get; set; }

	int[] ColumnPositions { get; set; }

	Pen Pen { get; set; }

	Color PenColor { get; set; }

	Pen ColumnPen { get; set; }

	Color ColumnPenColor { get; set; }

	bool Visible { get; set; }

	bool ColumnsVisible { get; set; }

	bool IsDragging { get; set; }

	bool AllowDrag { get; set; }

	bool ShowHints { get; set; }

	void Assign(IMargin source);

	void Paint(IPainter painter, Rectangle rect);

	void PaintColumn(IPainter painter, Rectangle rect);

	void ResetPosition();

	void ResetColumnPositions();

	void ResetPenColor();

	void ResetVisible();

	void ResetColumnsPenColor();

	void ResetColumnsVisible();

	void ResetAllowDrag();

	void ResetShowHints();

	void CancelDragging();

	bool Contains(int x, int y);

	void DragTo(int x, int y);
}
