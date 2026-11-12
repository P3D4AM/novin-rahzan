using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Stimulsoft.Controls;

internal sealed class StiShadow
{
	internal const int ShadowSize = 4;

	internal const int ShadowPower = 64;

	private static Bitmap edgeShadow;

	private static Bitmap leftShadow;

	private static Bitmap topShadow;

	internal static void DrawEdgeShadow(Graphics g, Rectangle rect)
	{
		g.DrawImageUnscaled((Image)(object)edgeShadow, rect.Right - 4, rect.Bottom - 4);
	}

	internal static void DrawLeftShadow(Graphics g, Rectangle rect)
	{
		g.DrawImageUnscaled((Image)(object)leftShadow, rect.Left + 2, rect.Bottom - 4);
	}

	internal static void DrawTopShadow(Graphics g, Rectangle rect)
	{
		g.DrawImageUnscaled((Image)(object)topShadow, rect.Right - 4, rect.Top + 2);
	}

	internal static void DrawRightShadow(Graphics g, Rectangle rect)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.Right - 4, rect.Y + 4 + 2, 4, rect.Height - 8 - 2);
		if (CannotDrawShadowPiece(rectangle))
		{
			return;
		}
		Color color = Color.FromArgb(64, 0, 0, 0);
		Color color2 = Color.FromArgb(0, 0, 0, 0);
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

	internal static void DrawBottomShadow(Graphics g, Rectangle rect)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.X + 4 + 2, rect.Bottom - 4, rect.Width - 8 - 2, 4);
		if (CannotDrawShadowPiece(rectangle))
		{
			return;
		}
		Color color = Color.FromArgb(64, 0, 0, 0);
		Color color2 = Color.FromArgb(0, 0, 0, 0);
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

	private static bool CannotDrawShadowPiece(Rectangle shadowPieceBounds)
	{
		if (shadowPieceBounds.Width > 0)
		{
			return shadowPieceBounds.Height <= 0;
		}
		return true;
	}

	internal static void DrawShadow(Graphics g, Rectangle rect)
	{
		DrawEdgeShadow(g, rect);
		DrawLeftShadow(g, rect);
		DrawTopShadow(g, rect);
		DrawRightShadow(g, rect);
		DrawBottomShadow(g, rect);
	}

	private static Bitmap GetEdgeShadow()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Bitmap val = new Bitmap(4, 4, (PixelFormat)2498570);
		for (int i = 0; i < 4; i++)
		{
			int num = 64 * (5 - i) / 5;
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
