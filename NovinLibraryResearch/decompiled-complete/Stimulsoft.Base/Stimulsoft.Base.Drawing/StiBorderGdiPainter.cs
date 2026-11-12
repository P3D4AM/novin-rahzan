using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public static class StiBorderGdiPainter
{
	public static void PaintBorder(Graphics g, Rectangle rect, StiSimpleBorder border, Color backColor, bool isTooLightBorderAdoptation = false)
	{
		PaintBorder(g, rect, border, backColor, 1f, isTooLightBorderAdoptation);
	}

	public static void PaintBorder(Graphics g, RectangleF rect, StiSimpleBorder border, Color backColor)
	{
		PaintBorder(g, rect, border, backColor, 1f);
	}

	public static void PaintBorder(Graphics g, RectangleF rect, StiSimpleBorder border, Color backColor, float zoom, bool isTooLightBorderAdoptation = false)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (border == null || border.IsDefault)
		{
			return;
		}
		Color color = Color.White;
		if (border.Style == StiPenStyle.Double)
		{
			color = backColor;
			if (color == Color.Transparent)
			{
				color = Color.White;
			}
			if (isTooLightBorderAdoptation && StiColorUtils.IsItTooLight(backColor))
			{
				color = Color.Transparent;
			}
		}
		Pen val = null;
		Pen val2 = new Pen(border.Color);
		try
		{
			val2.Alignment = (PenAlignment)1;
			if (border.Style == StiPenStyle.Double)
			{
				val = new Pen(color);
			}
			val2.DashStyle = StiPenUtils.GetPenStyle(border.Style);
			if (border.Style == StiPenStyle.None)
			{
				return;
			}
			val2.Width = (int)(border.Size * (double)zoom);
			val2.StartCap = (LineCap)1;
			val2.EndCap = (LineCap)1;
			if (val2.Width > 1f && border.Style != StiPenStyle.Double)
			{
				rect.Width++;
				rect.Height++;
			}
			RectangleF rectangleF = rect;
			RectangleF rectangleF2 = rect;
			if (border.Style == StiPenStyle.Double)
			{
				rect.Inflate(-1f, -1f);
				rectangleF.Inflate(-2f, -2f);
				val2.Width = 1f;
			}
			if (border.IsAllBorderSidesPresent)
			{
				if (border.Style == StiPenStyle.Double)
				{
					g.DrawRectangle(val, rect.X, rect.Y, rect.Width, rect.Height);
					g.DrawRectangle(val2, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
					g.DrawRectangle(val2, rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
				}
				else
				{
					g.DrawRectangle(val2, rect.X, rect.Y, rect.Width, rect.Height);
				}
			}
			else
			{
				float num = val2.Width / 2f;
				if (num < 1f)
				{
					num = 0f;
				}
				if (border.IsTopBorderSidePresent)
				{
					float left = rectangleF.Left;
					float right = rectangleF.Right;
					if (!border.IsLeftBorderSidePresent)
					{
						left = rectangleF2.Left;
					}
					if (!border.IsRightBorderSidePresent)
					{
						right = rectangleF2.Right;
					}
					if (border.Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Left, rect.Top, rect.Right, rect.Top);
						g.DrawLine(val2, left, rectangleF.Top, right, rectangleF.Top);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Top, rectangleF2.Right, rectangleF2.Top);
					}
					else
					{
						g.DrawLine(val2, rect.Left + num, rect.Top + num, rect.Right - num, rect.Top + num);
					}
				}
				if (border.IsLeftBorderSidePresent)
				{
					float top = rectangleF.Top;
					float bottom = rectangleF.Bottom;
					if (!border.IsTopBorderSidePresent)
					{
						top = rectangleF2.Top;
					}
					if (!border.IsBottomBorderSidePresent)
					{
						bottom = rectangleF2.Bottom;
					}
					if (border.Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Left, rect.Top, rect.Left, rect.Bottom);
						g.DrawLine(val2, rectangleF.Left, top, rectangleF.Left, bottom);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Top, rectangleF2.Left, rectangleF2.Bottom);
					}
					else
					{
						g.DrawLine(val2, rect.Left + num, rect.Top + num, rect.Left + num, rect.Bottom - num);
					}
				}
				if (border.IsBottomBorderSidePresent)
				{
					float left = rectangleF.Left;
					float right = rectangleF.Right;
					if (!border.IsLeftBorderSidePresent)
					{
						left = rectangleF2.Left;
					}
					if (!border.IsRightBorderSidePresent)
					{
						right = rectangleF2.Right;
					}
					if (border.Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
						g.DrawLine(val2, left, rectangleF.Bottom, right, rectangleF.Bottom);
						g.DrawLine(val2, rectangleF2.Left, rectangleF2.Bottom, rectangleF2.Right, rectangleF2.Bottom);
					}
					else
					{
						g.DrawLine(val2, rect.Left + num, rect.Bottom - num, rect.Right - num, rect.Bottom - num);
					}
				}
				if (border.IsRightBorderSidePresent)
				{
					float top = rectangleF.Top;
					float bottom = rectangleF.Bottom;
					if (!border.IsTopBorderSidePresent)
					{
						top = rectangleF2.Top;
					}
					if (!border.IsBottomBorderSidePresent)
					{
						bottom = rectangleF2.Bottom;
					}
					if (border.Style == StiPenStyle.Double)
					{
						g.DrawLine(val, rect.Right, rect.Top, rect.Right, rect.Bottom);
						g.DrawLine(val2, rectangleF.Right, top, rectangleF.Right, bottom);
						g.DrawLine(val2, rectangleF2.Right, rectangleF2.Top, rectangleF2.Right, rectangleF2.Bottom);
					}
					else
					{
						g.DrawLine(val2, rect.Right - num, rect.Top + num, rect.Right - num, rect.Bottom - num);
					}
				}
			}
			if (val != null)
			{
				val.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
