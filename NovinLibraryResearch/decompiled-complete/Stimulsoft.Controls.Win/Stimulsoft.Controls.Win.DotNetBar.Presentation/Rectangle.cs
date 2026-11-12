using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class Rectangle : Shape
{
	private int m_CornerSize;

	private ShapeBorder m_Border = new ShapeBorder();

	private ShapeFill m_Fill = new ShapeFill();

	private eCornerType m_TopLeftCornerType = eCornerType.Square;

	private eCornerType m_TopRightCornerType = eCornerType.Square;

	private eCornerType m_BottomLeftCornerType = eCornerType.Square;

	private eCornerType m_BottomRightCornerType = eCornerType.Square;

	public int CornerSize
	{
		get
		{
			return m_CornerSize;
		}
		set
		{
			m_CornerSize = value;
		}
	}

	public ShapeBorder Border
	{
		get
		{
			return m_Border;
		}
		set
		{
			m_Border = value;
		}
	}

	public ShapeFill Fill
	{
		get
		{
			return m_Fill;
		}
		set
		{
			m_Fill = value;
		}
	}

	public eCornerType CornerType
	{
		get
		{
			return m_TopLeftCornerType;
		}
		set
		{
			m_TopLeftCornerType = value;
			m_TopRightCornerType = value;
			m_BottomLeftCornerType = value;
			m_BottomRightCornerType = value;
		}
	}

	public eCornerType TopLeftCornerType
	{
		get
		{
			return m_TopLeftCornerType;
		}
		set
		{
			m_TopLeftCornerType = value;
		}
	}

	public eCornerType TopRightCornerType
	{
		get
		{
			return m_TopRightCornerType;
		}
		set
		{
			m_TopRightCornerType = value;
		}
	}

	public eCornerType BottomLeftCornerType
	{
		get
		{
			return m_BottomLeftCornerType;
		}
		set
		{
			m_BottomLeftCornerType = value;
		}
	}

	public eCornerType BottomRightCornerType
	{
		get
		{
			return m_BottomRightCornerType;
		}
		set
		{
			m_BottomRightCornerType = value;
		}
	}

	public Rectangle()
	{
	}

	public Rectangle(ShapeBorder border)
	{
		m_Border = border;
	}

	public Rectangle(ShapeBorder border, int cornerSize, eCornerType cornerType)
	{
		m_Border = border;
		m_CornerSize = cornerSize;
		CornerType = cornerType;
	}

	public Rectangle(ShapeBorder border, ShapeFill fill)
	{
		m_Border = border;
		m_Fill = fill;
	}

	public Rectangle(ShapeBorder border, ShapeFill fill, int cornerSize, eCornerType cornerType)
	{
		m_Border = border;
		m_Fill = fill;
		m_CornerSize = cornerSize;
		CornerType = cornerType;
	}

	public Rectangle(ShapeBorder border, ShapeFill fill, int cornerSize, eCornerType cornerType, PaddingInfo padding)
	{
		m_Border = border;
		m_Fill = fill;
		m_CornerSize = cornerSize;
		base.Padding = padding;
		CornerType = cornerType;
	}

	public Rectangle(ShapeFill fill)
	{
		m_Fill = fill;
	}

	public override void Paint(ShapePaintInfo p)
	{
		System.Drawing.Rectangle bounds = GetBounds(p.Bounds);
		Graphics graphics = p.Graphics;
		PaintFill(graphics, bounds);
		PaintBorder(graphics, bounds);
		base.Paint(p);
	}

	protected virtual void PaintBorder(Graphics g, System.Drawing.Rectangle r)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		if (m_Border.Width == 0 || m_Border.Color1.IsEmpty || r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		int cornerSize = m_CornerSize;
		ShapeBorder border = m_Border;
		r.Width--;
		r.Height--;
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, m_CornerSize, eStyleBackgroundPathPart.Complete, m_TopLeftCornerType, m_TopRightCornerType, m_BottomLeftCornerType, m_BottomRightCornerType);
		try
		{
			Pen val = new Pen(border.Color1, (float)border.Width);
			try
			{
				roundedRectanglePath.Widen(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (border.Color2.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(border.Color1);
				try
				{
					g.FillPath((Brush)(object)val2, roundedRectanglePath);
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			LinearGradientBrush val3 = DisplayHelp.CreateLinearGradientBrush(r, border.Color1, border.Color2, border.GradientAngle);
			try
			{
				g.FillPath((Brush)(object)val3, roundedRectanglePath);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)roundedRectanglePath)?.Dispose();
		}
	}

	protected virtual void PaintFill(Graphics g, System.Drawing.Rectangle r)
	{
		if (r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		int cornerSize = m_CornerSize;
		if (cornerSize > 0)
		{
			GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, m_CornerSize, eStyleBackgroundPathPart.Complete, m_TopLeftCornerType, m_TopRightCornerType, m_BottomLeftCornerType, m_BottomRightCornerType);
			try
			{
				Brush val = m_Fill.CreateBrush(System.Drawing.Rectangle.Ceiling(roundedRectanglePath.GetBounds()));
				if (val != null)
				{
					DisplayHelp.FillPath(g, roundedRectanglePath, m_Fill.Color1, m_Fill.Color2, m_Fill.GradientAngle);
					val.Dispose();
				}
				return;
			}
			finally
			{
				((IDisposable)roundedRectanglePath)?.Dispose();
			}
		}
		Brush val2 = m_Fill.CreateBrush(r);
		if (val2 != null)
		{
			g.FillRectangle(val2, r);
			val2.Dispose();
		}
	}

	protected override Region ClipChildren(Graphics g, System.Drawing.Rectangle childBounds)
	{
		if (m_CornerSize > 0)
		{
			Region clip = g.Clip;
			GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(childBounds, m_CornerSize, eStyleBackgroundPathPart.Complete, m_TopLeftCornerType, m_TopRightCornerType, m_BottomLeftCornerType, m_BottomRightCornerType);
			try
			{
				g.SetClip(roundedRectanglePath);
				return clip;
			}
			finally
			{
				((IDisposable)roundedRectanglePath)?.Dispose();
			}
		}
		return base.ClipChildren(g, childBounds);
	}
}
