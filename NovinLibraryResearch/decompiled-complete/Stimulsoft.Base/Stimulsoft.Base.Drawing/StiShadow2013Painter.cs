using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public static class StiShadow2013Painter
{
	public static void DrawShadow(Graphics g, Rectangle rect, int size, Color color)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle rect2 = new Rectangle(rect.Left - size, rect.Top - size, rect.Width + size * 2, rect.Height + size * 2);
		GraphicsPath roundedPath = GetRoundedPath(rect2);
		try
		{
			PathGradientBrush val = new PathGradientBrush(roundedPath);
			try
			{
				val.WrapMode = (WrapMode)4;
				ColorBlend val2 = new ColorBlend(3);
				val2.Colors = new Color[3]
				{
					Color.Transparent,
					color,
					color
				};
				val2.Positions = new float[3] { 0f, 0.01f, 1f };
				ColorBlend interpolationColors = val2;
				val.InterpolationColors = interpolationColors;
				rect.Inflate(size + 2, size + 2);
				g.FillRectangle((Brush)(object)val, rect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)roundedPath)?.Dispose();
		}
	}

	private static GraphicsPath GetRoundedPath(Rectangle rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = 5;
		val.StartFigure();
		val.AddLine(rect.X + num, rect.Y, rect.Right - num, rect.Y);
		val.AddArc(rect.Right - 2 * num, rect.Y, 2 * num, 2 * num, -90f, 90f);
		val.AddLine(rect.Right, rect.Y + num, rect.Right, rect.Bottom - num);
		val.AddArc(rect.Right - 2 * num, rect.Bottom - 2 * num, 2 * num, 2 * num, 0f, 90f);
		val.AddLine(rect.Right - num, rect.Bottom, rect.X + num, rect.Bottom);
		val.AddArc(rect.X, rect.Bottom - 2 * num, 2 * num, 2 * num, 90f, 90f);
		val.AddLine(rect.X, rect.Bottom - num, rect.X, rect.Y + num);
		val.AddArc(rect.X, rect.Y, 2 * num, 2 * num, 180f, 90f);
		val.CloseFigure();
		return val;
	}
}
