using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class UIGraphics
{
	private UIGraphics()
	{
	}

	public static GraphicsPath GetTrianglePath(Point p, int size, eTriangleDirection direction)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		switch (direction)
		{
		case eTriangleDirection.Left:
			p.X--;
			val.AddLine(p.X + size / 2, p.Y, p.X + size / 2, p.Y + size);
			val.AddLine(p.X, p.Y + size / 2, p.X + size / 2, p.Y);
			val.CloseAllFigures();
			break;
		case eTriangleDirection.Right:
			val.AddLine(p.X, p.Y, p.X, p.Y + size);
			val.AddLine(p.X + size / 2, p.Y + size / 2, p.X, p.Y);
			val.CloseAllFigures();
			break;
		case eTriangleDirection.Top:
		{
			int num3 = (int)Math.Ceiling((float)p.Y + (float)size / 2f);
			int num4 = (int)Math.Ceiling((float)p.X + (float)size / 2f);
			val.AddLine(p.X, num3, p.X + size, num3);
			val.AddLine(num4, p.Y, p.X, num3);
			val.CloseAllFigures();
			break;
		}
		case eTriangleDirection.Bottom:
		{
			int num = (int)Math.Floor((float)p.Y + (float)size / 2f);
			int num2 = (int)Math.Floor((float)p.X + (float)size / 2f);
			val.AddLine(p.X, p.Y, p.X + size - 1, p.Y);
			val.AddLine(num2, num, p.X, p.Y);
			val.CloseAllFigures();
			break;
		}
		}
		return val;
	}

	public static Image CreateExpandButtonImage(bool collapse, Color color, bool verticalCollapse)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, (PixelFormat)137224);
		val.MakeTransparent();
		Image val2 = (Image)(object)val;
		Graphics val3 = Graphics.FromImage(val2);
		val3.SmoothingMode = (SmoothingMode)4;
		if (collapse)
		{
			if (verticalCollapse)
			{
				Pen val4 = new Pen(color, 1f);
				try
				{
					val3.DrawLine(val4, 4, 7, 7, 4);
					val3.DrawLine(val4, 7, 4, 10, 7);
					val3.DrawLine(val4, 5, 7, 7, 5);
					val3.DrawLine(val4, 7, 5, 9, 7);
					val3.DrawLine(val4, 4, 10, 7, 7);
					val3.DrawLine(val4, 7, 7, 10, 10);
					val3.DrawLine(val4, 5, 10, 7, 8);
					val3.DrawLine(val4, 7, 8, 9, 10);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else
			{
				Pen val5 = new Pen(color, 1f);
				try
				{
					val3.DrawLine(val5, 7, 4, 4, 7);
					val3.DrawLine(val5, 4, 7, 7, 10);
					val3.DrawLine(val5, 7, 5, 5, 7);
					val3.DrawLine(val5, 5, 7, 7, 9);
					val3.DrawLine(val5, 10, 4, 7, 7);
					val3.DrawLine(val5, 7, 7, 10, 10);
					val3.DrawLine(val5, 10, 5, 8, 7);
					val3.DrawLine(val5, 8, 7, 10, 9);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
		}
		else if (verticalCollapse)
		{
			Pen val6 = new Pen(color, 1f);
			try
			{
				val3.DrawLine(val6, 4, 4, 7, 7);
				val3.DrawLine(val6, 7, 7, 10, 4);
				val3.DrawLine(val6, 5, 4, 7, 6);
				val3.DrawLine(val6, 7, 6, 9, 4);
				val3.DrawLine(val6, 4, 7, 7, 10);
				val3.DrawLine(val6, 7, 10, 10, 7);
				val3.DrawLine(val6, 5, 7, 7, 9);
				val3.DrawLine(val6, 7, 9, 9, 7);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		else
		{
			Pen val7 = new Pen(color, 1f);
			try
			{
				val3.DrawLine(val7, 4, 4, 7, 7);
				val3.DrawLine(val7, 7, 7, 4, 10);
				val3.DrawLine(val7, 4, 5, 6, 7);
				val3.DrawLine(val7, 6, 7, 4, 9);
				val3.DrawLine(val7, 7, 4, 10, 7);
				val3.DrawLine(val7, 10, 7, 7, 10);
				val3.DrawLine(val7, 7, 5, 9, 7);
				val3.DrawLine(val7, 9, 7, 7, 9);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		val3.Dispose();
		return val2;
	}
}
