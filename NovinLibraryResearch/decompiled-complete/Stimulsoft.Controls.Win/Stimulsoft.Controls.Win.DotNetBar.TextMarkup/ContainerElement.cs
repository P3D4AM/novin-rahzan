using System.Drawing;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class ContainerElement : MarkupElement
{
	private SerialContentLayoutManager m_Layout;

	private MarkupLayoutManager m_MarkupLayout;

	public override bool IsBlockElement => true;

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		ArrangeInternal(new Rectangle(Point.Empty, availableSize), d);
	}

	protected virtual SerialContentLayoutManager GetLayoutManager(bool mutliLine)
	{
		if (m_Layout == null)
		{
			m_Layout = new SerialContentLayoutManager();
			m_Layout.MultiLine = mutliLine;
			m_Layout.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			m_Layout.BlockLineAlignment = eContentVerticalAlignment.Top;
			m_Layout.BlockSpacing = 0;
		}
		m_Layout.EvenHeight = true;
		return m_Layout;
	}

	private MarkupLayoutManager GetMarkupLayout()
	{
		if (m_MarkupLayout == null)
		{
			m_MarkupLayout = new MarkupLayoutManager();
		}
		return m_MarkupLayout;
	}

	public override void Render(MarkupDrawContext d)
	{
		Point containerOffset = GetContainerOffset();
		d.Offset.Offset(containerOffset.X, containerOffset.Y);
		try
		{
			foreach (MarkupElement element in Elements)
			{
				element.Render(d);
			}
		}
		finally
		{
			d.Offset.Offset(-containerOffset.X, -containerOffset.Y);
		}
	}

	protected virtual Point GetContainerOffset()
	{
		return base.Bounds.Location;
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
		base.Bounds = finalRect;
		ArrangeInternal(finalRect, d);
	}

	protected virtual void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
	{
		SerialContentLayoutManager layoutManager = GetLayoutManager(d.AllowMultiLine);
		layoutManager.RightToLeft = d.RightToLeft;
		MarkupLayoutManager markupLayout = GetMarkupLayout();
		markupLayout.MarkupDrawContext = d;
		try
		{
			MarkupElement[] array = new MarkupElement[Elements.Count];
			Elements.CopyTo(array);
			Rectangle containerBounds = new Rectangle(Point.Empty, bounds.Size);
			IBlock[] contentBlocks = array;
			Rectangle rectangle = layoutManager.Layout(containerBounds, contentBlocks, markupLayout);
			base.Bounds = new Rectangle(bounds.Location, rectangle.Size);
		}
		finally
		{
			markupLayout.MarkupDrawContext = null;
		}
	}
}
