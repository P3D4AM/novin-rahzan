using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroRibbonTabGroupPainter : Office2007RibbonTabGroupPainter
{
	protected override void PaintTabGroupBackground(Graphics g, Office2007RibbonTabGroupColorTable colorTable, Rectangle bounds, Rectangle groupBounds, bool glassEnabled)
	{
		DisplayHelp.FillRectangle(g, groupBounds, colorTable.Background);
		Rectangle r = new Rectangle(groupBounds.X, groupBounds.Y, groupBounds.Width, 3);
		DisplayHelp.FillRectangle(g, r, colorTable.Border);
	}
}
