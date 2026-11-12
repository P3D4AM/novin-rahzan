using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroCaptionItemPainter : Office2007SystemCaptionItemPainter
{
	protected override Rectangle GetSignRect(Rectangle r, Size s)
	{
		if (r.Height < StiScale2.YYI(10.0))
		{
			return Rectangle.Empty;
		}
		return new Rectangle(r.X + (r.Width - s.Width) / 2, r.Bottom - r.Height / 4 - s.Height - StiScale2.I3, s.Width, s.Height);
	}

	protected override void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		Size size = new Size(StiScale2.XXI(12.0), StiScale2.YYI(1.0));
		int height = ((!IsUsedWin11()) ? 1 : StiScale2.I1);
		Rectangle r2 = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2, size.Width, height);
		if (isEnabled)
		{
			DisplayHelp.FillRectangle(g, r2, ct.Foreground);
		}
		else
		{
			DisplayHelp.FillRectangle(g, r2, GetDisabledColor(ct.Foreground.Start), GetDisabledColor(ct.Foreground.End), ct.Foreground.GradientAngle);
		}
	}

	internal static Color GetDisabledColor(Color color)
	{
		if (color.IsEmpty)
		{
			return color;
		}
		return Color.FromArgb(128, color);
	}

	public override void PaintSystemIcon(SystemCaptionItemRendererEventArgs e, bool isEnabled)
	{
		Graphics graphics = e.Graphics;
		SystemCaptionItem systemCaptionItem = e.SystemCaptionItem;
		Rectangle displayRectangle = systemCaptionItem.DisplayRectangle;
		if (systemCaptionItem.EffectiveStyle == eDotNetBarStyle.Metro && displayRectangle.Y == 4)
		{
			displayRectangle.Offset((displayRectangle.X + displayRectangle.Width - 16) / 2, (displayRectangle.Y + displayRectangle.Height - 16) / 2);
		}
		else
		{
			displayRectangle.Offset((displayRectangle.Width - 16) / 2, (displayRectangle.Height - 16) / 2);
		}
		if (systemCaptionItem.Icon == null)
		{
			return;
		}
		if (Environment.Version.Build <= 3705 && Environment.Version.Revision == 288 && Environment.Version.Major == 1 && Environment.Version.Minor == 0)
		{
			IntPtr hdc = graphics.GetHdc();
			try
			{
				NativeFunctions.DrawIconEx(hdc, displayRectangle.X, displayRectangle.Y, systemCaptionItem.Icon.Handle, displayRectangle.Width, displayRectangle.Height, 0, IntPtr.Zero, 3);
				return;
			}
			finally
			{
				graphics.ReleaseHdc(hdc);
			}
		}
		graphics.DrawIcon(systemCaptionItem.Icon, displayRectangle);
	}

	private bool IsUsedWin11()
	{
		if (StiOSVersion.IsWindows11())
		{
			return StiScale2.Factor >= 2.0;
		}
		return false;
	}

	protected override void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		if (IsUsedWin11())
		{
			PaintRestoreWin11(g, r, ct, isEnabled);
		}
		else
		{
			PaintRestoreWin10(g, r, ct, isEnabled);
		}
	}

	private void PaintRestoreWin10(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)3;
		Size size = new Size(StiScale2.XXI(12.0), StiScale2.YYI(12.0));
		Rectangle bounds = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2, size.Width, size.Height);
		Region clip = g.Clip;
		Brush val = DisplayHelp.CreateBrush(bounds, ct.Foreground);
		try
		{
			Rectangle rectangle = new Rectangle(bounds.X, bounds.Y + StiScale2.I4, StiScale2.I9, StiScale2.I9);
			Rectangle rectangle2 = new Rectangle(bounds.X + StiScale2.I4, bounds.Y, StiScale2.I9, StiScale2.I9);
			Rectangle rectangle3 = rectangle;
			Rectangle rectangle4 = rectangle2;
			rectangle3.Inflate(-1, -1);
			rectangle4.Inflate(-1, -1);
			g.SetClip(rectangle3, (CombineMode)4);
			g.FillRectangle(val, rectangle);
			g.ResetClip();
			g.SetClip(rectangle4, (CombineMode)4);
			g.SetClip(rectangle, (CombineMode)4);
			g.FillRectangle(val, rectangle2);
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

	private void PaintRestoreWin11(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)2;
		Size size = new Size(StiScale2.XXI(8.0), StiScale2.YYI(8.0));
		Rectangle bounds = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2 - StiScale2.I2, size.Width, size.Height);
		Region clip = g.Clip;
		Brush val = DisplayHelp.CreateBrush(bounds, ct.Foreground);
		try
		{
			Pen val2 = new Pen(val, 1.6f);
			try
			{
				Rectangle rectangle = new Rectangle(bounds.X, bounds.Y + StiScale2.I2, StiScale2.XXI(8.0), StiScale2.XXI(8.0));
				Rectangle rectangle2 = new Rectangle(bounds.X + StiScale2.I2, bounds.Y, StiScale2.XXI(8.0), StiScale2.XXI(8.0));
				Rectangle rectangle3 = rectangle;
				Rectangle rectangle4 = rectangle2;
				rectangle3.Inflate(-StiScale2.I2, -StiScale2.I2);
				rectangle4.Inflate(-StiScale2.I2, -StiScale2.I2);
				g.SetClip(new Rectangle(rectangle2.X - StiScale2.I1, rectangle2.Y + StiScale2.I1, StiScale2.I4, rectangle2.Height), (CombineMode)4);
				g.SetClip(new Rectangle(rectangle2.X, rectangle2.Y + StiScale2.I2, rectangle2.Width, rectangle2.Height), (CombineMode)4);
				GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(rectangle2, StiScale2.I2, StiScale2.I2, StiScale2.I2, StiScale2.I2);
				g.DrawPath(val2, roundedRectanglePath);
				g.ResetClip();
				GraphicsPath roundedRectanglePath2 = DisplayHelp.GetRoundedRectanglePath(rectangle, StiScale2.I2, StiScale2.I2, StiScale2.I2, StiScale2.I2);
				g.DrawPath(val2, roundedRectanglePath2);
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
		g.SmoothingMode = smoothingMode;
	}

	protected override void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		if (IsUsedWin11())
		{
			PaintMaximizeWin11(g, r, ct, isEnabled);
		}
		else
		{
			PaintMaximizeWin10(g, r, ct, isEnabled);
		}
	}

	private void PaintMaximizeWin10(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		Size size = new Size(StiScale2.XXI(12.0), StiScale2.YYI(12.0));
		Rectangle rectangle = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2, size.Width, size.Height);
		Region clip = g.Clip;
		Rectangle rectangle2 = rectangle;
		rectangle2.Inflate(-1, -1);
		g.SetClip(rectangle2, (CombineMode)4);
		if (isEnabled)
		{
			DisplayHelp.FillRectangle(g, rectangle, ct.Foreground);
		}
		else
		{
			DisplayHelp.FillRectangle(g, rectangle, GetDisabledColor(ct.Foreground.Start), GetDisabledColor(ct.Foreground.End), ct.Foreground.GradientAngle);
		}
		if (clip != null)
		{
			g.Clip = clip;
			clip.Dispose();
		}
	}

	private void PaintMaximizeWin11(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		Size size = new Size(StiScale2.XXI(10.0), StiScale2.YYI(10.0));
		Rectangle rectangle = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2 - StiScale2.I1, size.Width, size.Height);
		Region clip = g.Clip;
		if (isEnabled)
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)2;
			Brush val = DisplayHelp.CreateBrush(rectangle, ct.Foreground);
			try
			{
				GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(rectangle, StiScale2.I2, StiScale2.I2, StiScale2.I2, StiScale2.I2);
				Pen val2 = new Pen(val, 1.6f);
				try
				{
					g.DrawPath(val2, roundedRectanglePath);
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
			g.SmoothingMode = smoothingMode;
		}
		else
		{
			DisplayHelp.FillRectangle(g, rectangle, GetDisabledColor(ct.Foreground.Start), GetDisabledColor(ct.Foreground.End), ct.Foreground.GradientAngle);
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
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			Size size = new Size(StiScale2.XXI(10.0), StiScale2.YYI(10.0));
			Rectangle rectangle = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2, size.Width, size.Height);
			Rectangle rectangle2 = rectangle;
			rectangle2.Offset(0, -StiScale2.I1);
			if (isEnabled)
			{
				Pen val = new Pen(ct.Foreground.Start, 1.6f);
				try
				{
					val.StartCap = (LineCap)1;
					val.EndCap = (LineCap)1;
					g.DrawLine(val, rectangle2.X, rectangle2.Y, rectangle2.Right, rectangle2.Bottom);
					g.DrawLine(val, rectangle2.Right, rectangle2.Y, rectangle2.X, rectangle2.Bottom);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			Pen val2 = new Pen(GetDisabledColor(ct.Foreground.Start), 1.6f);
			try
			{
				val2.StartCap = (LineCap)1;
				val2.EndCap = (LineCap)1;
				g.DrawLine(val2, rectangle2.X, rectangle2.Y, rectangle2.Right, rectangle2.Bottom);
				g.DrawLine(val2, rectangle2.Right, rectangle2.Y, rectangle2.X, rectangle2.Bottom);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	protected override void PaintHelp(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		g.SmoothingMode = (SmoothingMode)0;
		g.TextRenderingHint = (TextRenderingHint)5;
		Font val = new Font(SystemFonts.DefaultFont, (FontStyle)1);
		try
		{
			Size s = TextDrawing.MeasureString(g, "?", val);
			s.Width += StiScale2.I4;
			s.Height -= StiScale2.I2;
			Rectangle signRect = GetSignRect(r, s);
			signRect.Offset(StiScale2.I1, StiScale2.I1);
			Color color = (isEnabled ? ct.DarkShade : GetDisabledColor(ct.DarkShade));
			SolidBrush val2 = new SolidBrush(color);
			try
			{
				g.DrawString("?", val, (Brush)(object)val2, (RectangleF)signRect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			signRect.Offset(-StiScale2.I1, -StiScale2.I1);
			color = (isEnabled ? ct.Foreground.Start : GetDisabledColor(ct.Foreground.Start));
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
