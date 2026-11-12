using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class Paragraph : Div
{
	protected override void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
	{
		base.ArrangeInternal(bounds, d);
		base.Bounds = new Rectangle(base.Bounds.X, base.Bounds.Y, base.Bounds.Width, base.Bounds.Height + d.CurrentFont.Height);
	}
}
