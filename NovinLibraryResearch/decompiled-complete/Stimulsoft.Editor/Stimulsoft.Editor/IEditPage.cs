using System.Drawing;
using System.Drawing.Printing;

namespace Stimulsoft.Editor;

public interface IEditPage
{
	Size PageSize { get; set; }

	Margins Margins { get; set; }

	PaperKind PageKind { get; set; }

	bool Landscape { get; set; }

	int HorzOffset { get; set; }

	int VertOffset { get; set; }

	bool IsFirstPage { get; }

	bool IsLastPage { get; }

	IEditPage PrevPage { get; }

	IEditPage NextPage { get; }

	IPageHeader Header { get; set; }

	IPageHeader Footer { get; set; }

	Point Origin { get; }

	Rectangle BoundsRect { get; }

	Rectangle ClientRect { get; }

	int StartLine { get; }

	int EndLine { get; }

	int Index { get; }

	bool PaintNumber { get; set; }

	bool UsePrinterSettings { get; set; }

	void Assign(IEditPage source);

	void Update();

	void Invalidate();

	int BeginUpdate();

	int EndUpdate();

	void Paint(IPainter painter);

	Rectangle GetBounds(bool includeSpace);
}
