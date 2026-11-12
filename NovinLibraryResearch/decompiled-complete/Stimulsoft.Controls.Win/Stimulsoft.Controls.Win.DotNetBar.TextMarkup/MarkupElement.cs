using System.Drawing;
using System.Xml;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal abstract class MarkupElement : IBlockExtended, IBlock
{
	private MarkupElementCollection m_Elements;

	private MarkupElement m_Parent;

	private Rectangle m_Bounds = Rectangle.Empty;

	private bool m_Visible = true;

	private bool m_SizeValid;

	private Rectangle m_RenderBounds = Rectangle.Empty;

	private Padding _Margin = new Padding(0);

	public virtual bool IsBlockContainer => false;

	public virtual bool IsBlockElement => false;

	public virtual bool IsNewLineAfterElement => false;

	public virtual bool CanStartNewLine => true;

	public virtual MarkupElementCollection Elements
	{
		get
		{
			if (m_Elements == null)
			{
				m_Elements = new MarkupElementCollection(this);
			}
			return m_Elements;
		}
	}

	public virtual bool IsSizeValid
	{
		get
		{
			return m_SizeValid;
		}
		set
		{
			m_SizeValid = value;
		}
	}

	public virtual MarkupElement Parent => m_Parent;

	public Rectangle Bounds
	{
		get
		{
			return m_Bounds;
		}
		set
		{
			m_Bounds = value;
		}
	}

	public bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			m_Visible = value;
		}
	}

	public Padding Margin
	{
		get
		{
			return _Margin;
		}
		set
		{
			_Margin = value;
		}
	}

	public Rectangle RenderBounds
	{
		get
		{
			return m_RenderBounds;
		}
		set
		{
			m_RenderBounds = value;
		}
	}

	public MarkupElement()
	{
		m_Elements = new MarkupElementCollection(this);
	}

	internal void InvalidateElementsSize()
	{
		IsSizeValid = false;
		if (m_Elements == null || m_Elements.Count == 0)
		{
			return;
		}
		foreach (MarkupElement element in m_Elements)
		{
			element.InvalidateElementsSize();
			element.IsSizeValid = false;
		}
	}

	internal void SetParent(MarkupElement parent)
	{
		m_Parent = parent;
	}

	public abstract void Measure(Size availableSize, MarkupDrawContext d);

	public virtual void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
	}

	public abstract void Render(MarkupDrawContext d);

	public virtual void RenderEnd(MarkupDrawContext d)
	{
	}

	protected abstract void ArrangeCore(Rectangle finalRect, MarkupDrawContext d);

	public void Arrange(Rectangle finalSize, MarkupDrawContext d)
	{
		ArrangeCore(finalSize, d);
	}

	public virtual void ReadAttributes(XmlTextReader reader)
	{
	}
}
