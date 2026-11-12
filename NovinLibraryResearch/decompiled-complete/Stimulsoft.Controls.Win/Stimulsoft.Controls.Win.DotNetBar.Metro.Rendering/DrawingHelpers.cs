using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal static class DrawingHelpers
{
	public static void DrawBorder(Graphics g, RectangleF bounds, Thickness borderThickness, BorderColors borderColor)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		if (borderColor.IsEmpty || borderThickness.IsZero)
		{
			return;
		}
		bounds.Width -= (float)borderThickness.Right / 2f;
		bounds.Height -= (float)borderThickness.Bottom / 2f;
		if (borderThickness.Left > 0.0 && !borderColor.Left.IsEmpty)
		{
			Pen val = new Pen(borderColor.Left, (float)borderThickness.Left);
			try
			{
				val.Alignment = (PenAlignment)1;
				g.DrawLine(val, bounds.X, bounds.Y, bounds.X, bounds.Bottom);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (borderThickness.Top > 0.0 && !borderColor.Top.IsEmpty)
		{
			Pen val2 = new Pen(borderColor.Top, (float)borderThickness.Top);
			try
			{
				val2.Alignment = (PenAlignment)1;
				g.DrawLine(val2, bounds.X, bounds.Y, bounds.Right, bounds.Y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (borderThickness.Right > 0.0 && !borderColor.Right.IsEmpty)
		{
			Pen val3 = new Pen(borderColor.Right, (float)borderThickness.Right);
			try
			{
				val3.Alignment = (PenAlignment)1;
				g.DrawLine(val3, bounds.Right, bounds.Y, bounds.Right, bounds.Bottom + (float)borderThickness.Bottom / 2f);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (!(borderThickness.Bottom > 0.0) || borderColor.Bottom.IsEmpty)
		{
			return;
		}
		Pen val4 = new Pen(borderColor.Bottom, (float)borderThickness.Bottom);
		try
		{
			val4.Alignment = (PenAlignment)1;
			g.DrawLine(val4, bounds.X, bounds.Bottom, bounds.Right, bounds.Bottom);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	public static void DrawBackground(Graphics g, RectangleF bounds, string color)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (string.IsNullOrEmpty(color))
		{
			return;
		}
		SolidBrush val = new SolidBrush(ColorScheme.GetColor(color));
		try
		{
			g.FillRectangle((Brush)(object)val, bounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static RectangleF Deflate(RectangleF bounds, Thickness borderThickness)
	{
		if (borderThickness.IsZero)
		{
			return bounds;
		}
		bounds.X += (float)borderThickness.Left;
		bounds.Width -= (float)(borderThickness.Left + borderThickness.Right);
		bounds.Y += (float)borderThickness.Top;
		bounds.Height -= (float)(borderThickness.Top + borderThickness.Bottom);
		return bounds;
	}
}
