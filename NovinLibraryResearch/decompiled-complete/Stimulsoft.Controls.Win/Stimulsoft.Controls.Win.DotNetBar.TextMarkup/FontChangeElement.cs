using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class FontChangeElement : MarkupElement
{
	protected Font m_OldFont;

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
		if (m_OldFont != null)
		{
			d.CurrentFont = m_OldFont;
		}
		m_OldFont = null;
		base.RenderEnd(d);
	}

	public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
		if (m_OldFont != null)
		{
			d.CurrentFont = m_OldFont;
		}
		m_OldFont = null;
		base.MeasureEnd(availableSize, d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}
}
