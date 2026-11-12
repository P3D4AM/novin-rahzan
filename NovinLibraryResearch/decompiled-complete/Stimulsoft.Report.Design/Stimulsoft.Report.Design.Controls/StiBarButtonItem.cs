using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Controls;

public class StiBarButtonItem : StiCustomItem
{
	public override void Paint(ItemPaintArgs e)
	{
		ButtonItemPainter buttonItemPainter = PainterFactory.CreateButtonPainter(this);
		Rectangle mouseOverRectangle = buttonItemPainter.GetMouseOverRectangle(this, e, null);
		if (Checked)
		{
			mouseOverRectangle.Inflate(-1, -1);
			if (buttonItemPainter is Office2003ButtonItemPainter office2003ButtonItemPainter)
			{
				office2003ButtonItemPainter.PaintButtonCheckBackground(this, e, mouseOverRectangle);
			}
		}
		if (base.IsMouseOver)
		{
			buttonItemPainter.PaintButtonMouseOver(this, e, null, mouseOverRectangle);
		}
		e.Graphics.DrawImage(base.Image, new Rectangle(mouseOverRectangle.X + (mouseOverRectangle.Width - base.Image.Width) / 2, mouseOverRectangle.Y + (mouseOverRectangle.Height - base.Image.Height) / 2, base.Image.Width, base.Image.Height));
	}
}
