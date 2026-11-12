using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class ShapePath : Shape
{
	private ShapeBorder m_Border;

	private ShapeFill m_Fill;

	private GraphicsPath m_Path;

	public GraphicsPath Path
	{
		get
		{
			return m_Path;
		}
		set
		{
			m_Path = value;
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

	public ShapePath()
	{
	}

	public ShapePath(ShapeBorder border)
	{
		m_Border = border;
	}

	public ShapePath(ShapeFill fill)
	{
		m_Fill = fill;
	}

	public ShapePath(ShapeBorder border, ShapeFill fill)
	{
		m_Border = border;
		m_Fill = fill;
	}

	public override void Paint(ShapePaintInfo p)
	{
		if (m_Path != null)
		{
			System.Drawing.Rectangle bounds = GetBounds(p.Bounds);
			Graphics graphics = p.Graphics;
			if (m_Fill != null)
			{
				PaintFill(graphics, bounds);
			}
			if (m_Border != null)
			{
				PaintBorder(graphics, bounds);
			}
			base.Paint(p);
		}
	}

	protected virtual void PaintBorder(Graphics g, System.Drawing.Rectangle r)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (m_Border.Color1.IsEmpty)
		{
			return;
		}
		ShapeBorder border = m_Border;
		object obj = m_Path.Clone();
		GraphicsPath val = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
		try
		{
			val.Transform(new Matrix(1f, 0f, 0f, 1f, (float)r.X, (float)r.Y));
			DisplayHelp.DrawGradientPathBorder(g, val, border.Color1, border.Color2, border.GradientAngle, border.Width);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void PaintFill(Graphics g, System.Drawing.Rectangle r)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		if (r.Width <= 0 || r.Height <= 0 || m_Fill == null)
		{
			return;
		}
		object obj = m_Path.Clone();
		GraphicsPath val = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
		try
		{
			System.Drawing.Rectangle rectangle = System.Drawing.Rectangle.Ceiling(val.GetBounds());
			int num = r.X + (int)Math.Ceiling((double)(Math.Max(r.Width, rectangle.Width) - rectangle.Width) / 2.0);
			int num2 = r.Y + (r.Height - rectangle.Height) / 2;
			Matrix val2 = new Matrix(1f, 0f, 0f, 1f, (float)num, (float)num2);
			try
			{
				val.Transform(val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			rectangle = System.Drawing.Rectangle.Ceiling(val.GetBounds());
			Brush val3 = m_Fill.CreateBrush(rectangle);
			if (val3 != null)
			{
				g.FillPath(val3, val);
				val3.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
