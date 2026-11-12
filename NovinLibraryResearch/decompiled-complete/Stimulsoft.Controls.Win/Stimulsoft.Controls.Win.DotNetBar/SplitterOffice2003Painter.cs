using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SplitterOffice2003Painter : SplitterPainter
{
	public override void Paint(SplitterPaintInfo info)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		int num = 8;
		Rectangle displayRectangle = info.DisplayRectangle;
		Graphics graphics = info.Graphics;
		SplitterColors colors = info.Colors;
		PaintBackground(info);
		if ((int)info.Dock == 1 || (int)info.Dock == 2)
		{
			Point point = new Point(displayRectangle.X + (displayRectangle.Width - 34) / 2 - 1, displayRectangle.Y + (displayRectangle.Height - 4) / 2);
			SolidBrush val = new SolidBrush(colors.GripLightColor);
			try
			{
				int num2 = point.X + 1;
				int num3 = point.Y + 1;
				for (int i = 0; i < 9; i++)
				{
					graphics.FillRectangle((Brush)(object)val, num2, num3, 2, 2);
					num2 += 4;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			SolidBrush val2 = new SolidBrush(colors.GripDarkColor);
			try
			{
				int num4 = point.X;
				int y = point.Y;
				for (int j = 0; j < 9; j++)
				{
					graphics.FillRectangle((Brush)(object)val2, num4, y, 2, 2);
					num4 += 4;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (info.Expandable)
			{
				Point p = new Point(displayRectangle.X + (displayRectangle.Width - 36) / 2 - base.ArrowSize - num, displayRectangle.Y + (displayRectangle.Height - base.ArrowSize / 2) / 2);
				PaintArrow(p, info);
				p.Offset(36 + num * 2 + base.ArrowSize, 0);
				PaintArrow(p, info);
			}
			return;
		}
		Point point2 = new Point(displayRectangle.X + (displayRectangle.Width - 4) / 2, displayRectangle.Y + (displayRectangle.Height - 34) / 2);
		SolidBrush val3 = new SolidBrush(colors.GripLightColor);
		try
		{
			int num5 = point2.Y;
			int num6 = point2.X + 2;
			for (int k = 0; k < 9; k++)
			{
				graphics.FillRectangle((Brush)(object)val3, num6, num5, 2, 2);
				num5 += 4;
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		SolidBrush val4 = new SolidBrush(colors.GripDarkColor);
		try
		{
			int num7 = point2.Y - 1;
			int num8 = point2.X + 1;
			for (int l = 0; l < 9; l++)
			{
				graphics.FillRectangle((Brush)(object)val4, num8, num7, 2, 2);
				num7 += 4;
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		if (info.Expandable)
		{
			Point p2 = new Point(displayRectangle.X + (displayRectangle.Width - base.ArrowSize / 2) / 2, displayRectangle.Y + (displayRectangle.Height - 36) / 2 - base.ArrowSize - num);
			PaintArrow(p2, info);
			p2.Offset(0, 36 + num * 2 + base.ArrowSize);
			PaintArrow(p2, info);
		}
	}
}
