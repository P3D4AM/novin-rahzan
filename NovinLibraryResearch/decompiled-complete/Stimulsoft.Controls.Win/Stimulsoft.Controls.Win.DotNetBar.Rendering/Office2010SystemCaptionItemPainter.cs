using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2010SystemCaptionItemPainter : Office2007SystemCaptionItemPainter
{
	protected override void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		Size s = new Size(11, 5);
		Rectangle signRect = GetSignRect(r, s);
		DisplayHelp.DrawRoundedRectangle(g, ct.DarkShade, ct.Foreground, signRect, 1);
	}

	protected override void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		Size s = new Size(12, 11);
		Rectangle signRect = GetSignRect(r, s);
		Region clip = g.Clip;
		Brush val = DisplayHelp.CreateBrush(signRect, ct.Foreground);
		try
		{
			Pen val2 = new Pen(ct.DarkShade);
			try
			{
				GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(new Rectangle(signRect.X + 5, signRect.Y, 8, 8), 1);
				try
				{
					Rectangle rectangle = new Rectangle(signRect.X + 7, signRect.Y + 4, 4, 2);
					g.SetClip(rectangle, (CombineMode)4);
					g.SetClip(new Rectangle(signRect.X, signRect.Y + 3, 8, 8), (CombineMode)4);
					g.FillPath(val, roundedRectanglePath);
					g.DrawPath(val2, roundedRectanglePath);
					g.ResetClip();
					g.DrawRectangle(val2, rectangle);
				}
				finally
				{
					((IDisposable)roundedRectanglePath)?.Dispose();
				}
				GraphicsPath roundedRectanglePath2 = DisplayHelp.GetRoundedRectanglePath(new Rectangle(signRect.X, signRect.Y + 3, 8, 8), 1);
				try
				{
					Rectangle rectangle2 = new Rectangle(signRect.X + 2, signRect.Y + 7, 4, 2);
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

	protected override void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		Size s = new Size(11, 9);
		Rectangle signRect = GetSignRect(r, s);
		Region clip = g.Clip;
		Brush val = DisplayHelp.CreateBrush(signRect, ct.Foreground);
		try
		{
			Pen val2 = new Pen(ct.DarkShade);
			try
			{
				Rectangle rectangle = new Rectangle(signRect.X + 3, signRect.Y + 3, ct.DarkShade.IsEmpty ? 5 : 4, ct.DarkShade.IsEmpty ? 3 : 2);
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

	protected override void PaintClose(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(11, 9);
		Rectangle signRect = GetSignRect(r, s);
		Rectangle rectangle = signRect;
		rectangle.Inflate(-1, 0);
		rectangle.Height--;
		GraphicsPath val = new GraphicsPath();
		try
		{
			val.AddLine(rectangle.X + 1, rectangle.Y, rectangle.X + 3, rectangle.Y);
			val.AddLine(rectangle.X + 3, rectangle.Y, rectangle.X + 5, rectangle.Y + 2);
			val.AddLine(rectangle.X + 5, rectangle.Y + 2, rectangle.X + 7, rectangle.Y);
			val.AddLine(rectangle.X + 7, rectangle.Y, rectangle.X + 9, rectangle.Y);
			val.AddLine(rectangle.X + 10, rectangle.Y + 1, rectangle.X + 7, rectangle.Y + 4);
			val.AddLine(rectangle.X + 7, rectangle.Y + 4, rectangle.X + 10, rectangle.Y + 7);
			val.AddLine(rectangle.X + 10, rectangle.Y + 7, rectangle.X + 9, rectangle.Y + 8);
			val.AddLine(rectangle.X + 9, rectangle.Y + 8, rectangle.X + 7, rectangle.Y + 8);
			val.AddLine(rectangle.X + 7, rectangle.Y + 8, rectangle.X + 5, rectangle.Y + 6);
			val.AddLine(rectangle.X + 5, rectangle.Y + 6, rectangle.X + 3, rectangle.Y + 8);
			val.AddLine(rectangle.X + 3, rectangle.Y + 8, rectangle.X + 1, rectangle.Y + 8);
			val.AddLine(rectangle.X, rectangle.Y + 7, rectangle.X + 3, rectangle.Y + 4);
			val.AddLine(rectangle.X + 3, rectangle.Y + 4, rectangle.X, rectangle.Y + 1);
			if (isEnabled)
			{
				DisplayHelp.FillPath(g, val, ct.Foreground);
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
					Pen val3 = new Pen(ct.Foreground.Start);
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
				LinearGradientColorTable table = new LinearGradientColorTable(ct.Foreground.Start.IsEmpty ? ct.Foreground.Start : Color.FromArgb(128, ct.Foreground.Start), ct.Foreground.End.IsEmpty ? ct.Foreground.End : Color.FromArgb(128, ct.Foreground.End), ct.Foreground.GradientAngle);
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

	protected override void PaintHelp(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
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
			SolidBrush val3 = new SolidBrush(ct.Foreground.Start);
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
