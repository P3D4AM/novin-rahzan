using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win;

public class StiButtonHelper
{
	public static int ButtonWidth { get; set; } = StiScale2.I6;


	public static int ButtonHeight { get; set; } = StiScale2.I3;


	public static double ScaleFactor => StiScale2.Factor;

	public static void DrawUpArrow(Graphics g, Rectangle rect, bool enabled, Color? color = null, Color? disabledColor = null)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			Rectangle rectangle = new Rectangle(rect.X + (rect.Width - ButtonWidth) / 2, rect.Y + (rect.Height - ButtonHeight) / 2, ButtonWidth, ButtonHeight);
			if (!color.HasValue)
			{
				color = Color.Black;
			}
			if (!disabledColor.HasValue)
			{
				disabledColor = Color.Gray;
			}
			float num = ((ScaleFactor >= 2.0) ? 1.6f : 1.4f);
			Color color2 = (enabled ? color.GetValueOrDefault() : disabledColor.GetValueOrDefault());
			Pen val = new Pen(color2, num);
			try
			{
				g.DrawLines(val, new Point[3]
				{
					new Point(rectangle.X, rectangle.Bottom),
					new Point(rectangle.X + rectangle.Width / 2, rectangle.Y),
					new Point(rectangle.Right, rectangle.Bottom)
				});
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	public static void DrawDownArrow(Graphics g, Rectangle rect, bool enabled, Color? color = null, Color? disabledColor = null)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			Rectangle rectangle = new Rectangle(rect.X + (rect.Width - ButtonWidth) / 2, rect.Y + (rect.Height - ButtonHeight) / 2, ButtonWidth, ButtonHeight);
			if (!color.HasValue)
			{
				color = Color.Black;
			}
			if (!disabledColor.HasValue)
			{
				disabledColor = Color.Gray;
			}
			float num = ((ScaleFactor >= 2.0) ? 1.6f : 1.4f);
			Color color2 = (enabled ? color.GetValueOrDefault() : disabledColor.GetValueOrDefault());
			Pen val = new Pen(color2, num);
			try
			{
				g.DrawLines(val, new Point[3]
				{
					new Point(rectangle.X, rectangle.Y),
					new Point(rectangle.X + rectangle.Width / 2, rectangle.Bottom),
					new Point(rectangle.Right, rectangle.Y)
				});
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	public static void DrawRightArrow(Graphics g, Rectangle rect, bool enabled, Color? color = null, Color? disabledColor = null)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			Rectangle rectangle = new Rectangle(rect.X + (rect.Width - ButtonHeight) / 2, rect.Y + (rect.Height - ButtonWidth) / 2, ButtonHeight, ButtonWidth);
			if (!color.HasValue)
			{
				color = Color.Black;
			}
			if (!disabledColor.HasValue)
			{
				disabledColor = Color.Gray;
			}
			float num = ((ScaleFactor >= 2.0) ? 1.6f : 1.4f);
			Color color2 = (enabled ? color.GetValueOrDefault() : disabledColor.GetValueOrDefault());
			Pen val = new Pen(color2, num);
			try
			{
				g.DrawLines(val, new Point[3]
				{
					new Point(rectangle.X, rectangle.Y),
					new Point(rectangle.Right, rectangle.Y + rectangle.Height / 2),
					new Point(rectangle.X, rectangle.Bottom)
				});
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	public static Bitmap GetDropDownArrowBitmapLeftAligned(int width, int height, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Bitmap val = new Bitmap(width, height, (PixelFormat)2498570);
		val.MakeTransparent(Color.Transparent);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.Clear(Color.Transparent);
			DrawDownArrow(val2, new Rectangle(0, 0, ButtonWidth, ((Image)val).Height), enabled: true, color);
			return val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
