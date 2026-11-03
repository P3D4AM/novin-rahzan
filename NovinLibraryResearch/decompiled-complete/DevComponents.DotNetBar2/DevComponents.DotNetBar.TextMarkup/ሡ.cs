using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሡ : _0CBF
{
	protected Font ሢ;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = Rectangle.Empty;
		SetFont(d);
	}

	public override void Render(MarkupDrawContext d)
	{
		SetFont(d);
	}

	protected virtual void SetFont(MarkupDrawContext d)
	{
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		if (ሢ != null)
		{
			d.CurrentFont = ሢ;
		}
		ሢ = null;
		base.RenderEnd(d);
	}

	public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
		if (ሢ != null)
		{
			d.CurrentFont = ሢ;
		}
		ሢ = null;
		base.MeasureEnd(availableSize, d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}
}
