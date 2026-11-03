using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Metro.Rendering;

internal class ᣙ : ᣘ
{
	protected override void PaintTabGroupBackground(Graphics g, Office2007RibbonTabGroupColorTable colorTable, Rectangle bounds, Rectangle groupBounds, bool glassEnabled)
	{
		DisplayHelp.FillRectangle(g, groupBounds, colorTable.Background);
		if (StyleManager.Style != eStyle.Office2016)
		{
			Rectangle r = new Rectangle(groupBounds.X, groupBounds.Y, groupBounds.Width, 3);
			DisplayHelp.FillRectangle(g, r, colorTable.Border);
			if (StyleManager.Style == eStyle.OfficeMobile2014)
			{
				DisplayHelp.DrawGradientLine(g, groupBounds.X, groupBounds.Y, groupBounds.X, 24, colorTable.Border, 1);
				DisplayHelp.DrawGradientLine(g, groupBounds.Right - 1, groupBounds.Y, groupBounds.Right - 1, 24, colorTable.Border, 1);
			}
		}
	}
}
