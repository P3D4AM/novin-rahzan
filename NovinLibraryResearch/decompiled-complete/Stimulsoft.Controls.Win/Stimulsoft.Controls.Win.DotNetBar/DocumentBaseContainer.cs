using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public abstract class DocumentBaseContainer
{
	private Rectangle m_LayoutBounds = Rectangle.Empty;

	private Rectangle m_DisplayBounds = Rectangle.Empty;

	private DocumentBaseContainer m_Parent;

	private static long IdCounter;

	private long _Id;

	public long Id => _Id;

	[Browsable(false)]
	public virtual Rectangle DisplayBounds => m_DisplayBounds;

	[Browsable(false)]
	public virtual Rectangle LayoutBounds => m_LayoutBounds;

	public virtual DocumentBaseContainer Parent => m_Parent;

	public abstract bool Visible { get; }

	protected internal abstract Size MinimumSize { get; }

	public DocumentBaseContainer()
	{
		IdCounter++;
		_Id = IdCounter;
	}

	public abstract void Layout(Rectangle bounds);

	internal void SetDisplayBounds(Rectangle r)
	{
		m_DisplayBounds = r;
	}

	internal void SetLayoutBounds(Rectangle r)
	{
		m_LayoutBounds = r;
	}

	public void ResetLayoutBounds()
	{
		m_LayoutBounds = Rectangle.Empty;
	}

	public void ResetDisplayBounds()
	{
		m_DisplayBounds = Rectangle.Empty;
	}

	internal void SetParent(DocumentBaseContainer parent)
	{
		m_Parent = parent;
	}

	public virtual void SetWidth(int width)
	{
		if (m_Parent != null)
		{
			m_Parent.OnSetWidth(this, width);
		}
		if (width >= MinimumSize.Width || MinimumSize.Width == 0)
		{
			ResetDisplayBounds();
			m_LayoutBounds.Width = width;
		}
	}

	public virtual void SetHeight(int height)
	{
		if (m_Parent != null)
		{
			m_Parent.OnSetHeight(this, height);
		}
		if (height >= MinimumSize.Height || MinimumSize.Height == 0)
		{
			ResetDisplayBounds();
			m_LayoutBounds.Height = height;
		}
	}

	protected internal virtual bool OnSetWidth(DocumentBaseContainer doc, int width)
	{
		return false;
	}

	protected internal virtual bool OnSetHeight(DocumentBaseContainer doc, int height)
	{
		return false;
	}
}
