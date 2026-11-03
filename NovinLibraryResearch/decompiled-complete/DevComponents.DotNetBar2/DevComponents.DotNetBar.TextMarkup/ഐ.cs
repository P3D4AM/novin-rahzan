using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ഐ : _0CBF
{
	private _0CBF ഒ;

	public _0CBF _0D11 => ഒ;

	public ഐ(_0CBF startElement)
	{
		ഒ = startElement;
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		ഒ.MeasureEnd(availableSize, d);
		base.Bounds = Rectangle.Empty;
	}

	public override void Render(MarkupDrawContext d)
	{
		ഒ.RenderEnd(d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
		base.Bounds = Rectangle.Empty;
	}
}
