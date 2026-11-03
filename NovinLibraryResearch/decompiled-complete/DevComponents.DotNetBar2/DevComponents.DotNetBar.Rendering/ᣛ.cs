using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar.Rendering;

internal class ᣛ : ᣜ
{
	protected override void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		Size s = new Size(Dpi.Width11, Dpi.Height5);
		Rectangle signRect = GetSignRect(r, s);
		DisplayHelp.DrawRoundedRectangle(g, ct.DarkShade, foregroundOverride ?? ct.Foreground, signRect, 1);
	}

	protected override void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		Size s = new Size(Dpi.Width12, Dpi.Height11);
		Rectangle signRect = GetSignRect(r, s);
		Region clip = g.Clip;
		LinearGradientColorTable colorTable = foregroundOverride ?? ct.Foreground;
		Brush val = DisplayHelp.CreateBrush(signRect, colorTable);
		try
		{
			Pen val2 = new Pen(ct.DarkShade);
			try
			{
				GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(new Rectangle(signRect.X + Dpi.Width5, signRect.Y, Dpi.Width8, Dpi.Height8), 1);
				try
				{
					Rectangle rectangle = new Rectangle(signRect.X + Dpi.Width7, signRect.Y + Dpi.Height4, Dpi.Width4, Dpi.Width2);
					g.SetClip(rectangle, (CombineMode)4);
					g.SetClip(new Rectangle(signRect.X, signRect.Y + Dpi.Height3, Dpi.Width8, Dpi.Height8), (CombineMode)4);
					g.FillPath(val, roundedRectanglePath);
					g.DrawPath(val2, roundedRectanglePath);
					g.ResetClip();
					g.DrawRectangle(val2, rectangle);
				}
				finally
				{
					((IDisposable)roundedRectanglePath)?.Dispose();
				}
				GraphicsPath roundedRectanglePath2 = DisplayHelp.GetRoundedRectanglePath(new Rectangle(signRect.X, signRect.Y + Dpi.Height3, Dpi.Width8, Dpi.Height8), 1);
				try
				{
					Rectangle rectangle2 = new Rectangle(signRect.X + Dpi.Width2, signRect.Y + Dpi.Height7, Dpi.Width4, Dpi.Width2);
					g.SetClip(rectangle2, (CombineMode)4);
					g.FillPath(val, roundedRectanglePath2);
					g.DrawPath(val2, roundedRectanglePath2);
					g.ResetClip();
					g.DrawRectangle(val2, rectangle2);
				}
				finally
				{
					((IDisposable)roundedRectanglePath2)?.Dispose();
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
		if (clip != null)
		{
			g.Clip = clip;
			clip.Dispose();
		}
	}

	protected override void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		Size s = new Size(Dpi.Width11, Dpi.Height9);
		Rectangle signRect = GetSignRect(r, s);
		Region clip = g.Clip;
		LinearGradientColorTable colorTable = foregroundOverride ?? ct.Foreground;
		Brush val = DisplayHelp.CreateBrush(signRect, colorTable);
		try
		{
			Pen val2 = new Pen(ct.DarkShade);
			try
			{
				Rectangle rectangle = new Rectangle(signRect.X + Dpi.Width3, signRect.Y + Dpi.Height3, Dpi.Width(ct.DarkShade.IsEmpty ? 5 : 4), Dpi.Height(ct.DarkShade.IsEmpty ? 3 : 2));
				g.SetClip(rectangle, (CombineMode)4);
				DisplayHelp.DrawRoundedRectangle(g, val2, val, signRect.X, signRect.Y, signRect.Width, signRect.Height, 1);
				g.ResetClip();
				g.DrawRectangle(val2, rectangle);
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
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(Dpi.Width11, Dpi.Height9);
		Rectangle signRect = GetSignRect(r, s);
		Rectangle rectangle = signRect;
		rectangle.Inflate(-1, 0);
		rectangle.Height--;
		GraphicsPath val = new GraphicsPath();
		try
		{
			val.AddLine(rectangle.X + Dpi.Width1, rectangle.Y, rectangle.X + Dpi.Width3, rectangle.Y);
			val.AddLine(rectangle.X + Dpi.Width3, rectangle.Y, rectangle.X + Dpi.Width5, rectangle.Y + Dpi.Width2);
			val.AddLine(rectangle.X + Dpi.Width5, rectangle.Y + Dpi.Width2, rectangle.X + Dpi.Width7, rectangle.Y);
			val.AddLine(rectangle.X + Dpi.Width7, rectangle.Y, rectangle.X + Dpi.Width9, rectangle.Y);
			val.AddLine(rectangle.X + Dpi.Width10, rectangle.Y + Dpi.Height1, rectangle.X + Dpi.Width7, rectangle.Y + Dpi.Height4);
			val.AddLine(rectangle.X + Dpi.Width7, rectangle.Y + Dpi.Height4, rectangle.X + Dpi.Width10, rectangle.Y + Dpi.Height7);
			val.AddLine(rectangle.X + Dpi.Width10, rectangle.Y + Dpi.Height7, rectangle.X + Dpi.Width9, rectangle.Y + Dpi.Height8);
			val.AddLine(rectangle.X + Dpi.Width9, rectangle.Y + Dpi.Height8, rectangle.X + Dpi.Width7, rectangle.Y + Dpi.Height8);
			val.AddLine(rectangle.X + Dpi.Width7, rectangle.Y + Dpi.Height8, rectangle.X + Dpi.Width5, rectangle.Y + Dpi.Height6);
			val.AddLine(rectangle.X + Dpi.Width5, rectangle.Y + Dpi.Height6, rectangle.X + Dpi.Width3, rectangle.Y + Dpi.Height8);
			val.AddLine(rectangle.X + Dpi.Width3, rectangle.Y + Dpi.Height8, rectangle.X + Dpi.Width1, rectangle.Y + Dpi.Height8);
			val.AddLine(rectangle.X, rectangle.Y + Dpi.Height7, rectangle.X + Dpi.Width3, rectangle.Y + Dpi.Height4);
			val.AddLine(rectangle.X + Dpi.Width3, rectangle.Y + Dpi.Height4, rectangle.X, rectangle.Y + Dpi.Height1);
			LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
			if (isEnabled)
			{
				DisplayHelp.FillPath(g, val, linearGradientColorTable);
				if (!ct.DarkShade.IsEmpty)
				{
					Pen val2 = new Pen(ct.DarkShade);
					try
					{
						g.DrawPath(val2, val);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				else
				{
					Pen val3 = new Pen(linearGradientColorTable.Start);
					try
					{
						g.DrawPath(val3, val);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
			}
			else
			{
				LinearGradientColorTable table = new LinearGradientColorTable(linearGradientColorTable.Start.IsEmpty ? linearGradientColorTable.Start : Color.FromArgb(128, linearGradientColorTable.Start), linearGradientColorTable.End.IsEmpty ? linearGradientColorTable.End : Color.FromArgb(128, linearGradientColorTable.End), linearGradientColorTable.GradientAngle);
				DisplayHelp.FillPath(g, val, table);
				DisplayHelp.DrawGradientPathBorder(g, val, table, 1);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		g.SmoothingMode = (SmoothingMode)0;
		g.TextRenderingHint = (TextRenderingHint)5;
		Font val = new Font(SystemFonts.DefaultFont, (FontStyle)1);
		try
		{
			Size s = TextDrawing.MeasureString(g, "?", val);
			s.Width += 4;
			s.Height -= 2;
			Rectangle signRect = GetSignRect(r, s);
			signRect.Offset(1, 1);
			SolidBrush val2 = new SolidBrush(ct.DarkShade);
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
			SolidBrush val3 = new SolidBrush(linearGradientColorTable.Start);
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
