using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Report.Viewer;

internal class StiViewerHelper
{
	internal static void DrawPageBorder(Graphics g, Rectangle rect, bool active)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (active)
		{
			if (StiOptions.Viewer.Windows.ActivePageBorderColor is Color)
			{
				DrawOfficePageBorder(g, rect);
			}
			else
			{
				DrawOffice2013PageBorder(g, rect);
			}
			return;
		}
		Color color = Color.FromArgb(255, 198, 198, 198);
		Pen val = new Pen(Color.White);
		try
		{
			Pen val2 = new Pen(color);
			try
			{
				g.DrawRectangle(val2, rect);
				rect.X++;
				rect.Y++;
				rect.Width -= 2;
				rect.Height -= 2;
				g.DrawRectangle(val, rect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static void DrawOfficePageBorder(Graphics g, Rectangle rect)
	{
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		Color color = Color.FromArgb(255, 207, 45);
		Color color2 = Color.FromArgb(254, 235, 162);
		Color color3 = Color.FromArgb(247, 152, 52);
		Rectangle rectangle = rect;
		Rectangle rectangle2 = rect;
		rectangle.Height /= 2;
		rectangle2.Y = rectangle.Bottom;
		rectangle2.Height = rect.Bottom - rectangle2.Y;
		rectangle.Width++;
		rectangle2.Width++;
		rectangle2.Height++;
		Rectangle rectangle3 = rectangle;
		Rectangle rectangle4 = rectangle;
		Rectangle rectangle5 = rectangle2;
		Rectangle rectangle6 = rectangle2;
		rectangle3.Width = 2;
		rectangle4.X = rectangle.Right - 2;
		rectangle4.Width = 2;
		rectangle5.Width = 2;
		rectangle6.X = rectangle2.Right - 2;
		rectangle6.Width = 2;
		Rectangle rectangle7 = rect;
		rectangle7.Height = 2;
		Rectangle rectangle8 = rect;
		rectangle8.Y = rect.Bottom - 1;
		rectangle8.Height = 2;
		LinearGradientBrush val = new LinearGradientBrush(new Rectangle(rectangle.X, rectangle.Y - 1, rectangle.Width, rectangle.Height + 2), color, color2, 90f);
		try
		{
			LinearGradientBrush val2 = new LinearGradientBrush(new Rectangle(rectangle2.X, rectangle2.Y - 1, rectangle2.Width, rectangle2.Height + 2), color2, color3, 90f);
			try
			{
				SolidBrush val3 = new SolidBrush(color);
				try
				{
					SolidBrush val4 = new SolidBrush(color3);
					try
					{
						g.FillRectangle((Brush)(object)val, rectangle3);
						g.FillRectangle((Brush)(object)val, rectangle4);
						g.FillRectangle((Brush)(object)val2, rectangle5);
						g.FillRectangle((Brush)(object)val2, rectangle6);
						g.FillRectangle((Brush)(object)val3, rectangle7);
						g.FillRectangle((Brush)(object)val4, rectangle8);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static Rectangle DrawOffice2013PageBorder(Graphics g, Rectangle rect)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		rect.X++;
		rect.Y++;
		rect.Width--;
		rect.Height--;
		Pen val = new Pen(Color.FromArgb(255, 43, 87, 154), 2f);
		try
		{
			g.DrawRectangle(val, rect);
			return rect;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
