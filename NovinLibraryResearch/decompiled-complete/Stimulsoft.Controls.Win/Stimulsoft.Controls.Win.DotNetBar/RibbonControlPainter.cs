using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class RibbonControlPainter
{
	public virtual void PaintBackground(RibbonControlRendererEventArgs e)
	{
	}

	public virtual void PaintCaptionBackground(RibbonControlRendererEventArgs e, Rectangle displayRect)
	{
	}

	public virtual void PaintCaptionText(RibbonControlRendererEventArgs e)
	{
	}

	public virtual void PaintQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
	}
}
