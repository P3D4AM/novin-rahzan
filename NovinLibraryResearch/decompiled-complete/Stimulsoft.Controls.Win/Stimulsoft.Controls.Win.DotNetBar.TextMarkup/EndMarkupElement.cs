using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class EndMarkupElement : MarkupElement
{
	private MarkupElement m_StartElement;

	public MarkupElement StartElement => m_StartElement;

	public EndMarkupElement(MarkupElement startElement)
	{
		m_StartElement = startElement;
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		m_StartElement.MeasureEnd(availableSize, d);
		base.Bounds = Rectangle.Empty;
	}

	public override void Render(MarkupDrawContext d)
	{
		m_StartElement.RenderEnd(d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
		base.Bounds = Rectangle.Empty;
	}
}
