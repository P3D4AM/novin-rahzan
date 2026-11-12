using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal abstract class SplitterPainter
{
	private int m_ArrowSize = 6;

	protected int ArrowSize
	{
		get
		{
			return m_ArrowSize;
		}
		set
		{
			m_ArrowSize = value;
		}
	}

	public SplitterPainter()
	{
	}

	public virtual void Paint(SplitterPaintInfo info)
	{
	}

	protected virtual void PaintArrow(Point p, SplitterPaintInfo info)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		GraphicsPath trianglePath = UIGraphics.GetTrianglePath(p, ArrowSize, GetDirection(info));
		if (!info.Colors.ExpandFillColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(info.Colors.ExpandFillColor);
			try
			{
				info.Graphics.FillPath((Brush)(object)val, trianglePath);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (!info.Colors.ExpandLineColor.IsEmpty)
		{
			Pen val2 = new Pen(info.Colors.ExpandLineColor, 1f);
			try
			{
				info.Graphics.DrawPath(val2, trianglePath);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		trianglePath.Dispose();
	}

	protected eTriangleDirection GetDirection(SplitterPaintInfo info)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		if (((int)info.Dock == 1 && info.Expanded) || ((int)info.Dock == 2 && !info.Expanded))
		{
			return eTriangleDirection.Top;
		}
		if (((int)info.Dock == 1 && !info.Expanded) || ((int)info.Dock == 2 && info.Expanded))
		{
			return eTriangleDirection.Bottom;
		}
		if (((int)info.Dock == 3 && info.Expanded) || ((int)info.Dock == 4 && !info.Expanded))
		{
			return eTriangleDirection.Left;
		}
		return eTriangleDirection.Right;
	}

	protected LinearGradientBrush CreateLinearGradientBrush(Rectangle r, Color color1, Color color2, float gradientAngle)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		return new LinearGradientBrush(new Rectangle(r.X - 1, r.Y - 1, r.Width + 1, r.Height + 1), color1, color2, gradientAngle);
	}

	protected virtual void PaintBackground(SplitterPaintInfo info)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		int num = info.Colors.BackColorGradientAngle;
		Rectangle displayRectangle = info.DisplayRectangle;
		if ((int)info.Dock == 1 || (int)info.Dock == 2)
		{
			num += 90;
		}
		if (info.Colors.BackColor2.IsEmpty)
		{
			SolidBrush val = new SolidBrush(info.Colors.BackColor);
			try
			{
				info.Graphics.FillRectangle((Brush)(object)val, displayRectangle);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		LinearGradientBrush val2 = CreateLinearGradientBrush(displayRectangle, info.Colors.BackColor, info.Colors.BackColor2, num);
		try
		{
			info.Graphics.FillRectangle((Brush)(object)val2, displayRectangle);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
