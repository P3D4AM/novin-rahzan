using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Metro.Rendering;

internal class ᣚ : ᣜ
{
	protected override Rectangle GetSignRect(Rectangle r, Size s)
	{
		if (r.Height < 10)
		{
			return Rectangle.Empty;
		}
		return new Rectangle(r.X + (r.Width - s.Width) / 2, r.Y + (int)Math.Ceiling((double)(r.Height - s.Height) / 2.0), s.Width, s.Height);
	}

	protected override void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		Size s = new Size(Dpi.Width9, Dpi.Height2);
		Rectangle signRect = GetSignRect(r, s);
		LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
		if (isEnabled)
		{
			DisplayHelp.FillRectangle(g, signRect, linearGradientColorTable);
		}
		else
		{
			DisplayHelp.FillRectangle(g, signRect, _2518(linearGradientColorTable.Start), _2518(linearGradientColorTable.End), linearGradientColorTable.GradientAngle);
		}
	}

	internal static Color _2518(Color _065B)
	{
		if (_065B.IsEmpty)
		{
			return _065B;
		}
		return Color.FromArgb(128, _065B);
	}

	protected override void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)3;
		Size s = new Size(Dpi.Width10, Dpi.Height10);
		Rectangle signRect = GetSignRect(r, s);
		Region clip = g.Clip;
		LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
		if (!isEnabled)
		{
			new LinearGradientColorTable(_2518(linearGradientColorTable.Start), _2518(linearGradientColorTable.End), linearGradientColorTable.GradientAngle);
		}
		Brush val = DisplayHelp.CreateBrush(signRect, linearGradientColorTable);
		try
		{
			Rectangle rectangle = new Rectangle(signRect.X + Dpi.Width4, signRect.Y + Dpi.Width2, Dpi.Width6, Dpi.Height4);
			g.SetClip(rectangle, (CombineMode)4);
			g.SetClip(new Rectangle(signRect.X + Dpi.Width1, signRect.Y + Dpi.Height5, Dpi.Width6, Dpi.Height4), (CombineMode)4);
			g.FillRectangle(val, signRect.X + Dpi.Width3, signRect.Y, Dpi.Width8, Dpi.Height7);
			g.ResetClip();
			rectangle = new Rectangle(signRect.X + Dpi.Width1, signRect.Y + Dpi.Height5, Dpi.Width6, Dpi.Height4);
			g.SetClip(rectangle, (CombineMode)4);
			g.FillRectangle(val, signRect.X, signRect.Y + Dpi.Height3, Dpi.Width8, Dpi.Height7);
			g.ResetClip();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (clip != null)
		{
			g.Clip = clip;
			clip.Dispose();
		}
		g.SmoothingMode = smoothingMode;
	}

	protected override void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		Size s = new Size(Dpi.Width10, Dpi.Height10);
		Rectangle signRect = GetSignRect(r, s);
		Region clip = g.Clip;
		Rectangle rectangle = new Rectangle(signRect.X + Dpi.Width1, signRect.Y + Dpi.Height3, ct.DarkShade.IsEmpty ? Dpi.Width8 : Dpi.Width7, ct.DarkShade.IsEmpty ? Dpi.Height6 : Dpi.Height5);
		g.SetClip(rectangle, (CombineMode)4);
		LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
		if (isEnabled)
		{
			DisplayHelp.FillRectangle(g, signRect, linearGradientColorTable);
		}
		else
		{
			DisplayHelp.FillRectangle(g, signRect, _2518(linearGradientColorTable.Start), _2518(linearGradientColorTable.End), linearGradientColorTable.GradientAngle);
		}
		if (clip != null)
		{
			g.Clip = clip;
			clip.Dispose();
		}
	}

	protected override void PaintClose(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(Dpi.Width8, Dpi.Height8);
		Rectangle signRect = GetSignRect(r, s);
		Rectangle rectangle = signRect;
		rectangle.Offset(0, -1);
		LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
		if (isEnabled)
		{
			Pen val = new Pen(linearGradientColorTable.Start, (float)Dpi.Width2);
			try
			{
				g.DrawLine(val, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
				g.DrawLine(val, rectangle.Right, rectangle.Y, rectangle.X, rectangle.Bottom);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			Pen val2 = new Pen(_2518(linearGradientColorTable.Start), (float)Dpi.Width2);
			try
			{
				g.DrawLine(val2, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
				g.DrawLine(val2, rectangle.Right, rectangle.Y, rectangle.X, rectangle.Bottom);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}

	protected override void PaintHelp(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		g.SmoothingMode = (SmoothingMode)0;
		g.TextRenderingHint = (TextRenderingHint)5;
		Font val = new Font(SystemFonts.DefaultFont, (FontStyle)1);
		try
		{
			Size s = TextDrawing.MeasureString(g, "?", val);
			s.Width += Dpi.Width4;
			s.Height -= Dpi.Width2;
			Rectangle signRect = GetSignRect(r, s);
			signRect.Offset(1, 1);
			Color color = (isEnabled ? ct.DarkShade : _2518(ct.DarkShade));
			SolidBrush val2 = new SolidBrush(color);
			try
			{
				g.DrawString("?", val, (Brush)(object)val2, (RectangleF)signRect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			signRect.Offset(-1, -1);
			LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
			color = (isEnabled ? linearGradientColorTable.Start : _2518(linearGradientColorTable.Start));
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				g.DrawString("?", val, (Brush)(object)val3, (RectangleF)signRect);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
		g.TextRenderingHint = textRenderingHint;
	}
}
