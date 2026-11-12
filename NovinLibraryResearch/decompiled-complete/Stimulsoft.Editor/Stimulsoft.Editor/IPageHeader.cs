using System.Drawing;

namespace Stimulsoft.Editor;

public interface IPageHeader
{
	string LeftText { get; set; }

	string CenterText { get; set; }

	string RightText { get; set; }

	Font Font { get; set; }

	Color FontColor { get; set; }

	Point Offset { get; set; }

	bool Visible { get; set; }

	bool ReverseOnEvenPages { get; set; }

	void Assign(IPageHeader source);

	int BeginUpdate();

	int EndUpdate();

	void Update();

	void ResetFont();

	void ResetOffset();

	void ResetFontColor();

	void ResetReverseOnEvenPages();

	void Paint(IPainter painter, Rectangle rect, int pageIndex, int pageCount, bool pageNumbers);
}
