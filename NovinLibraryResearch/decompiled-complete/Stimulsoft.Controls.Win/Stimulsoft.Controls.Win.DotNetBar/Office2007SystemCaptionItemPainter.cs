using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007SystemCaptionItemPainter : SystemCaptionItemPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public override void PaintSystemIcon(SystemCaptionItemRendererEventArgs e, bool isEnabled)
	{
		Graphics graphics = e.Graphics;
		SystemCaptionItem systemCaptionItem = e.SystemCaptionItem;
		Rectangle displayRectangle = systemCaptionItem.DisplayRectangle;
		displayRectangle.Offset((displayRectangle.Width - 16) / 2, (displayRectangle.Height - 16) / 2);
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

	public override void PaintFormButtons(SystemCaptionItemRendererEventArgs e)
	{
		if (e.GlassEnabled)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		SystemCaptionItem systemCaptionItem = e.SystemCaptionItem;
		Office2007SystemButtonColorTable office2007SystemButtonColorTable = m_ColorTable.SystemButton;
		Rectangle displayRectangle = systemCaptionItem.DisplayRectangle;
		Region clip = graphics.Clip;
		Rectangle clip2 = displayRectangle;
		clip2.Height++;
		graphics.SetClip(clip2);
		Size size = systemCaptionItem.GetButtonSize();
		if (size.Height > displayRectangle.Height)
		{
			size = new Size(displayRectangle.Height, displayRectangle.Height);
		}
		bool enabled = systemCaptionItem.GetEnabled();
		Rectangle r = new Rectangle(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height);
		Office2007SystemButtonStateColorTable ct = office2007SystemButtonColorTable.Default;
		if (systemCaptionItem.HelpVisible && (!systemCaptionItem.IsRightToLeft || (systemCaptionItem.CloseVisible && systemCaptionItem.IsRightToLeft)))
		{
			Office2007SystemButtonColorTable office2007SystemButtonColorTable2 = office2007SystemButtonColorTable;
			if (systemCaptionItem.CloseEnabled && enabled && systemCaptionItem.IsRightToLeft && m_ColorTable.SystemButtonClose != null)
			{
				office2007SystemButtonColorTable = m_ColorTable.SystemButtonClose;
			}
			if ((systemCaptionItem.CloseEnabled && enabled && systemCaptionItem.IsRightToLeft) || !systemCaptionItem.IsRightToLeft)
			{
				if ((systemCaptionItem.MouseDownButton == SystemButton.Help && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.MouseDownButton == SystemButton.Close && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.Pressed;
				}
				else if ((systemCaptionItem.MouseOverButton == SystemButton.Help && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.MouseOverButton == SystemButton.Close && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.MouseOver;
				}
			}
			PaintBackground(graphics, r, ct);
			if (systemCaptionItem.IsRightToLeft)
			{
				PaintClose(graphics, r, ct, systemCaptionItem.CloseEnabled && enabled);
			}
			else
			{
				PaintHelp(graphics, r, ct, enabled);
			}
			r.Offset(r.Width + 1, 0);
			office2007SystemButtonColorTable = office2007SystemButtonColorTable2;
		}
		if ((systemCaptionItem.MinimizeVisible && systemCaptionItem.HelpVisible) || (systemCaptionItem.MinimizeVisible && !systemCaptionItem.HelpVisible && (!systemCaptionItem.IsRightToLeft || (systemCaptionItem.CloseVisible && systemCaptionItem.IsRightToLeft))))
		{
			ct = office2007SystemButtonColorTable.Default;
			if ((systemCaptionItem.CloseEnabled && enabled && systemCaptionItem.IsRightToLeft) || !systemCaptionItem.IsRightToLeft)
			{
				if ((systemCaptionItem.MouseDownButton == SystemButton.Minimize && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.MouseDownButton == SystemButton.Close && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.Pressed;
				}
				else if ((systemCaptionItem.MouseOverButton == SystemButton.Minimize && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.MouseOverButton == SystemButton.Close && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.MouseOver;
				}
			}
			PaintBackground(graphics, r, ct);
			if (systemCaptionItem.IsRightToLeft)
			{
				PaintClose(graphics, r, ct, systemCaptionItem.CloseEnabled && enabled);
			}
			else
			{
				PaintMinimize(graphics, r, ct, enabled);
			}
			r.Offset(r.Width + 1, 0);
		}
		if (systemCaptionItem.RestoreMaximizeVisible)
		{
			if (systemCaptionItem.RestoreEnabled && enabled)
			{
				ct = office2007SystemButtonColorTable.Default;
				if (systemCaptionItem.MouseDownButton == SystemButton.Restore)
				{
					ct = office2007SystemButtonColorTable.Pressed;
				}
				else if (systemCaptionItem.MouseOverButton == SystemButton.Restore)
				{
					ct = office2007SystemButtonColorTable.MouseOver;
				}
				PaintBackground(graphics, r, ct);
				PaintRestore(graphics, r, ct, enabled);
			}
			else
			{
				ct = office2007SystemButtonColorTable.Default;
				if (systemCaptionItem.MouseDownButton == SystemButton.Maximize)
				{
					ct = office2007SystemButtonColorTable.Pressed;
				}
				else if (systemCaptionItem.MouseOverButton == SystemButton.Maximize)
				{
					ct = office2007SystemButtonColorTable.MouseOver;
				}
				PaintBackground(graphics, r, ct);
				PaintMaximize(graphics, r, ct, enabled);
			}
			r.Offset(r.Width + 1, 0);
		}
		if ((systemCaptionItem.CloseVisible && !systemCaptionItem.IsRightToLeft) || (!systemCaptionItem.HelpVisible && systemCaptionItem.MinimizeVisible && systemCaptionItem.IsRightToLeft) || (systemCaptionItem.HelpVisible && systemCaptionItem.IsRightToLeft))
		{
			Office2007SystemButtonColorTable office2007SystemButtonColorTable3 = office2007SystemButtonColorTable;
			if (systemCaptionItem.CloseEnabled && enabled && !systemCaptionItem.IsRightToLeft && m_ColorTable.SystemButtonClose != null)
			{
				office2007SystemButtonColorTable = m_ColorTable.SystemButtonClose;
			}
			ct = office2007SystemButtonColorTable.Default;
			if ((systemCaptionItem.CloseEnabled && enabled && !systemCaptionItem.IsRightToLeft) || systemCaptionItem.IsRightToLeft)
			{
				if ((systemCaptionItem.MouseDownButton == SystemButton.Close && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.MouseDownButton == SystemButton.Minimize && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.Pressed;
				}
				else if ((systemCaptionItem.MouseOverButton == SystemButton.Close && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.MouseOverButton == SystemButton.Minimize && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.MouseOver;
				}
			}
			PaintBackground(graphics, r, ct);
			if (systemCaptionItem.IsRightToLeft)
			{
				if (systemCaptionItem.HelpVisible)
				{
					PaintHelp(graphics, r, ct, enabled);
				}
				else
				{
					PaintMinimize(graphics, r, ct, enabled);
				}
			}
			else
			{
				PaintClose(graphics, r, ct, systemCaptionItem.CloseEnabled && enabled);
			}
			office2007SystemButtonColorTable = office2007SystemButtonColorTable3;
		}
		graphics.Clip = clip;
		if (clip != null)
		{
			clip.Dispose();
		}
	}

	protected virtual void PaintBackground(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
	{
		int roundCornerSize = 2;
		Rectangle bounds = r;
		if (ct.OuterBorder != null && !ct.OuterBorder.IsEmpty)
		{
			r.Inflate(-1, -1);
		}
		Rectangle r2 = new Rectangle(r.X, r.Y, r.Width, r.Height / 2);
		if (ct.TopBackground != null && !ct.TopBackground.IsEmpty)
		{
			r2.Height++;
			if (ct.BottomBackground == null)
			{
				r2 = r;
			}
			DisplayHelp.FillRectangle(g, r2, ct.TopBackground);
			r2.Height--;
		}
		if (ct.BottomBackground != null && !ct.BottomBackground.IsEmpty)
		{
			r2.Y += r2.Height;
			r2.Height = r.Height - r2.Height;
			DisplayHelp.FillRectangle(g, r2, ct.BottomBackground);
		}
		if (ct.TopHighlight != null && !ct.TopHighlight.IsEmpty)
		{
			Rectangle r3 = r;
			r3.Height /= 2;
			DrawHighlight(g, ct.TopHighlight, r3, new PointF(r3.X + r3.Width / 2, r3.Bottom));
		}
		if (ct.BottomHighlight != null && !ct.BottomHighlight.IsEmpty)
		{
			Rectangle r4 = r;
			r4.Height /= 2;
			r4.Y += r.Height - r4.Height;
			DrawHighlight(g, ct.BottomHighlight, r4, new PointF(r4.X + r4.Width / 2, r4.Bottom));
		}
		if (ct.OuterBorder != null && !ct.OuterBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundGradientRectangle(g, bounds, ct.OuterBorder, 1, roundCornerSize);
			bounds.Inflate(-1, -1);
		}
		if (ct.InnerBorder != null && !ct.InnerBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundGradientRectangle(g, bounds, ct.InnerBorder, 1, roundCornerSize);
		}
	}

	private void DrawHighlight(Graphics g, LinearGradientColorTable c, Rectangle r, PointF centerPoint)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(r.X, r.Y, r.Width, r.Height * 2);
		GraphicsPath val = new GraphicsPath();
		val.AddEllipse(rectangle);
		PathGradientBrush val2 = new PathGradientBrush(val);
		val2.CenterColor = c.Start;
		val2.SurroundColors = new Color[1] { c.End };
		val2.CenterPoint = centerPoint;
		Blend val3 = new Blend();
		val3.Factors = new float[3] { 0f, 0.5f, 1f };
		val3.Positions = new float[3] { 0f, 0.4f, 1f };
		val2.Blend = val3;
		g.FillRectangle((Brush)(object)val2, r);
		((Brush)val2).Dispose();
		val.Dispose();
	}

	protected virtual void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(7, 3);
		Rectangle signRect = GetSignRect(r, s);
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right, signRect.Y, ct.DarkShade, 1);
		signRect.Offset(0, 1);
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right, signRect.Y, ct.Foreground.Start, 1);
		signRect.Offset(0, 1);
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right, signRect.Y, ct.LightShade, 1);
		g.SmoothingMode = smoothingMode;
	}

	protected virtual Rectangle GetSignRect(Rectangle r, Size s)
	{
		if (r.Height < 10)
		{
			return Rectangle.Empty;
		}
		return new Rectangle(r.X + (r.Width - s.Width) / 2, r.Bottom - r.Height / 4 - s.Height, s.Width, s.Height);
	}

	protected virtual void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(9, 8);
		Rectangle signRect = GetSignRect(r, s);
		Rectangle rectangle = new Rectangle(signRect.X, signRect.Y + 1, 7, 7);
		Region clip = g.Clip;
		for (int i = 0; i < 2; i++)
		{
			DisplayHelp.DrawGradientRectangle(g, new Rectangle(rectangle.X, rectangle.Y + 1, rectangle.Width, rectangle.Height - 1), ct.Foreground, 1);
			DisplayHelp.DrawLine(g, rectangle.X, rectangle.Y, rectangle.Right - 1, rectangle.Y, ct.DarkShade, 1);
			DisplayHelp.DrawLine(g, rectangle.X + 1, rectangle.Y + 2, rectangle.Right - 2, rectangle.Y + 2, ct.LightShade, 1);
			DisplayHelp.DrawLine(g, rectangle.X + 1, rectangle.Y + 2, rectangle.Right - 2, rectangle.Y + 2, ct.LightShade, 1);
			g.SetClip(rectangle, (CombineMode)4);
			rectangle.Offset(2, -1);
		}
		if (clip != null)
		{
			g.Clip = clip;
		}
		else
		{
			g.ResetClip();
		}
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(9, 8);
		Rectangle signRect = GetSignRect(r, s);
		Color color = (isEnabled ? ct.DarkShade : Color.FromArgb(128, ct.DarkShade));
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right - 1, signRect.Y, color, 1);
		signRect.Y++;
		signRect.Height--;
		color = (isEnabled ? ct.Foreground.Start : Color.FromArgb(128, ct.Foreground.Start));
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right - 1, signRect.Y, color, 1);
		signRect.Y++;
		signRect.Height--;
		Rectangle bounds = signRect;
		bounds.Height--;
		if (isEnabled)
		{
			DisplayHelp.DrawGradientRectangle(g, bounds, ct.Foreground, 1);
		}
		else
		{
			DisplayHelp.DrawGradientRectangle(g, bounds, Color.FromArgb(128, ct.Foreground.Start), Color.FromArgb(128, ct.Foreground.End), ct.Foreground.GradientAngle, 1);
		}
		color = (isEnabled ? ct.LightShade : Color.FromArgb(128, ct.LightShade));
		DisplayHelp.DrawLine(g, signRect.X, signRect.Bottom - 1, signRect.Right - 1, signRect.Bottom - 1, color, 1);
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintClose(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
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
			val.AddLine(rectangle.X, rectangle.Y, rectangle.X + 2, rectangle.Y);
			val.AddLine(rectangle.X + 2, rectangle.Y, rectangle.X + 4, rectangle.Y + 2);
			val.AddLine(rectangle.X + 4, rectangle.Y + 2, rectangle.X + 6, rectangle.Y);
			val.AddLine(rectangle.X + 6, rectangle.Y, rectangle.X + 8, rectangle.Y);
			val.AddLine(rectangle.X + 8, rectangle.Y, rectangle.X + 5, rectangle.Y + 3);
			val.AddLine(rectangle.X + 5, rectangle.Y + 4, rectangle.X + 8, rectangle.Y + 7);
			val.AddLine(rectangle.X + 8, rectangle.Y + 7, rectangle.X + 6, rectangle.Y + 7);
			val.AddLine(rectangle.X + 6, rectangle.Y + 7, rectangle.X + 4, rectangle.Y + 5);
			val.AddLine(rectangle.X + 4, rectangle.Y + 5, rectangle.X + 2, rectangle.Y + 7);
			val.AddLine(rectangle.X + 2, rectangle.Y + 7, rectangle.X, rectangle.Y + 7);
			val.AddLine(rectangle.X, rectangle.Y + 7, rectangle.X + 3, rectangle.Y + 4);
			val.AddLine(rectangle.X + 3, rectangle.Y + 3, rectangle.X, rectangle.Y);
			if (isEnabled)
			{
				DisplayHelp.FillPath(g, val, ct.Foreground);
				DisplayHelp.DrawGradientPathBorder(g, val, ct.Foreground, 1);
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
		if (!ct.DarkShade.IsEmpty)
		{
			Pen val2 = new Pen(isEnabled ? ct.DarkShade : Color.FromArgb(128, ct.DarkShade), 1f);
			try
			{
				g.DrawLine(val2, rectangle.X, rectangle.Y, rectangle.X + 2, rectangle.Y);
				g.DrawLine(val2, rectangle.X + 2, rectangle.Y, rectangle.X + 4, rectangle.Y + 2);
				g.DrawLine(val2, rectangle.X + 4, rectangle.Y + 2, rectangle.X + 6, rectangle.Y);
				g.DrawLine(val2, rectangle.X + 6, rectangle.Y, rectangle.X + 8, rectangle.Y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!ct.LightShade.IsEmpty)
		{
			Pen val3 = new Pen(isEnabled ? ct.LightShade : Color.FromArgb(128, ct.LightShade), 1f);
			try
			{
				g.DrawLine(val3, signRect.X, signRect.Y + 8, signRect.X + 3, signRect.Y + 8);
				g.DrawLine(val3, signRect.X + 3, signRect.Y + 8, signRect.X + 5, signRect.Y + 6);
				g.DrawLine(val3, signRect.X + 5, signRect.Y + 6, signRect.X + 7, signRect.Y + 8);
				g.DrawLine(val3, signRect.X + 7, signRect.Y + 8, signRect.X + 10, signRect.Y + 8);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintHelp(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
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
			Color color = (isEnabled ? ct.LightShade : Color.FromArgb(128, ct.LightShade));
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
			color = (isEnabled ? ct.Foreground.Start : Color.FromArgb(128, ct.Foreground.Start));
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
