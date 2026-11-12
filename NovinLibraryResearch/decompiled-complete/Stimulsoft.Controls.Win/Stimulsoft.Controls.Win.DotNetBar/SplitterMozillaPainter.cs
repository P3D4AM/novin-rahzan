using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SplitterMozillaPainter : SplitterPainter
{
	public override void Paint(SplitterPaintInfo info)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		int num = 42;
		int num2 = 8;
		Rectangle displayRectangle = info.DisplayRectangle;
		Graphics graphics = info.Graphics;
		PaintBackground(info);
		if ((int)info.Dock == 1 || (int)info.Dock == 2)
		{
			if (info.DisplayRectangle.Width - base.ArrowSize * 2 < num)
			{
				num = info.DisplayRectangle.Width - base.ArrowSize * 2;
				if (num < 0)
				{
					num = info.DisplayRectangle.Width;
				}
			}
			Point point = new Point(displayRectangle.X + (displayRectangle.Width - num) / 2, displayRectangle.Y + (displayRectangle.Height - 5) / 2);
			SolidBrush val = new SolidBrush(info.Colors.GripLightColor);
			SolidBrush val2 = new SolidBrush(info.Colors.GripDarkColor);
			for (int i = point.X; i < point.X + num; i += 3)
			{
				graphics.FillRectangle((Brush)(object)val, i, point.Y + 3, 1, 1);
				graphics.FillRectangle((Brush)(object)val2, i + 1, point.Y + 4, 1, 1);
				graphics.FillRectangle((Brush)(object)val, i + 1, point.Y, 1, 1);
				graphics.FillRectangle((Brush)(object)val2, i + 2, point.Y + 1, 1, 1);
			}
			((Brush)val).Dispose();
			((Brush)val2).Dispose();
			if (info.Expandable)
			{
				Point p = new Point(displayRectangle.X + (displayRectangle.Width - num) / 2 - base.ArrowSize - num2, displayRectangle.Y + (displayRectangle.Height - base.ArrowSize / 2) / 2);
				PaintArrow(p, info);
				p.Offset(num + num2 * 2 + base.ArrowSize, 0);
				PaintArrow(p, info);
			}
			return;
		}
		if (info.DisplayRectangle.Height - base.ArrowSize * 2 < num)
		{
			num = info.DisplayRectangle.Height - base.ArrowSize * 2;
			if (num < 0)
			{
				num = info.DisplayRectangle.Height;
			}
		}
		Point point2 = new Point(displayRectangle.X + (displayRectangle.Width - 5) / 2, displayRectangle.Y + (displayRectangle.Height - num) / 2);
		SolidBrush val3 = new SolidBrush(info.Colors.GripLightColor);
		SolidBrush val4 = new SolidBrush(info.Colors.GripDarkColor);
		for (int j = point2.Y; j < point2.Y + num; j += 3)
		{
			graphics.FillRectangle((Brush)(object)val3, point2.X + 3, j, 1, 1);
			graphics.FillRectangle((Brush)(object)val4, point2.X + 4, j + 1, 1, 1);
			graphics.FillRectangle((Brush)(object)val3, point2.X, j + 1, 1, 1);
			graphics.FillRectangle((Brush)(object)val4, point2.X + 1, j + 2, 1, 1);
		}
		((Brush)val3).Dispose();
		((Brush)val4).Dispose();
		if (info.Expandable)
		{
			Point p2 = new Point(displayRectangle.X + (displayRectangle.Width - base.ArrowSize / 2) / 2, displayRectangle.Y + (displayRectangle.Height - num) / 2 - base.ArrowSize - num2);
			PaintArrow(p2, info);
			p2.Offset(0, num + num2 * 2 + base.ArrowSize);
			PaintArrow(p2, info);
		}
	}
}
