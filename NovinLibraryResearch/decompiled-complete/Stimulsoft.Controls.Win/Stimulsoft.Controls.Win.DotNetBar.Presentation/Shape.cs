using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class Shape
{
	private Location m_Location = new Location();

	private SizeInfo m_Size = new SizeInfo();

	private ShapeCollection m_Children;

	private PaddingInfo m_Padding;

	private bool m_SetChildClip;

	public Location Location => m_Location;

	public SizeInfo Size => m_Size;

	public PaddingInfo Padding
	{
		get
		{
			return m_Padding;
		}
		set
		{
			m_Padding = value;
		}
	}

	public ShapeCollection Children
	{
		get
		{
			if (m_Children == null)
			{
				m_Children = new ShapeCollection();
			}
			return m_Children;
		}
	}

	public bool SetChildClip
	{
		get
		{
			return m_SetChildClip;
		}
		set
		{
			m_SetChildClip = value;
		}
	}

	public virtual void Paint(ShapePaintInfo p)
	{
		if (m_Children == null || m_Children.Count <= 0)
		{
			return;
		}
		System.Drawing.Rectangle bounds = p.Bounds;
		System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(GetLocation(p.Bounds), GetSize(p.Bounds));
		if (m_Padding != null)
		{
			rectangle.Width -= m_Padding.HorizontalPadding;
			rectangle.Height -= m_Padding.VerticalPadding;
			rectangle.X += m_Padding.Left;
			rectangle.Y += m_Padding.Top;
		}
		p.Bounds = rectangle;
		Region val = ClipChildren(p.Graphics, rectangle);
		if (m_SetChildClip && p.Graphics.Clip != null)
		{
			p.ChildContentClip = p.Graphics.Clip;
		}
		foreach (Shape child in m_Children)
		{
			child.Paint(p);
		}
		if (p.ChildContentClip != null)
		{
			p.ChildContentClip.Dispose();
			p.ChildContentClip = null;
		}
		if (val != null)
		{
			p.Graphics.Clip = val;
			val.Dispose();
		}
		else
		{
			p.Graphics.ResetClip();
		}
		p.Bounds = bounds;
		if (val != null)
		{
			val.Dispose();
		}
	}

	protected virtual Region ClipChildren(Graphics g, System.Drawing.Rectangle childBounds)
	{
		Region clip = g.Clip;
		g.SetClip(childBounds);
		return clip;
	}

	protected virtual Point GetLocation(System.Drawing.Rectangle bounds)
	{
		return GetLocation(bounds, m_Location);
	}

	protected virtual Point GetLocation(System.Drawing.Rectangle bounds, Location refLocation)
	{
		Point location = bounds.Location;
		if (refLocation.RelativeX == eRelativeLocation.Right)
		{
			location.X = bounds.Right;
		}
		else if (refLocation.RelativeX == eRelativeLocation.Top)
		{
			location.X = bounds.Y;
		}
		else if (refLocation.RelativeX == eRelativeLocation.Bottom)
		{
			location.X = bounds.Bottom;
		}
		if (refLocation.RelativeY == eRelativeLocation.Bottom)
		{
			location.Y = bounds.Bottom;
		}
		else if (refLocation.RelativeY == eRelativeLocation.Right)
		{
			location.Y = bounds.Right;
		}
		else if (refLocation.RelativeY == eRelativeLocation.Left)
		{
			location.Y = bounds.X;
		}
		location.Offset(refLocation.X, refLocation.Y);
		return location;
	}

	protected virtual Size GetSize(System.Drawing.Rectangle bounds)
	{
		Size size = bounds.Size;
		if (m_Size.RelativeWidth == eRelativeSize.Width)
		{
			size.Width = bounds.Width + m_Size.Width;
		}
		else if (m_Size.RelativeWidth == eRelativeSize.Height)
		{
			size.Width = bounds.Height + m_Size.Height;
		}
		else if (m_Size.Width != 0)
		{
			size.Width = m_Size.Width;
		}
		if (m_Size.RelativeHeight == eRelativeSize.Height)
		{
			size.Height = bounds.Height + m_Size.Height;
		}
		else if (m_Size.RelativeHeight == eRelativeSize.Width)
		{
			size.Height = bounds.Width + m_Size.Height;
		}
		else if (m_Size.Height != 0)
		{
			size.Height = m_Size.Height;
		}
		return size;
	}

	protected virtual System.Drawing.Rectangle GetBounds(System.Drawing.Rectangle parentBounds)
	{
		return new System.Drawing.Rectangle(GetLocation(parentBounds), GetSize(parentBounds));
	}
}
