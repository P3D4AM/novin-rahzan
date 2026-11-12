using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Drawing;

[JsonObject]
public sealed class StiShadow
{
	internal const int ShadowSize = 4;

	internal const int ShadowPower = 99;

	private static Bitmap edgeShadow;

	private static Bitmap leftShadow;

	private static Bitmap topShadow;

	public static void DrawCachedShadow(Graphics g, RectangleF rect, StiShadowSides sides, bool isSimple)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (isSimple)
		{
			GraphicsPath val = new GraphicsPath();
			try
			{
				SolidBrush val2 = new SolidBrush(Color.FromArgb(50, Color.Black));
				try
				{
					if ((sides & StiShadowSides.Top) > (StiShadowSides)0)
					{
						val.AddRectangle(new RectangleF(rect.Right, rect.Y + 4f, 4f, 4f));
					}
					if ((sides & StiShadowSides.Right) > (StiShadowSides)0)
					{
						val.AddRectangle(new RectangleF(rect.Right, rect.Y + 8f, 4f, rect.Height - 8f));
					}
					if ((sides & StiShadowSides.Edge) > (StiShadowSides)0)
					{
						val.AddRectangle(new RectangleF(rect.Right, rect.Bottom, 4f, 4f));
					}
					if ((sides & StiShadowSides.Bottom) > (StiShadowSides)0)
					{
						val.AddRectangle(new RectangleF(rect.X + 8f, rect.Bottom, rect.Width - 8f, 4f));
					}
					if ((sides & StiShadowSides.Left) > (StiShadowSides)0)
					{
						val.AddRectangle(new RectangleF(rect.X + 4f, rect.Bottom, 4f, 4f));
					}
					g.FillPath((Brush)(object)val2, val);
					return;
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
		DrawCachedShadow(g, ConvertRect(rect), sides);
	}

	public static void DrawCachedShadow(Graphics g, Rectangle rect, StiShadowSides sides)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Rectangle rect2 = rect;
		if (rect.Width <= 0)
		{
			rect.Width = 1;
		}
		if (rect.Height <= 0)
		{
			rect.Height = 1;
		}
		Bitmap val = new Bitmap(rect.Width, rect.Height, (PixelFormat)2498570);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				rect2.X = -4;
				rect2.Y = -4;
				if ((sides & StiShadowSides.Top) > (StiShadowSides)0)
				{
					DrawTopShadow(val2, rect2);
				}
				if ((sides & StiShadowSides.Right) > (StiShadowSides)0)
				{
					DrawRightShadow(val2, rect2);
				}
				if ((sides & StiShadowSides.Edge) > (StiShadowSides)0)
				{
					DrawEdgeShadow(val2, rect2);
				}
				if ((sides & StiShadowSides.Bottom) > (StiShadowSides)0)
				{
					DrawBottomShadow(val2, rect2);
				}
				if ((sides & StiShadowSides.Left) > (StiShadowSides)0)
				{
					DrawLeftShadow(val2, rect2);
				}
				g.DrawImageUnscaled((Image)(object)val, rect.X + 4, rect.Y + 4);
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

	public static void DrawEdgeShadow(Graphics g, Rectangle rect)
	{
		lock (edgeShadow)
		{
			g.DrawImageUnscaled((Image)(object)edgeShadow, rect.Right, rect.Bottom, rect.Width * 3, rect.Height * 3);
		}
	}

	public static void DrawLeftShadow(Graphics g, Rectangle rect)
	{
		lock (leftShadow)
		{
			g.DrawImageUnscaled((Image)(object)leftShadow, rect.Left + 4, rect.Bottom);
		}
	}

	public static void DrawTopShadow(Graphics g, Rectangle rect)
	{
		lock (topShadow)
		{
			g.DrawImageUnscaled((Image)(object)topShadow, rect.Right, rect.Top + 4);
		}
	}

	public static void DrawRightShadow(Graphics g, Rectangle rect)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		Color color = Color.FromArgb(99, 0, 0, 0);
		Color color2 = Color.FromArgb(0, 0, 0, 0);
		Rectangle rectangle = new Rectangle(rect.Right - 1, rect.Y + 8, 5, rect.Height - 8);
		if (rectangle.Width == 0 || rectangle.Height == 0)
		{
			return;
		}
		LinearGradientBrush val = new LinearGradientBrush(rectangle, color, color2, 0f);
		try
		{
			g.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawBottomShadow(Graphics g, Rectangle rect)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		Color color = Color.FromArgb(99, 0, 0, 0);
		Color color2 = Color.FromArgb(0, 0, 0, 0);
		Rectangle rectangle = new Rectangle(rect.X + 8, rect.Bottom, rect.Width - 8, 4);
		if (rectangle.Width == 0 || rectangle.Height == 0)
		{
			return;
		}
		LinearGradientBrush val = new LinearGradientBrush(rectangle, color, color2, 90f);
		try
		{
			g.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawShadow(Graphics g, Rectangle rect)
	{
		DrawEdgeShadow(g, rect);
		DrawLeftShadow(g, rect);
		DrawTopShadow(g, rect);
		DrawRightShadow(g, rect);
		DrawBottomShadow(g, rect);
	}

	private static Rectangle ConvertRect(RectangleF rect)
	{
		return new Rectangle((int)Math.Round(rect.X), (int)Math.Round(rect.Y), (int)Math.Round(rect.Width), (int)Math.Round(rect.Height));
	}

	private static Bitmap GetEdgeShadow()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Bitmap val = new Bitmap(4, 4, (PixelFormat)2498570);
		for (int i = 0; i < 4; i++)
		{
			int num = 99 * (5 - i) / 5;
			for (int j = 0; j < 4; j++)
			{
				val.SetPixel(j, i, Color.FromArgb(num * (4 - j) / 5, 0, 0, 0));
			}
		}
		return val;
	}

	static StiShadow()
	{
		edgeShadow = GetEdgeShadow();
		object obj = ((Image)edgeShadow).Clone();
		leftShadow = (Bitmap)((obj is Bitmap) ? obj : null);
		object obj2 = ((Image)edgeShadow).Clone();
		topShadow = (Bitmap)((obj2 is Bitmap) ? obj2 : null);
		((Image)leftShadow).RotateFlip((RotateFlipType)4);
		((Image)topShadow).RotateFlip((RotateFlipType)6);
	}
}
