using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class Underline : FontChangeElement
{
	protected override void SetFont(MarkupDrawContext d)
	{
		d.Underline = true;
		base.SetFont(d);
	}

	public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
		d.Underline = false;
		base.MeasureEnd(availableSize, d);
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		d.Underline = false;
		base.RenderEnd(d);
	}
}
