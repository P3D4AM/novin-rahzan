using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

internal class ScrollBarPainter
{
	public virtual void PaintThumb(Graphics g, Rectangle bounds, eScrollThumbPosition position, eScrollBarState state)
	{
	}

	public virtual void PaintTrackVertical(Graphics g, Rectangle bounds, eScrollBarState state)
	{
	}

	public virtual void PaintTrackHorizontal(Graphics g, Rectangle bounds, eScrollBarState state)
	{
	}

	public virtual void PaintBackground(Graphics g, Rectangle bounds, eScrollBarState state, bool horizontal, bool sideBorderOnly, bool rtl)
	{
	}
}
