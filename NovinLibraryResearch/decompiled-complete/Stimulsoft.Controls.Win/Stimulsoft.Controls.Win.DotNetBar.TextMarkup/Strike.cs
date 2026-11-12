using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class Strike : MarkupElement
{
	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = Rectangle.Empty;
	}

	public override void Render(MarkupDrawContext d)
	{
		d.StrikeOut = true;
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		d.StrikeOut = false;
		base.RenderEnd(d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}
}
