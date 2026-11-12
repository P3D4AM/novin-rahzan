using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ShadowPainter
{
	private static Bitmap _ShadowTemplate = null;

	private static Bitmap[] _ShadowParts = null;

	private static int LastUsedShadowVersion = 1;

	private static GraphicsPath GetPath(Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLine(r.Left + 1, r.Y, r.Right - 1, r.Y);
		val.AddLine(r.Right - 1, r.Y, r.Right - 1, r.Y + 1);
		val.AddLine(r.Right - 1, r.Y + 1, r.Right, r.Y + 1);
		val.AddLine(r.Right, r.Y + 1, r.Right, r.Bottom - 1);
		val.AddLine(r.Right, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
		val.AddLine(r.Right - 1, r.Bottom - 1, r.Right - 1, r.Bottom);
		val.AddLine(r.Right - 1, r.Bottom, r.Left + 1, r.Bottom);
		val.AddLine(r.Left + 1, r.Bottom, r.Left + 1, r.Bottom - 1);
		val.AddLine(r.Left + 1, r.Bottom - 1, r.Left, r.Bottom - 1);
		val.AddLine(r.Left, r.Bottom - 1, r.Left, r.Top + 1);
		return val;
	}

	public static void Paint(ShadowPaintInfo info)
	{
		Paint(info, 0);
	}

	public static void Paint(ShadowPaintInfo info, int alphaOffset)
	{
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		Graphics graphics = info.Graphics;
		Region clip = graphics.Clip;
		if (info.ClipRectangle.IsEmpty)
		{
			graphics.SetClip(info.Rectangle, (CombineMode)4);
		}
		else
		{
			graphics.SetClip(info.ClipRectangle, (CombineMode)4);
		}
		Color[] array = new Color[5]
		{
			Color.FromArgb(Math.Max(0, 14 - alphaOffset), Color.Black),
			Color.FromArgb(Math.Max(0, 43 - alphaOffset), Color.Black),
			Color.FromArgb(Math.Max(0, 84 - alphaOffset), Color.Black),
			Color.FromArgb(Math.Max(0, 113 - alphaOffset), Color.Black),
			Color.FromArgb(Math.Max(0, 128 - alphaOffset), Color.Black)
		};
		Rectangle rectangle = info.Rectangle;
		if (info.IsSquare)
		{
			rectangle.Inflate(info.Size, info.Size);
			rectangle.Width += info.Size;
		}
		else
		{
			rectangle.Width--;
			rectangle.Height--;
			int num = info.Size / 2;
			rectangle.Offset(num, num);
			rectangle.Width += info.Size - num;
			rectangle.Height += info.Size - num;
		}
		for (int i = 0; i < info.Size; i++)
		{
			Pen val = new Pen(array[i], 1f);
			try
			{
				GraphicsPath path = GetPath(rectangle);
				try
				{
					graphics.DrawPath(val, path);
				}
				finally
				{
					((IDisposable)path)?.Dispose();
				}
				rectangle.Inflate(-1, -1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		graphics.Clip = clip;
	}

	public static void Paint2(ShadowPaintInfo info)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		if (info.Size <= 2)
		{
			return;
		}
		Graphics graphics = info.Graphics;
		Color color = Color.FromArgb(128, Color.Black);
		Rectangle rectangle = info.Rectangle;
		rectangle.Offset(info.Size - 1, info.Size - 1);
		Bitmap val = new Bitmap(info.Size, info.Size);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				GraphicsPath val3 = new GraphicsPath();
				try
				{
					val3.AddEllipse(0, 0, info.Size * 2, info.Size * 2);
					PathGradientBrush val4 = new PathGradientBrush(val3);
					try
					{
						val4.CenterColor = color;
						val4.SurroundColors = new Color[1] { Color.Transparent };
						val2.FillRectangle((Brush)(object)val4, new Rectangle(0, 0, info.Size, info.Size));
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
			graphics.DrawImage((Image)(object)val, rectangle.X, rectangle.Y);
			((Image)val).RotateFlip((RotateFlipType)1);
			graphics.DrawImage((Image)(object)val, rectangle.Right - info.Size, rectangle.Y);
			((Image)val).RotateFlip((RotateFlipType)1);
			graphics.DrawImage((Image)(object)val, rectangle.Right - info.Size, rectangle.Bottom - info.Size);
			((Image)val).RotateFlip((RotateFlipType)1);
			graphics.DrawImage((Image)(object)val, rectangle.X, rectangle.Bottom - info.Size);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		Rectangle rectangle2 = new Rectangle(rectangle.X + info.Size, rectangle.Y + 1, rectangle.Width - info.Size * 2, info.Size - 1);
		LinearGradientBrush val5 = DisplayHelp.CreateLinearGradientBrush(rectangle2, Color.Transparent, color, 90f);
		try
		{
			graphics.FillRectangle((Brush)(object)val5, rectangle2);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		rectangle2.Offset(0, rectangle.Height - info.Size - 1);
		LinearGradientBrush val6 = DisplayHelp.CreateLinearGradientBrush(rectangle2, color, Color.Transparent, 90f);
		try
		{
			graphics.FillRectangle((Brush)(object)val6, rectangle2);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		rectangle2 = new Rectangle(rectangle.X, rectangle.Y + info.Size, info.Size, rectangle.Height - info.Size * 2);
		LinearGradientBrush val7 = DisplayHelp.CreateLinearGradientBrush(rectangle2, Color.Transparent, color, 0f);
		try
		{
			graphics.FillRectangle((Brush)(object)val7, rectangle2);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
		rectangle2.Offset(rectangle.Width - info.Size - 1, 0);
		LinearGradientBrush val8 = DisplayHelp.CreateLinearGradientBrush(rectangle2, color, Color.Transparent, 0f);
		try
		{
			graphics.FillRectangle((Brush)(object)val8, rectangle2);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
		graphics.SmoothingMode = smoothingMode;
	}

	public static void Paint3(ShadowPaintInfo p)
	{
		Paint3(p, 1);
	}

	public static void Paint3(ShadowPaintInfo p, int shadowVersion)
	{
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		if (LastUsedShadowVersion != shadowVersion && _ShadowTemplate != null)
		{
			((Image)_ShadowTemplate).Dispose();
			_ShadowTemplate = null;
			for (int i = 0; i < _ShadowParts.Length; i++)
			{
				((Image)_ShadowParts[i]).Dispose();
			}
			_ShadowParts = null;
		}
		if (_ShadowTemplate == null)
		{
			if (shadowVersion == 1)
			{
				_ShadowTemplate = BarFunctions.LoadBitmap("SystemImages.Shadow3px.png");
			}
			else
			{
				_ShadowTemplate = BarFunctions.LoadBitmap("SystemImages.Shadow3px-2.png");
			}
			_ShadowParts = (Bitmap[])(object)new Bitmap[4];
			Bitmap val = new Bitmap((Image)(object)_ShadowTemplate, 1, 1);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				val2.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(0, 0, 1, 1), new Rectangle(0, 3, 1, 1), (GraphicsUnit)2);
				_ShadowParts[0] = val;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			val = new Bitmap((Image)(object)_ShadowTemplate, 1, 1);
			Graphics val3 = Graphics.FromImage((Image)(object)val);
			try
			{
				val3.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(0, 0, 1, 1), new Rectangle(3, 0, 1, 1), (GraphicsUnit)2);
				_ShadowParts[1] = val;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			val = new Bitmap((Image)(object)_ShadowTemplate, 3, 1);
			Graphics val4 = Graphics.FromImage((Image)(object)val);
			try
			{
				val4.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(0, 0, 3, 1), new Rectangle(((Image)_ShadowTemplate).Width - 3, 6, 3, 1), (GraphicsUnit)2);
				_ShadowParts[2] = val;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			val = new Bitmap((Image)(object)_ShadowTemplate, 1, 3);
			Graphics val5 = Graphics.FromImage((Image)(object)val);
			try
			{
				val5.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(0, 0, 1, 3), new Rectangle(5, ((Image)_ShadowTemplate).Height - 3, 1, 3), (GraphicsUnit)2);
				_ShadowParts[3] = val;
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		Graphics graphics = p.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		Rectangle rectangle = p.Rectangle;
		graphics.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(rectangle.X, rectangle.Y, 1, 1), new Rectangle(0, 0, 1, 1), (GraphicsUnit)2);
		TextureBrush val6 = new TextureBrush((Image)(object)_ShadowParts[0], (WrapMode)0);
		try
		{
			graphics.FillRectangle((Brush)(object)val6, new Rectangle(rectangle.X, rectangle.Y + 1, ((Image)_ShadowParts[0]).Width, rectangle.Height - 4));
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		TextureBrush val7 = new TextureBrush((Image)(object)_ShadowParts[1], (WrapMode)0);
		try
		{
			graphics.FillRectangle((Brush)(object)val7, new Rectangle(rectangle.X + 1, rectangle.Y, rectangle.Width - 4, ((Image)_ShadowParts[1]).Height));
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
		graphics.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(rectangle.Right - 3, rectangle.Y, 3, 4), new Rectangle(((Image)_ShadowTemplate).Width - 3, 0, 3, 4), (GraphicsUnit)2);
		TextureBrush val8 = new TextureBrush((Image)(object)_ShadowParts[2], (WrapMode)0);
		try
		{
			Rectangle rectangle2 = new Rectangle(rectangle.Right - ((Image)_ShadowParts[2]).Width, rectangle.Y + 4, ((Image)_ShadowParts[2]).Width, rectangle.Height - 7);
			val8.TranslateTransform((float)rectangle2.X, (float)rectangle2.Y);
			graphics.FillRectangle((Brush)(object)val8, rectangle2);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
		graphics.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(rectangle.Right - 4, rectangle.Bottom - 3, 4, 3), new Rectangle(((Image)_ShadowTemplate).Width - 4, ((Image)_ShadowTemplate).Height - 3, 4, 3), (GraphicsUnit)2);
		TextureBrush val9 = new TextureBrush((Image)(object)_ShadowParts[3], (WrapMode)0);
		try
		{
			Rectangle rectangle3 = new Rectangle(rectangle.X + 4, rectangle.Bottom - 3, rectangle.Width - 8, ((Image)_ShadowParts[3]).Height);
			val9.TranslateTransform((float)rectangle3.X, (float)rectangle3.Y);
			graphics.FillRectangle((Brush)(object)val9, rectangle3);
		}
		finally
		{
			((IDisposable)val9)?.Dispose();
		}
		graphics.DrawImage((Image)(object)_ShadowTemplate, new Rectangle(rectangle.X, rectangle.Bottom - 3, 4, 3), new Rectangle(0, ((Image)_ShadowTemplate).Height - 3, 4, 3), (GraphicsUnit)2);
		graphics.SmoothingMode = smoothingMode;
	}
}
