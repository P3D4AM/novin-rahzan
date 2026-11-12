using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public static class StiRoundedBackgroundGdiPainter
{
	public static void PaintBackground(Graphics g, RectangleF rect, StiSimpleBorder border, StiCornerRadius cornerRadius, Color? backColor, StiButtonShapeType shapeType, bool isEnabled, float scale)
	{
		StiSolidBrush brush = new StiSolidBrush(backColor.GetValueOrDefault(Color.Transparent));
		PaintBackground(g, rect, border, cornerRadius, brush, shapeType, isEnabled, scale);
	}

	public static void PaintBackground(Graphics g, RectangleF rect, StiSimpleBorder border, StiCornerRadius cornerRadius, StiBrush brush, StiButtonShapeType shapeType, bool isEnabled, float scale)
	{
		if (shapeType == StiButtonShapeType.Circle)
		{
			PaintCircledBackground(g, rect, brush, isEnabled);
		}
		else if (cornerRadius == null || cornerRadius.IsEmpty)
		{
			PaintBackground(g, rect, border, brush, isEnabled);
		}
		else
		{
			PaintRoundedBackground(g, rect, border, cornerRadius, brush, isEnabled, scale);
		}
	}

	public static void PaintRoundedBackground(Graphics g, RectangleF rect, StiSimpleBorder border, StiCornerRadius cornerRadius, StiBrush brush, bool isEnabled, float scale)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			rect = ShiftRectangleFromBorder(rect, border);
			if (!isEnabled)
			{
				brush = new StiSolidBrush(Color.FromArgb(230, 230, 230));
			}
			Brush brush2 = StiBrush.GetBrush(brush, rect);
			try
			{
				GraphicsPath val = StiRoundedRectangleCreator.Create(rect, cornerRadius, scale);
				try
				{
					g.FillPath(brush2, val);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)brush2)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	public static void PaintBackground(Graphics g, RectangleF rect, StiSimpleBorder border, StiBrush brush, bool isEnabled)
	{
		rect = ShiftRectangleFromBorder(rect, border);
		if (!isEnabled)
		{
			brush = new StiSolidBrush(Color.FromArgb(230, 230, 230));
		}
		StiDrawing.FillRectangle(g, brush, rect);
	}

	public static void PaintCircledBackground(Graphics g, RectangleF rect, StiBrush brush, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			rect = StiRoundedBorderGdiPainter.FitRectangleInCircle(rect);
			if (!isEnabled)
			{
				brush = new StiSolidBrush(Color.FromArgb(230, 230, 230));
			}
			StiDrawing.FillEllipse(g, brush, rect);
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private static RectangleF ShiftRectangleFromBorder(RectangleF rect, StiSimpleBorder border)
	{
		if (border == null || border.Size <= 1.0)
		{
			return rect;
		}
		float num = (float)border.Size / 2f;
		StiBorderSides side = border.Side;
		if ((side & StiBorderSides.Left) > StiBorderSides.None)
		{
			rect.X += num;
			rect.Width -= num;
		}
		if ((side & StiBorderSides.Top) > StiBorderSides.None)
		{
			rect.Y += num;
			rect.Height -= num;
		}
		if ((side & StiBorderSides.Right) > StiBorderSides.None)
		{
			rect.Width -= num;
		}
		if ((side & StiBorderSides.Bottom) > StiBorderSides.None)
		{
			rect.Height -= num;
		}
		return rect;
	}
}
