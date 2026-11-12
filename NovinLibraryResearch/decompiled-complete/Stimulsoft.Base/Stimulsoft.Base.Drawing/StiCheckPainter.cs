using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public static class StiCheckPainter
{
	public static void PaintCheckBox(Graphics g, Rectangle rect, StiCheckState checkState, Color color)
	{
		PaintCheckBox(g, RectangleD.CreateFromRectangle(rect), checkState, color, (float)StiScale.Factor);
	}

	public static void PaintCheckBox(Graphics g, Rectangle rect, bool checkState, Color color)
	{
		PaintCheckBox(g, RectangleD.CreateFromRectangle(rect), (!checkState) ? StiCheckState.Unchecked : StiCheckState.Checked, color, (float)StiScale.Factor);
	}

	public static void PaintCheckBox(Graphics g, RectangleD rectD, StiCheckState checkState, Color color, float scale)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		int checkSize = GetCheckSize(scale);
		Rectangle rectangle = new RectangleD(rectD.X + (rectD.Width - (double)checkSize) / 2.0, rectD.Y + (rectD.Height - (double)checkSize) / 2.0, checkSize, checkSize).ToRectangle();
		Pen val = new Pen(color);
		try
		{
			g.DrawRectangle(val, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		switch (checkState)
		{
		case StiCheckState.Indeterminate:
		{
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				g.FillRectangle((Brush)(object)val3, rectangle.X + StiScale.I3, rectangle.Y + StiScale.I3, rectangle.Width - StiScale.I6 + 1, rectangle.Height - StiScale.I6 + 1);
				break;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		case StiCheckState.Checked:
		{
			rectangle.Inflate(-(int)(4f * scale), -(int)(4f * scale));
			PointF pointF = new PointF(rectangle.X, (float)rectangle.Y + (float)rectangle.Height * 0.7f);
			PointF pointF2 = new PointF((float)rectangle.X + (float)rectangle.Width * 0.5f, rectangle.Bottom);
			PointF pointF3 = new PointF(rectangle.Right, rectangle.Y);
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)4;
			Pen val2 = new Pen(color);
			try
			{
				val2.Width = 2f * scale;
				val2.StartCap = (LineCap)2;
				val2.EndCap = (LineCap)2;
				g.DrawLine(val2, pointF, pointF2);
				g.DrawLine(val2, pointF2, pointF3);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			g.SmoothingMode = smoothingMode;
			break;
		}
		}
	}

	public static int GetCheckSize(float scale)
	{
		int num = (int)(16f * scale);
		return (int)Math.Min(26f * scale, num);
	}

	public static int GetCheckSize()
	{
		return GetCheckSize((float)StiScale.Factor);
	}
}
